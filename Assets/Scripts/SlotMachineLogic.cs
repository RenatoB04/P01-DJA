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
        if (GameManager.instancia.ObterDinheiro() < 50)
        {
            textoResultado.text = "Saldo insuficiente!";
            return;
        }

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
            GameManager.instancia.AdicionarDinheiro(250);
        }
        else
        {
            textoResultado.text = "Perdeste!";
            GameManager.instancia.RemoverDinheiro(50);
        }
    }
}