using TMPro;
using UnityEngine;

public class DinheiroUI : MonoBehaviour
{
    public TMP_Text textoMoedas;

    void Update()
    {
        textoMoedas.text = "Moedas: " + GameManager.instancia.ObterDinheiro().ToString();
    }
}