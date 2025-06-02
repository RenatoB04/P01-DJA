using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalEscapeHandler : MonoBehaviour
{
    void Awake()
    {
        // Garante que este objecto não é destruído ao mudar de cena
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        // Se o jogador pressionar a tecla Escape, volta para o Menu principal
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}