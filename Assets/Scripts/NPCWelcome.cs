using UnityEngine;

public class NPCWelcome : MonoBehaviour
{
    public GameObject textoUI; // Referência ao GameObject do texto que será mostrado ao jogador

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Quando o jogador colide com o NPC, mostra o texto de boas-vindas
        if (collision.collider.CompareTag("Player"))
        {
            textoUI.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Quando o jogador se afasta do NPC, esconde o texto
        if (collision.collider.CompareTag("Player"))
        {
            textoUI.SetActive(false);
        }
    }
}