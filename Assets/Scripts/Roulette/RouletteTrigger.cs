using UnityEngine;
using UnityEngine.SceneManagement;

public class RouletteTrigger : MonoBehaviour
{
    private bool jogadorPerto = false; // Indica se o jogador está dentro da área de interação

    void Update()
    {
        // Quando o jogador está perto e pressiona a tecla E, carrega a cena da roleta
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("RouletteScene");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ativa a interação ao entrar na área e mostra a mensagem
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = true;
            MensagemInteracaoUI.instancia.Mostrar("Pressiona E para jogar na Roleta");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Desativa a interação ao sair da área e esconde a mensagem
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = false;
            MensagemInteracaoUI.instancia.Esconder();
        }
    }
}