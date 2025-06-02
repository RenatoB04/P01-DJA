using System.Collections.Generic;
using UnityEngine;

public class Baralho : MonoBehaviour
{
    [Header("Sprites das Cartas")]
    public List<Sprite> spritesCartas; // Lista de sprites que representam visualmente cada carta

    public List<Carta> todasAsCartas = new List<Carta>(); // Contém todas as cartas possíveis, criadas a partir dos sprites
    private List<Carta> baralhoAtual = new List<Carta>(); // Representa o estado atual do baralho (pode ser embaralhado ou alterado durante o jogo)

    void Awake()
    {
        CriarCartas(); // Garante que todas as cartas são criadas assim que o objecto é instanciado
    }

    public void CriarCartas()
    {
        todasAsCartas.Clear(); // Limpa a lista para evitar duplicações caso esta função seja chamada mais do que uma vez

        foreach (var sprite in spritesCartas)
        {
            string nome = sprite.name;
            string[] partes = nome.Split('-'); // Espera que o nome da sprite siga um padrão com separadores '-'

            if (partes.Length < 3)
            {
                Debug.LogWarning("Nome inválido na carta: " + nome);
                continue; // Ignora sprites com nomes que não seguem o formato esperado
            }

            string numeroTexto = partes[2].Split('_')[0]; // Extrai o número da carta a partir da string

            if (!int.TryParse(numeroTexto, out int numero))
            {
                Debug.LogWarning("Número inválido na carta: " + nome);
                continue; // Ignora cartas cujo número não é válido
            }

            // Determina o valor da carta com base nas regras típicas de jogos de cartas (e.g., blackjack)
            int valor;
            if (numero >= 10) valor = 10;
            else if (numero == 1) valor = 11;
            else valor = numero;

            // Cria uma nova carta com os dados extraídos
            todasAsCartas.Add(new Carta
            {
                nome = nome,
                valor = valor,
                imagem = sprite
            });
        }

        Debug.Log("Cartas criadas: " + todasAsCartas.Count);
    }

    public void Inicializar()
    {
        if (todasAsCartas.Count == 0)
        {
            Debug.LogWarning("Nenhuma carta disponível para inicializar. Verifica os sprites.");
            CriarCartas(); // Garante que o baralho é criado mesmo que Inicializar seja chamado prematuramente
        }

        baralhoAtual = new List<Carta>(todasAsCartas); // Cria uma cópia do baralho completo para ser usado no jogo
        Embaralhar(); // Mistura a ordem das cartas
    }

    public void Embaralhar()
    {
        // Implementação do algoritmo de Fisher-Yates para embaralhar a lista
        for (int i = 0; i < baralhoAtual.Count; i++)
        {
            var temp = baralhoAtual[i];
            int j = Random.Range(i, baralhoAtual.Count);
            baralhoAtual[i] = baralhoAtual[j];
            baralhoAtual[j] = temp;
        }
    }

    public Carta TirarCarta()
    {
        if (baralhoAtual.Count == 0)
        {
            Debug.LogWarning("Baralho vazio! A reiniciar.");
            Inicializar(); // Reinicia o baralho automaticamente caso este esteja vazio
        }

        Carta carta = baralhoAtual[0]; // Retira a primeira carta do baralho
        baralhoAtual.RemoveAt(0); // Remove-a da lista
        return carta; // Devolve a carta retirada
    }
}