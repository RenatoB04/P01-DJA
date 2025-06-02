using TMPro;
using UnityEngine;

public class DinheiroUI : MonoBehaviour
{
    public TMP_Text textoMoedas; // Referência ao texto que mostra a quantidade de moedas

    void Update()
    {
        // Atualiza o texto a cada frame com o valor atual de moedas do jogador
        textoMoedas.text = "Moedas: " + GameManager.instancia.ObterDinheiro().ToString();
    }
}