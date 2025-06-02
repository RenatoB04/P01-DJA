using UnityEngine;
using TMPro;
using System.Collections;

public class MensagemInteracaoUI : MonoBehaviour
{
    public static MensagemInteracaoUI instancia; // Singleton para acesso global
    public TextMeshProUGUI textoInteracao;       // Referência ao texto mostrado no ecrã

    private Coroutine esconderCoroutine;         // Referência à coroutine ativa (se existir)

    private void Awake()
    {
        // Define a instância global
        if (instancia == null)
            instancia = this;

        // Garante que a mensagem está escondida no início
        if (textoInteracao != null)
            textoInteracao.gameObject.SetActive(false);
    }

    // Mostra uma mensagem de interação (ou uma mensagem padrão)
    public void Mostrar(string mensagem = "Pressiona E para interagir")
    {
        if (textoInteracao != null)
        {
            // Interrompe qualquer contagem de tempo anterior
            if (esconderCoroutine != null)
            {
                StopCoroutine(esconderCoroutine);
                esconderCoroutine = null;
            }

            textoInteracao.text = mensagem;
            textoInteracao.gameObject.SetActive(true);
        }
    }

    // Esconde a mensagem de interação imediatamente
    public void Esconder()
    {
        if (textoInteracao != null)
        {
            textoInteracao.gameObject.SetActive(false);
        }
    }

    // Mostra uma mensagem por tempo limitado
    public void MostrarTemporario(string mensagem, float duracao)
    {
        Mostrar(mensagem); // Mostra de imediato
        esconderCoroutine = StartCoroutine(EsconderAposTempo(duracao)); // Inicia contagem para esconder
    }

    // Coroutine que espera e depois esconde a mensagem
    private IEnumerator EsconderAposTempo(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        Esconder();
        esconderCoroutine = null;
    }
}