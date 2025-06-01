using UnityEngine;

public class FichaRecolher : MonoBehaviour
{
    public string idUnico;
    public int valor = 50;

    private void Start()
    {
        if (SaveManager.MoedaFoiApanhada(idUnico))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instancia.AdicionarDinheiro(valor);
            SaveManager.MarcarMoedaComoApanhada(idUnico);
            gameObject.SetActive(false);
        }
    }
}