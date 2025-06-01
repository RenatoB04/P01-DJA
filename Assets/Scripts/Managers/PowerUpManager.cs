using UnityEngine;
using System;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instancia;

    [Header("Ganho Extra")]
    public bool ganhoExtraAtivo = false;
    public int ganhoExtraPercentagem = 0;
    public string ganhoExtraDuracao = "";
    public string ganhoExtraDataCompra = "";

    [Header("Proteção de Perda")]
    public bool protecaoPerdaAtiva = false;
    public int protecaoPerdaPercentagem = 0;
    public string protecaoPerdaDuracao = "";
    public string protecaoPerdaDataCompra = "";

    private void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);

        VerificarExpiracaoPowerUps();
    }

    public void VerificarExpiracaoPowerUps()
    {
        if (ganhoExtraAtivo && ganhoExtraDuracao.ToLower() != "permanente" && !string.IsNullOrEmpty(ganhoExtraDataCompra))
        {
            if (DuracaoExpirada(ganhoExtraDataCompra, ganhoExtraDuracao))
            {
                ganhoExtraAtivo = false;
                ganhoExtraPercentagem = 0;
                ganhoExtraDuracao = "";
                ganhoExtraDataCompra = "";
                Debug.Log("Power-up 'Ganho Extra' expirou.");
            }
        }

        if (protecaoPerdaAtiva && protecaoPerdaDuracao.ToLower() != "permanente" && !string.IsNullOrEmpty(protecaoPerdaDataCompra))
        {
            if (DuracaoExpirada(protecaoPerdaDataCompra, protecaoPerdaDuracao))
            {
                protecaoPerdaAtiva = false;
                protecaoPerdaPercentagem = 0;
                protecaoPerdaDuracao = "";
                protecaoPerdaDataCompra = "";
                Debug.Log("Power-up 'Proteção de Perda' expirou.");
            }
        }
    }

    private bool DuracaoExpirada(string dataCompraISO, string duracao)
    {
        if (!DateTime.TryParse(dataCompraISO, out DateTime dataCompra))
            return true;

        TimeSpan tempoPassado = DateTime.UtcNow - dataCompra;
        switch (duracao.ToLower())
        {
            case "10 minutos": return tempoPassado.TotalMinutes > 10;
            case "30 minutos": return tempoPassado.TotalMinutes > 30;
            case "1 hora":     return tempoPassado.TotalHours > 1;
            default:           return false;
        }
    }

    public int AplicarBónusGanho(int ganho)
    {
        if (ganhoExtraAtivo && ganhoExtraPercentagem > 0)
        {
            float bonus = ganho * (ganhoExtraPercentagem / 100f);
            return ganho + Mathf.RoundToInt(bonus);
        }
        return ganho;
    }

    public int AplicarProtecaoPerda(int perda)
    {
        if (protecaoPerdaAtiva && protecaoPerdaPercentagem > 0)
        {
            return Mathf.RoundToInt(perda * (protecaoPerdaPercentagem / 100f));
        }
        return 0;
    }
}