using UnityEngine;

public class SlotInteraction : MonoBehaviour
{
    public GameObject textoUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textoUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textoUI.SetActive(false);
        }
    }
}