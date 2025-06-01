using UnityEngine;
using TMPro;
using System;
using System.Globalization;

public class PowerUpsUI : MonoBehaviour
{
    public TMP_Text textoPowerUps;

    void Update()
    {
        string texto = " ";

        if (PowerUpManager.instancia.ganhoExtraAtivo)
        {
            texto += $"Ganho Extra: +{PowerUpManager.instancia.ganhoExtraPercentagem}%";

            if (PowerUpManager.instancia.ganhoExtraDuracao.ToLower() != "permanente" &&
                DateTime.TryParse(PowerUpManager.instancia.ganhoExtraDataCompra, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime dataCompra))
            {
                string tempoRestante = CalcularTempoRestante(dataCompra, PowerUpManager.instancia.ganhoExtraDuracao);
                texto += $" ({tempoRestante})";
            }

            texto += "\n";
        }

        if (PowerUpManager.instancia.protecaoPerdaAtiva)
        {
            texto += $"Proteção de Perda: {PowerUpManager.instancia.protecaoPerdaPercentagem}%";

            if (PowerUpManager.instancia.protecaoPerdaDuracao.ToLower() != "permanente" &&
                DateTime.TryParse(PowerUpManager.instancia.protecaoPerdaDataCompra, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime dataCompra))
            {
                string tempoRestante = CalcularTempoRestante(dataCompra, PowerUpManager.instancia.protecaoPerdaDuracao);
                texto += $" ({tempoRestante})";
            }

            texto += "\n";
        }

        if (texto.Trim() == "Power-Ups ativos:")
            texto += "\n(nenhum ativo)";

        textoPowerUps.text = texto;
    }

    private string CalcularTempoRestante(DateTime dataCompra, string duracao)
    {
        TimeSpan tempoTotal;

        switch (duracao.ToLower())
        {
            case "10 minutos": tempoTotal = TimeSpan.FromMinutes(10); break;
            case "30 minutos": tempoTotal = TimeSpan.FromMinutes(30); break;
            case "1 hora": tempoTotal = TimeSpan.FromHours(1); break;
            default: return "Desconhecido";
        }

        TimeSpan tempoDecorrido = DateTime.UtcNow - dataCompra;
        TimeSpan restante = tempoTotal - tempoDecorrido;

        if (restante.TotalSeconds <= 0)
            return "Expirado";

        return $"{(int)restante.TotalMinutes}m {restante.Seconds}s";
    }
}