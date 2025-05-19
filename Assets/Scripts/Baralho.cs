using System.Collections.Generic;
using UnityEngine;

public class Baralho : MonoBehaviour
{
    [Header("Sprites das Cartas")]
    public List<Sprite> spritesCartas;

    public List<Carta> todasAsCartas = new List<Carta>();
    private List<Carta> baralhoAtual = new List<Carta>();

    void Awake()
    {
        CriarCartas();
    }

    public void CriarCartas()
    {
        todasAsCartas.Clear();

        foreach (var sprite in spritesCartas)
        {
            string nome = sprite.name;
            string[] partes = nome.Split('-');

            if (partes.Length < 3)
            {
                Debug.LogWarning("Nome inválido na carta: " + nome);
                continue;
            }
            
            string numeroTexto = partes[2].Split('_')[0];

            if (!int.TryParse(numeroTexto, out int numero))
            {
                Debug.LogWarning("Número inválido na carta: " + nome);
                continue;
            }

            int valor;
            if (numero >= 10) valor = 10;
            else if (numero == 1) valor = 11;
            else valor = numero;

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
            CriarCartas();
        }

        baralhoAtual = new List<Carta>(todasAsCartas);
        Embaralhar();
    }

    public void Embaralhar()
    {
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
            Inicializar();
        }

        Carta carta = baralhoAtual[0];
        baralhoAtual.RemoveAt(0);
        return carta;
    }
}
