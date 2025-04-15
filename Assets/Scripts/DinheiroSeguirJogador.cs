using UnityEngine;
using TMPro;

public class DinheiroSeguirJogador : MonoBehaviour
{
    public TextMeshProUGUI textoDinheiro;

    void Update()
    {
        textoDinheiro.text = "Moedas: " + GameManager.instancia.ObterDinheiro().ToString();
    }
}
