using UnityEngine;

public class NPCWelcome : MonoBehaviour
{
    public GameObject textoUI;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            textoUI.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            textoUI.SetActive(false);
        }
    }
}
