using UnityEngine;
using TMPro;

public class MensagemInteracaoUI : MonoBehaviour
{
    public static MensagemInteracaoUI instancia;
    public TextMeshProUGUI textoInteracao;

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
            textoInteracao.text = mensagem;
            textoInteracao.gameObject.SetActive(true);
        }
    }

    public void Esconder()
    {
        if (textoInteracao != null)
            textoInteracao.gameObject.SetActive(false);
    }
}