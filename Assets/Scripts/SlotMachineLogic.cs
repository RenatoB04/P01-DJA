using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotMachineLogic : MonoBehaviour
{
    public Image[] slots;
    public Sprite[] simbolos;
    public TextMeshProUGUI textoResultado;

    public void Girar()
    {
        int[] resultados = new int[slots.Length];
        
        for (int i = 0; i < slots.Length; i++)
        {
            int indexAleatorio = Random.Range(0, simbolos.Length);
            resultados[i] = indexAleatorio;
            slots[i].sprite = simbolos[indexAleatorio];
        }

        if (resultados[0] == resultados[1] && resultados[1] == resultados[2])
        {
            textoResultado.text = "Ganhaste!";
        }
        else
        {
            textoResultado.text = "Perdeste!";
        }
    }
}
