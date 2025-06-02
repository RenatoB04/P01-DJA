using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Carrega a cena principal do jogo
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Jogo");
    }

    // Carrega a cena das opções/configurações
    public void LoadOptionsScene()
    {
        SceneManager.LoadScene("Opcoes");
    }

    // Carrega a cena dos créditos
    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("Creditos");
    }
}