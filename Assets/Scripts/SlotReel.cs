using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotReel : MonoBehaviour
{
    public Image[] simbolos;
    public Sprite[] spritesDisponiveis;
    public float tempoRolar = 1f;

    public IEnumerator Rolar()
    {
        float t = 0f;
        while (t < tempoRolar)
        {
            for (int i = 0; i < simbolos.Length; i++)
            {
                int aleatorio = Random.Range(0, spritesDisponiveis.Length);
                simbolos[i].sprite = spritesDisponiveis[aleatorio];
            }

            yield return new WaitForSeconds(0.1f);
            t += 0.1f;
        }
    }

    public Sprite GetSimboloDoMeio()
    {
        return simbolos[1].sprite;
    }
}