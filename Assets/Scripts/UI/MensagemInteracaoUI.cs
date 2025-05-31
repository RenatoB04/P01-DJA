using UnityEngine;
using TMPro;
using System.Collections;

public class MensagemInteracaoUI : MonoBehaviour
{
    public static MensagemInteracaoUI instancia;
    public TextMeshProUGUI textoInteracao;

    private Coroutine esconderCoroutine;

    private void Awake()
    {
        if (instancia == null)
            instancia = this;

        if (textoInteracao != null)
            textoInteracao.gameObject.SetActive(false);
    }

    public void Mostrar(string mensagem = "Pressiona E para interagir")
    {
        if (textoInteracao != null)
        {
            if (esconderCoroutine != null)
            {
                StopCoroutine(esconderCoroutine);
                esconderCoroutine = null;
            }

            textoInteracao.text = mensagem;
            textoInteracao.gameObject.SetActive(true);
        }
    }

    public void Esconder()
    {
        if (textoInteracao != null)
        {
            textoInteracao.gameObject.SetActive(false);
        }
    }

    public void MostrarTemporario(string mensagem, float duracao)
    {
        Mostrar(mensagem);
        esconderCoroutine = StartCoroutine(EsconderAposTempo(duracao));
    }

    private IEnumerator EsconderAposTempo(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        Esconder();
        esconderCoroutine = null;
    }
}