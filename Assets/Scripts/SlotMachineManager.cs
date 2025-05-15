using System.Collections;
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
        Debug.Log("SlotMachineManager começou");
        Time.timeScale = 1f;
        textoResultado.text = "";
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
        }
    }

    IEnumerator RodarSlots()
    {
        textoResultado.text = "";
        botaoJogar.interactable = false;

        foreach (SlotReel coluna in colunas)
            StartCoroutine(coluna.Rolar());

        yield return new WaitForSeconds(1.5f);

        Sprite s1 = colunas[0].GetSimboloDoMeio();
        Sprite s2 = colunas[1].GetSimboloDoMeio();
        Sprite s3 = colunas[2].GetSimboloDoMeio();

        if (s1 == s2 && s2 == s3)
        {
            textoResultado.text = "Ganhaste!";
            GameManager.instancia.AdicionarDinheiro(250);
        }
        else
        {
            textoResultado.text = "Perdeste!";
        }

        AtualizarTextoDinheiro();
        botaoJogar.interactable = true;
    }

    void AtualizarTextoDinheiro()
    {
        textoDinheiro.text = "Moedas: " + GameManager.instancia.ObterDinheiro();
    }
}   