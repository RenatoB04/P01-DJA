using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SlotMachineManager : MonoBehaviour
{
    public SlotReel[] colunas;
    public Button botaoJogar;
    public Button botaoVoltar;
    public TextMeshProUGUI textoResultado;
    public TextMeshProUGUI textoDinheiro;
    public TMP_InputField inputAposta;

    private int valorAposta = 0;

    void Start()
    {
        Time.timeScale = 1f;
        textoResultado.text = "";
        textoResultado.gameObject.SetActive(false);
        botaoJogar.onClick.AddListener(TentarJogar);
        botaoVoltar.onClick.AddListener(() => SceneManager.LoadScene("Jogo"));
        AtualizarTextoDinheiro();
    }

    void TentarJogar()
    {
        if (!int.TryParse(inputAposta.text, out valorAposta) || valorAposta <= 0)
        {
            textoResultado.text = "Insere um valor de aposta válido.";
            textoResultado.gameObject.SetActive(true);
            return;
        }

        int saldoAtual = GameManager.instancia.ObterDinheiro();

        if (saldoAtual >= valorAposta)
        {
            GameManager.instancia.RemoverDinheiro(valorAposta);
            AtualizarTextoDinheiro();
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

        foreach (SlotReel coluna in colunas)
            StartCoroutine(coluna.Rolar());

        yield return new WaitForSeconds(1.5f);

        Sprite[][] grelha = new Sprite[3][];
        for (int i = 0; i < 3; i++)
        {
            grelha[i] = colunas[i].simbolos.Select(img => img.sprite).ToArray();
        }

        int multiplicadorTotal = 0;
        
        for (int linha = 0; linha < 3; linha++)
        {
            Sprite s1 = grelha[0][linha];
            Sprite s2 = grelha[1][linha];
            Sprite s3 = grelha[2][linha];

            if (s1 == s2 && s2 == s3)
            {
                multiplicadorTotal += 3;
            }
        }
        
        if (grelha[0][0] == grelha[1][1] && grelha[1][1] == grelha[2][2])
        {
            multiplicadorTotal += 5;
        }

        if (grelha[0][2] == grelha[1][1] && grelha[1][1] == grelha[2][0])
        {
            multiplicadorTotal += 5;
        }

        if (multiplicadorTotal > 0)
        {
            int ganho = valorAposta * multiplicadorTotal;
            textoResultado.text = $"Ganhaste {ganho} moedas!\n(Pagamento: x{multiplicadorTotal})";
            GameManager.instancia.AdicionarDinheiro(ganho);
        }
        else
        {
            textoResultado.text = $"Perdeste!\n- {valorAposta} moedas.";
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