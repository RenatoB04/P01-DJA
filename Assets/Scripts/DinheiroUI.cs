using UnityEngine;
using TMPro;

public class DinheiroUI : MonoBehaviour
{
    public TextMeshProUGUI textoDinheiro;

    void Update()
    {
        textoDinheiro.text = "Moedas: " + GameManager.instancia.ObterDinheiro().ToString();
    }
}
