using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalEscapeHandler : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}