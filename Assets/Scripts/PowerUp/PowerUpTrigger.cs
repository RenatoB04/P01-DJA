using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpTrigger : MonoBehaviour
{
    private bool jogadorPerto = false; // Indica se o jogador está na área de interação

    void Update()
    {
        // Se o jogador estiver perto e pressionar a tecla E, muda para a cena da loja de Power-Ups
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("PowerUp");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Quando o jogador entra na área, ativa a possibilidade de interação e mostra a mensagem
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = true;
            MensagemInteracaoUI.instancia.Mostrar("Pressiona E para comprar Power-Ups!");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Quando o jogador sai da área, desativa a interação e esconde a mensagem
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = false;
            MensagemInteracaoUI.instancia.Esconder();
        }
    }
}