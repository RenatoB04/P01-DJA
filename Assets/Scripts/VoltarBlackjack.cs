using UnityEngine;
using UnityEngine.SceneManagement;

public class VoltarAoMenu : MonoBehaviour
{
    public string nomeCena = "Jogo";

    public void Voltar()
    {
        SceneManager.LoadScene(nomeCena);
    }
}
