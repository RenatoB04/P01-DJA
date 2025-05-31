using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class RodaDiaria : MonoBehaviour
{
    [Header("Referências da UI")]
    public TextMeshProUGUI textoResultado;
    public TextMeshProUGUI textoSaldo;
    public TextMeshProUGUI textoTempoRestante;
    public Button botaoRodar;
    public Button botaoVoltar;
    public Transform imagemRoda;

    [Header("Configuração da Roda")]
    private int[] premios = { 5000, 50, 250, 1000, 50, 500, 250, 50 };

    private bool modoDebug = false;

    private DateTime proximaRotacao;
    private bool podeRodar = false;

    void Start()
    {
        botaoRodar.onClick.AddListener(Rodar);
        botaoVoltar.onClick.AddListener(VoltarParaCenaJogo);

        if (!modoDebug)
        {
            string dataGuardada = PlayerPrefs.GetString("DataUltimaRoda", "");
            if (!string.IsNullOrEmpty(dataGuardada))
            {
                proximaRotacao = DateTime.Parse(dataGuardada).AddHours(24);
            }
            else
            {
                proximaRotacao = DateTime.MinValue;
            }
        }
        else
        {
            proximaRotacao = DateTime.Now;
        }

        AtualizarSaldoUI();
        StartCoroutine(VerificarTempoRestante());
    }

    void Rodar()
    {
        if (!podeRodar) return;
        StartCoroutine(RodarAnimacao());
    }

    IEnumerator RodarAnimacao()
    {
        podeRodar = false;
        botaoRodar.interactable = false;
        textoResultado.text = "A rodar...";

        int totalFatias = premios.Length;
        float anguloPorFatia = 360f / totalFatias;

        float anguloFinal = UnityEngine.Random.Range(0f, 360f);
        float voltas = 5f;
        float anguloInicial = imagemRoda.eulerAngles.z;
        float anguloDestino = anguloInicial - (360f * voltas + anguloFinal);

        float tempoTotal = 3.5f;
        float tempo = 0f;

        while (tempo < tempoTotal)
        {
            tempo += Time.deltaTime;
            float t = tempo / tempoTotal;
            float curva = EaseOutQuint(t);
            float anguloAtual = Mathf.Lerp(anguloInicial, anguloDestino, curva);
            imagemRoda.eulerAngles = new Vector3(0, 0, anguloAtual);
            yield return null;
        }

        imagemRoda.eulerAngles = new Vector3(0, 0, anguloDestino % 360f);

        float anguloPositivo = Mathf.Abs(anguloDestino % 360f);
        int indicePremio = Mathf.FloorToInt(anguloPositivo / anguloPorFatia) % totalFatias;

        int premio = premios[indicePremio];
        if (premio == 5000)
        {
            textoResultado.text = "JACKPOT! Ganhaste 5000 moedas!";
        }
        else
        {
            textoResultado.text = $"Ganhaste {premio} moedas!";
        }

        if (premio > 0)
        {
            GameManager.instancia.AdicionarDinheiro(premio);
        }

        AtualizarSaldoUI();

        if (!modoDebug)
        {
            proximaRotacao = DateTime.Now.AddHours(24);
            PlayerPrefs.SetString("DataUltimaRoda", DateTime.Now.ToString());
            PlayerPrefs.Save();
        }

        StartCoroutine(VerificarTempoRestante());
    }

    IEnumerator VerificarTempoRestante()
    {
        while (true)
        {
            TimeSpan diferenca = proximaRotacao - DateTime.Now;

            if (modoDebug || diferenca.TotalSeconds <= 0)
            {
                podeRodar = true;
                botaoRodar.interactable = true;
                textoTempoRestante.text = "Podes rodar!";
                yield break;
            }
            else
            {
                podeRodar = false;
                botaoRodar.interactable = false;
                textoTempoRestante.text = $"Podes rodar em: {diferenca.Hours:D2}:{diferenca.Minutes:D2}:{diferenca.Seconds:D2}";
            }

            yield return new WaitForSeconds(1f);
        }
    }

    float EaseOutQuint(float t) => 1 - Mathf.Pow(1 - t, 5);

    [ContextMenu("Limpar memória da roda")]
    void LimparMemoriaDaRoda()
    {
        PlayerPrefs.DeleteKey("DataUltimaRoda");
        PlayerPrefs.Save();
        Debug.Log("Memória da roda limpa!");
    }
    
    void AtualizarSaldoUI()
    {
        if (textoSaldo != null && GameManager.instancia != null)
        {
            int saldo = GameManager.instancia.ObterDinheiro();
            textoSaldo.text = $"Moedas: {saldo}";
        }
    }

    void VoltarParaCenaJogo()
    {
        SceneManager.LoadScene("Jogo");
    }
}