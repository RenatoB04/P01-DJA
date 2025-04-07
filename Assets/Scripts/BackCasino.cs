using UnityEngine;
using UnityEngine.SceneManagement;

public class BackCasino : MonoBehaviour
{
    public void Voltar()
    {
        SceneManager.LoadScene("Jogo");
    }
}
