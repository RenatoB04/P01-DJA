using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SlotMachineManager : MonoBehaviour
{
    public SlotReel[] colunas;                        // Referência às colunas da slot machine
    public Button botaoJogar;
    public Button botaoVoltar;
    public TextMeshProUGUI textoResultado;
    public TextMeshProUGUI textoDinheiro;
    public TMP_InputField inputAposta;

    [Header("Áudio")]
    public AudioClip somPull;                         // Som da alavanca
    public AudioClip somWin;                          // Som de vitória
    private AudioSource audioSource;

    private int valorAposta = 0;

    void Start()
    {
        Time.timeScale = 1f;
        textoResultado.text = "";
        textoResultado.gameObject.SetActive(false);

        botaoJogar.onClick.AddListener(TentarJogar);
        botaoVoltar.onClick.AddListener(() => SceneManager.LoadScene("Jogo"));

        AtualizarTextoDinheiro();
        audioSource = GetComponent<AudioSource>();
    }

    void TentarJogar()
    {
        // Valida a aposta introduzida
        if (!int.TryParse(inputAposta.text, out valorAposta) || valorAposta <= 0)
        {
            textoResultado.text = "Insere um valor de aposta válido.";
            textoResultado.gameObject.SetActive(true);
            return;
        }

        int saldoAtual = GameManager.instancia.ObterDinheiro();

        if (saldoAtual >= valorAposta)
        {
            // Toca som da alavanca
            if (somPull != null && audioSource != null)
                audioSource.PlayOneShot(somPull);

            GameManager.instancia.RemoverDinheiro(valorAposta);
            AtualizarTextoDinheiro();

            // Inicia o processo de rotação das colunas
            StartCoroutine(RodarSlots());
        }
        else
        {
            textoResultado.text = "Saldo insuficiente!";
            textoResultado.gameObject.SetActive(true);
        }
    }

    IEnumerator RodarSlots()
    {
        textoResultado.text = "";
        textoResultado.gameObject.SetActive(false);
        botaoJogar.interactable = false;

        // Inicia a rotação das 3 colunas
        foreach (SlotReel coluna in colunas)
            StartCoroutine(coluna.Rolar());

        yield return new WaitForSeconds(1.5f); // Espera que as colunas terminem

        // Constrói a grelha de símbolos (3 colunas x 3 linhas)
        Sprite[][] grelha = new Sprite[3][];
        for (int i = 0; i < 3; i++)
            grelha[i] = colunas[i].simbolos.Select(img => img.sprite).ToArray();

        int multiplicadorTotal = 0;

        // Verifica combinações horizontais
        for (int linha = 0; linha < 3; linha++)
        {
            Sprite s1 = grelha[0][linha];
            Sprite s2 = grelha[1][linha];
            Sprite s3 = grelha[2][linha];

            if (s1 == s2 && s2 == s3)
                multiplicadorTotal += 3;
        }

        // Verifica diagonais
        if (grelha[0][0] == grelha[1][1] && grelha[1][1] == grelha[2][2])
            multiplicadorTotal += 5;

        if (grelha[0][2] == grelha[1][1] && grelha[1][1] == grelha[2][0])
            multiplicadorTotal += 5;

        // Apresenta o resultado
        if (multiplicadorTotal > 0)
        {
            int ganho = valorAposta * multiplicadorTotal;

            // Toca som de vitória
            if (somWin != null && audioSource != null)
                audioSource.PlayOneShot(somWin);

            textoResultado.text = $"Ganhaste {ganho} moedas!\n(x{multiplicadorTotal})";
            GameManager.instancia.AdicionarDinheiro(ganho);
        }
        else
        {
            textoResultado.text = $"Perdeste {valorAposta} moedas.";
        }

        textoResultado.gameObject.SetActive(true);
        AtualizarTextoDinheiro();
        botaoJogar.interactable = true;
    }

    void AtualizarTextoDinheiro()
    {
        textoDinheiro.text = "Moedas: " + GameManager.instancia.ObterDinheiro();
    }
}
