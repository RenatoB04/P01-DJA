using UnityEngine;

public class FichaRecolher : MonoBehaviour
{
    public string idUnico; // Identificador único da ficha, usado para verificar se já foi apanhada
    public int valor = 50; // Valor da ficha em moedas

    private void Start()
    {
        // Verifica se esta ficha já foi apanhada anteriormente, usando o sistema de gravação
        if (SaveManager.MoedaFoiApanhada(idUnico))
        {
            gameObject.SetActive(false); // Esconde a ficha do jogo
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Quando o jogador entra em contacto com a ficha
        if (other.CompareTag("Player"))
        {
            GameManager.instancia.AdicionarDinheiro(valor); // Adiciona o valor ao saldo do jogador
            SaveManager.MarcarMoedaComoApanhada(idUnico);   // Marca esta ficha como recolhida no sistema de gravação
            gameObject.SetActive(false);                    // Remove visualmente a ficha do mundo
        }
    }
}