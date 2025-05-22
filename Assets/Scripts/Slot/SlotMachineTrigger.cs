using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotMachineTrigger : MonoBehaviour
{
    private bool jogadorPerto = false;

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("SlotMachine");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = true;
            MensagemInteracaoUI.instancia.Mostrar("Pressiona E para jogar nas Slots");
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