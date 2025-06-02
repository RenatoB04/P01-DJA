using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotReel : MonoBehaviour
{
    public Image[] simbolos;              // Referência às imagens que representam os símbolos da coluna
    public Sprite[] spritesDisponiveis;   // Lista de sprites possíveis para os símbolos
    public float tempoRolar = 1f;         // Duração da rotação desta coluna

    public IEnumerator Rolar()
    {
        float t = 0f;

        // Enquanto a rotação não tiver terminado
        while (t < tempoRolar)
        {
            // Para cada símbolo da coluna, atribui aleatoriamente um sprite
            for (int i = 0; i < simbolos.Length; i++)
            {
                int aleatorio = Random.Range(0, spritesDisponiveis.Length);
                simbolos[i].sprite = spritesDisponiveis[aleatorio];
            }

            yield return new WaitForSeconds(0.1f); // Pequena pausa entre cada "tick" visual
            t += 0.1f;
        }
    }

    // Devolve o sprite que está no centro da coluna (útil se quiseres lógica mais avançada)
    public Sprite GetSimboloDoMeio()
    {
        return simbolos[1].sprite;
    }
}