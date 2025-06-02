using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackjackTrigger : MonoBehaviour
{
    private bool jogadorPerto = false; // Indica se o jogador está dentro da zona de interação

    void Update()
    {
        // Se o jogador estiver perto e pressionar a tecla E, muda para a cena do Blackjack
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("BlackjackScene");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Quando o jogador entra em contacto com o trigger, ativa a possibilidade de interação
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = true;
            MensagemInteracaoUI.instancia.Mostrar("Pressiona E para jogar BlackJack"); // Mostra mensagem de interação
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Quando o jogador sai do trigger, desativa a interação e esconde a mensagem
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = false;
            MensagemInteracaoUI.instancia.Esconder();
        }
    }
}