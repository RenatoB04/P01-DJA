using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LojaPowerUps : MonoBehaviour
{
    public TMP_Text textoSaldo;
    public Button botaoGanhoExtra;
    public Button botaoProtecaoPerda;
    public Button botaoVoltar;

    private void Start()
    {
        AtualizarUI();

        botaoGanhoExtra.onClick.AddListener(ComprarGanhoExtra);
        botaoProtecaoPerda.onClick.AddListener(ComprarProtecaoPerda);
        botaoVoltar.onClick.AddListener(() => SceneManager.LoadScene("Jogo"));
    }

    void AtualizarUI()
    {
        textoSaldo.text = "Saldo: " + GameManager.instancia.ObterDinheiro() + " moedas";

        if (PowerUpManager.instancia.ganhoExtraAtivo)
        {
            botaoGanhoExtra.interactable = false;
            botaoGanhoExtra.GetComponentInChildren<TMP_Text>().text = "Já comprado";
        }

        if (PowerUpManager.instancia.protecaoPerdaAtiva)
        {
            botaoProtecaoPerda.interactable = false;
            botaoProtecaoPerda.GetComponentInChildren<TMP_Text>().text = "Já comprado";
        }
    }

    void ComprarGanhoExtra()
    {
        if (GameManager.instancia.ObterDinheiro() >= 200)
        {
            GameManager.instancia.RemoverDinheiro(200);
            PowerUpManager.instancia.ganhoExtraAtivo = true;
            SaveManager.GuardarJogo(
                GameManager.instancia.ObterDinheiro(),
                true,
                PowerUpManager.instancia.protecaoPerdaAtiva
            );
            AtualizarUI();
        }
    }

    void ComprarProtecaoPerda()
    {
        if (GameManager.instancia.ObterDinheiro() >= 200)
        {
            GameManager.instancia.RemoverDinheiro(200);
            PowerUpManager.instancia.protecaoPerdaAtiva = true;
            SaveManager.GuardarJogo(
                GameManager.instancia.ObterDinheiro(),
                PowerUpManager.instancia.ganhoExtraAtivo,
                true
            );
            AtualizarUI();
        }
    }
}