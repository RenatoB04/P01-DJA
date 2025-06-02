using UnityEngine;
using TMPro;
using System;
using System.Globalization;

public class PowerUpsUI : MonoBehaviour
{
    public TMP_Text textoPowerUps; // Referência ao texto que apresenta os power-ups ativos

    void Update()
    {
        string texto = " ";

        // Verifica se o power-up de ganho extra está ativo
        if (PowerUpManager.instancia.ganhoExtraAtivo)
        {
            texto += $"Ganho Extra: +{PowerUpManager.instancia.ganhoExtraPercentagem}%";

            // Se não for permanente, calcula o tempo restante
            if (PowerUpManager.instancia.ganhoExtraDuracao.ToLower() != "permanente" &&
                DateTime.TryParse(PowerUpManager.instancia.ganhoExtraDataCompra, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime dataCompra))
            {
                string tempoRestante = CalcularTempoRestante(dataCompra, PowerUpManager.instancia.ganhoExtraDuracao);
                texto += $" ({tempoRestante})";
            }

            texto += "\n";
        }

        // Verifica se a proteção de perda está ativa
        if (PowerUpManager.instancia.protecaoPerdaAtiva)
        {
            texto += $"Proteção de Perda: {PowerUpManager.instancia.protecaoPerdaPercentagem}%";

            // Se não for permanente, calcula o tempo restante
            if (PowerUpManager.instancia.protecaoPerdaDuracao.ToLower() != "permanente" &&
                DateTime.TryParse(PowerUpManager.instancia.protecaoPerdaDataCompra, null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime dataCompra))
            {
                string tempoRestante = CalcularTempoRestante(dataCompra, PowerUpManager.instancia.protecaoPerdaDuracao);
                texto += $" ({tempoRestante})";
            }

            texto += "\n";
        }

        // Se não houver nenhum ativo, mostra indicação
        if (texto.Trim() == "Power-Ups ativos:")
            texto += "\n(nenhum ativo)";

        textoPowerUps.text = texto;
    }

    // Calcula o tempo restante até o power-up expirar
    private string CalcularTempoRestante(DateTime dataCompra, string duracao)
    {
        TimeSpan tempoTotal;

        // Define a duração total com base na string de entrada
        switch (duracao.ToLower())
        {
            case "10 minutos": tempoTotal = TimeSpan.FromMinutes(10); break;
            case "30 minutos": tempoTotal = TimeSpan.FromMinutes(30); break;
            case "1 hora":     tempoTotal = TimeSpan.FromHours(1); break;
            default:           return "Desconhecido";
        }

        // Calcula quanto tempo passou e quanto falta
        TimeSpan tempoDecorrido = DateTime.UtcNow - dataCompra;
        TimeSpan restante = tempoTotal - tempoDecorrido;

        if (restante.TotalSeconds <= 0)
            return "Expirado";

        return $"{(int)restante.TotalMinutes}m {restante.Seconds}s";
    }
}