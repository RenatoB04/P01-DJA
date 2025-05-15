using UnityEngine;
using UnityEngine.SceneManagement;

public class IrParaRoda : MonoBehaviour
{
    private bool jogadorPerto = false;

    public GameObject textoUI;

    void Start()
    {
        textoUI.SetActive(false);
    }

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("RodaScene");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            jogadorPerto = true;
            textoUI.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            jogadorPerto = false;
            textoUI.SetActive(false);
        }
    }
}