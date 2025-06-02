using UnityEngine;
using UnityEngine.SceneManagement;

public class DailyWheelTrigger : MonoBehaviour
{
    private bool jogadorPerto = false; // Indica se o jogador está na área de interação com a roda diária

    void Update()
    {
        // Se o jogador estiver na área e pressionar E, muda para a cena da Roda Diária
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("DailyWheel");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Quando o jogador entra na área, ativa a interação e mostra a mensagem
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = true;
            MensagemInteracaoUI.instancia.Mostrar("Pressiona E para jogar na Roda Diária!");
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