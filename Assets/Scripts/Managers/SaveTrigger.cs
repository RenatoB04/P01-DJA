using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private bool jogadorPerto = false; // Indica se o jogador está dentro da zona de interação

    void Update()
    {
        // Se o jogador estiver perto e pressionar E, recomeça o jogo
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            RecomecarJogo();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Quando o jogador entra na área de colisão, mostra mensagem de interação
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = true;
            MensagemInteracaoUI.instancia.Mostrar("Pressiona E para recomeçar o jogo!");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Quando o jogador sai da área, remove a mensagem e bloqueia a interação
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = false;
            MensagemInteracaoUI.instancia.Esconder();
        }
    }

    void RecomecarJogo()
    {
        int dinheiroInicial = 1000;

        // Repõe o dinheiro no GameManager
        GameManager.instancia.DefinirDinheiro(dinheiroInicial);

        // Desativa todos os power-ups, caso estejam ativos
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

        // Apaga a data da última utilização da roda diária
        PlayerPrefs.DeleteKey("DataUltimaRoda");
        PlayerPrefs.Save();

        // Grava os dados com valores "limpos"
        SaveManager.GuardarJogo(
            dinheiroInicial,
            false, 0, "", "",
            false, 0, "", ""
        );

        // Limpa o registo de moedas apanhadas
        SaveManager.CarregarJogo().moedasApanhadas.Clear();

        // Grava novamente para refletir a lista limpa
        SaveManager.GuardarJogo(
            dinheiroInicial,
            false, 0, "", "",
            false, 0, "", ""
        );

        Debug.Log("Jogo reiniciado.");
        MensagemInteracaoUI.instancia.MostrarTemporario("Jogo reiniciado com sucesso!", 2f);
    }
}