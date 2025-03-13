using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Jogo");
    }

    public void LoadOptionsScene()
    {
        SceneManager.LoadScene("Opcoes");
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("Creditos");
    }
}