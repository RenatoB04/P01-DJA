using UnityEngine;

public class FichaRecolher : MonoBehaviour
{
    public int valor = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instancia.AdicionarDinheiro(valor);
            Destroy(gameObject);
        }
    }
}
