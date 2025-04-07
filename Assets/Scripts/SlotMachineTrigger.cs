using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotMachineTrigger : MonoBehaviour
{
    private bool jogadorPerto = false;

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("SlotScene");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            Debug.Log("Aproximaste-te da slot machine. Pressiona E para jogar.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
        }
    }
}
