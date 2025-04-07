using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotMachineTrigger : MonoBehaviour
{
    private bool jogadorPerto = false;

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            // Aqui fazes o que quiseres: abrir UI ou mudar de cena
            SceneManager.LoadScene("SlotScene"); // Exemplo
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
