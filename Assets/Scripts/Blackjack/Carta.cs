using UnityEngine;

[System.Serializable] // Permite que esta classe seja visível e editável no Inspector da Unity
public class Carta
{
    public string nome;      // Nome da carta, normalmente extraído do nome do ficheiro da sprite
    public int valor;        // Valor numérico usado para cálculos no jogo (ex: 2 a 11)
    public Sprite imagem;    // Imagem da carta, usada para mostrar visualmente no UI
}