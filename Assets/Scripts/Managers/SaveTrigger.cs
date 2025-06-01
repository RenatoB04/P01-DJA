using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private bool jogadorPerto = false;

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            RecomecarJogo();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = true;
            MensagemInteracaoUI.instancia.Mostrar("Pressiona E para recomeçar o jogo!");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = false;
            MensagemInteracaoUI.instancia.Esconder();
        }
    }

    void RecomecarJogo()
    {
        int dinheiroInicial = 1000;

        GameManager.instancia.DefinirDinheiro(dinheiroInicial);

        if (PowerUpManager.instancia != null)
        {
            PowerUpManager.instancia.ganhoExtraAtivo = false;
            PowerUpManager.instancia.ganhoExtraPercentagem = 0;
            PowerUpManager.instancia.ganhoExtraDuracao = "";
            PowerUpManager.instancia.ganhoExtraDataCompra = "";

            PowerUpManager.instancia.protecaoPerdaAtiva = false;
            PowerUpManager.instancia.protecaoPerdaPercentagem = 0;
            PowerUpManager.instancia.protecaoPerdaDuracao = "";
            PowerUpManager.instancia.protecaoPerdaDataCompra = "";
        }
        
        SaveManager.GuardarJogo(
            dinheiroInicial,
            false, 0, "", "",
            false, 0, "", ""
        );

        SaveManager.CarregarJogo().moedasApanhadas.Clear();

        SaveManager.GuardarJogo(
            dinheiroInicial,
            false, 0, "", "",
            false, 0, "", ""
        );

        Debug.Log("Jogo reiniciado.");
        MensagemInteracaoUI.instancia.MostrarTemporario("Jogo reiniciado com sucesso!", 2f);
    }
}