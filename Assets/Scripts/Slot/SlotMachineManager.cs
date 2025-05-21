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
        if (GameManager.instancia.ObterDinheiro() >= 100)
        {
            GameManager.instancia.RemoverDinheiro(100);
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

        int premioTotal = 0;

        for (int linha = 0; linha < 3; linha++)
        {
            Sprite s1 = grelha[0][linha];
            Sprite s2 = grelha[1][linha];
            Sprite s3 = grelha[2][linha];

            if (s1 == s2 && s2 == s3)
            {
                premioTotal += 150;
            }
        }
        
        if (grelha[0][0] == grelha[1][1] && grelha[1][1] == grelha[2][2])
        {
            premioTotal += 300;
        }

        if (grelha[0][2] == grelha[1][1] && grelha[1][1] == grelha[2][0])
        {
            premioTotal += 300;
        }

        if (premioTotal > 0)
        {
            textoResultado.text = $"Ganhaste {premioTotal} moedas!";
            GameManager.instancia.AdicionarDinheiro(premioTotal);
        }
        else
        {
            textoResultado.text = "Perdeste!";
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