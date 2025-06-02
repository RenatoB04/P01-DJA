using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotMachineTrigger : MonoBehaviour
{
    private bool jogadorPerto = false; // Indica se o jogador está na zona de interação

    void Update()
    {
        // Se o jogador estiver perto e pressionar E, entra na cena da máquina de slots
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("SlotMachine");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Quando o jogador entra na área, ativa a interação e mostra mensagem
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = true;
            MensagemInteracaoUI.instancia.Mostrar("Pressiona E para jogar nas Slots");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Quando o jogador sai da área, desativa a interação e esconde mensagem
        if (collision.collider.CompareTag("Player"))
        {
            jogadorPerto = false;
            MensagemInteracaoUI.instancia.Esconder();
        }
    }
}