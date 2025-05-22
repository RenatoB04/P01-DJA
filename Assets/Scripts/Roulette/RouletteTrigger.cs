using UnityEngine;
using UnityEngine.SceneManagement;

public class RouletteTrigger : MonoBehaviour
{
    private bool jogadorPerto = false;

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("RouletteScene");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = true;
            MensagemInteracaoUI.instancia.Mostrar("Pressiona E para jogar na Roleta");
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
}