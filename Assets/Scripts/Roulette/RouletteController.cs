using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class RoletaController : MonoBehaviour
{
    public Image roletaNumeros;
    public RectTransform bolinha;
    public TMP_InputField inputAposta;
    public TMP_Dropdown dropdownAposta;
    public Button botaoJogar;
    public Button botaoVoltar;
    public TMP_Text textoResultado;
    public TMP_Text textoSaldo;

    [Header("Áudio")]
    public AudioClip somRoleta;
    public AudioClip somWin;
    private AudioSource audioSource;

    private bool aRodar = false;

    private readonly int[] ordemRoleta = {
        0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6,
        27, 13, 36, 11, 30, 8, 23, 10, 5, 24,
        16, 33, 1, 20, 14, 31, 9, 22, 18, 29,
        7, 28, 12, 35, 3, 26
    };

    void Start()
    {
        botaoJogar.onClick.AddListener(GirarRoleta);
        botaoVoltar.onClick.AddListener(VoltarParaJogo);
        AtualizarTextoSaldo();

        audioSource = GetComponent<AudioSource>();
    }

    void GirarRoleta()
    {
        if (aRodar) return;

        if (!int.TryParse(inputAposta.text, out int valorAposta) || valorAposta <= 0)
        {
            textoResultado.text = "Valor de aposta inválido.";
            return;
        }

        int saldoAtual = GameManager.instancia.ObterDinheiro();

        if (valorAposta > saldoAtual)
        {
            textoResultado.text = "Saldo insuficiente.";
            return;
        }

        GameManager.instancia.RemoverDinheiro(valorAposta);
        AtualizarTextoSaldo();

        StartCoroutine(RodarAnimacao(valorAposta));
    }

    IEnumerator RodarAnimacao(int valorAposta)
    {
        aRodar = true;
        textoResultado.text = "A girar...";

        if (somRoleta != null && audioSource != null)
        {
            audioSource.PlayOneShot(somRoleta);
        }

        float anguloInicial = roletaNumeros.rectTransform.eulerAngles.z;
        float voltas = Random.Range(4f, 5f);
        float anguloTotal = voltas * 360f;
        float anguloFinal = anguloInicial + anguloTotal;

        float duracao = 3.5f;
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float t = tempo / duracao;
            float anguloAtual = Mathf.Lerp(anguloInicial, anguloFinal, EaseOut(t));
            roletaNumeros.rectTransform.rotation = Quaternion.Euler(0, 0, anguloAtual);
            bolinha.rotation = Quaternion.Euler(0, 0, -anguloAtual * 1.5f);
            yield return null;
        }

        float anguloCorrigido = 360f - (anguloFinal % 360f);
        float anguloPorNumero = 360f / 37f;

        int indice = Mathf.RoundToInt(anguloCorrigido / anguloPorNumero) % 37;
        indice = (37 - indice) % 37;
        int resultado = ordemRoleta[indice];

        VerificarAposta(resultado, valorAposta);
        AtualizarTextoSaldo();
        aRodar = false;
    }

    float EaseOut(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }

    void VerificarAposta(int resultado, int valorAposta)
    {
        string aposta = dropdownAposta.options[dropdownAposta.value].text;
        bool ganhou = false;
        int multiplicador = 0;

        string cor = resultado == 0 ? "Verde" : NumeroPreto(resultado) ? "Preto" : "Vermelho";

        string paridade = "-";
        if (resultado != 0)
            paridade = resultado % 2 == 0 ? "Par" : "Ímpar";

        switch (aposta)
        {
            case "Verde":
                ganhou = resultado == 0;
                multiplicador = 36;
                break;
            case "Preto":
                ganhou = NumeroPreto(resultado);
                multiplicador = 2;
                break;
            case "Vermelho":
                ganhou = NumeroVermelho(resultado);
                multiplicador = 2;
                break;
            case "Par":
                ganhou = resultado != 0 && resultado % 2 == 0;
                multiplicador = 2;
                break;
            case "Ímpar":
                ganhou = resultado % 2 != 0;
                multiplicador = 2;
                break;
            case "1 até 18":
                ganhou = resultado >= 1 && resultado <= 18;
                multiplicador = 2;
                break;
            case "19 até 36":
                ganhou = resultado >= 19 && resultado <= 36;
                multiplicador = 2;
                break;
            case "1 até 12":
                ganhou = resultado >= 1 && resultado <= 12;
                multiplicador = 3;
                break;
            case "13 até 24":
                ganhou = resultado >= 13 && resultado <= 24;
                multiplicador = 3;
                break;
            case "25 até 36":
                ganhou = resultado >= 25 && resultado <= 36;
                multiplicador = 3;
                break;
        }

        if (ganhou)
        {
            if (somWin != null && audioSource != null)
            {
                audioSource.PlayOneShot(somWin);
            }

            int ganho = valorAposta * multiplicador;
            GameManager.instancia.AdicionarDinheiro(ganho);
            textoResultado.text =
                $"Número: {resultado} ({cor}, {paridade})\n" +
                $"Aposta: {aposta}\n" +
                $"Ganhaste {ganho} moedas. (x{multiplicador})";
        }
        else
        {
            textoResultado.text =
                $"Número: {resultado} ({cor}, {paridade})\n" +
                $"Aposta: {aposta}\n" +
                $"Perdeste {valorAposta} moedas.";
        }
    }

    void AtualizarTextoSaldo()
    {
        if (textoSaldo != null)
        {
            textoSaldo.text = $"Moedas: {GameManager.instancia.ObterDinheiro()}";
        }
    }

    bool NumeroPreto(int num)
    {
        int[] pretos = { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 };
        return System.Array.Exists(pretos, x => x == num);
    }

    bool NumeroVermelho(int num)
    {
        int[] vermelhos = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
        return System.Array.Exists(vermelhos, x => x == num);
    }

    public void VoltarParaJogo()
    {
        SceneManager.LoadScene("Jogo");
    }
}