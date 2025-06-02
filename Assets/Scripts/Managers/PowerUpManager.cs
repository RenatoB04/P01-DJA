using UnityEngine;
using System;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instancia; // Singleton para acesso global

    [Header("Ganho Extra")]
    public bool ganhoExtraAtivo = false;             // Se o power-up está ativo
    public int ganhoExtraPercentagem = 0;            // Percentagem de bónus ao ganhar moedas
    public string ganhoExtraDuracao = "";            // Duração do power-up (ex: "10 minutos")
    public string ganhoExtraDataCompra = "";         // Data de ativação no formato ISO

    [Header("Proteção de Perda")]
    public bool protecaoPerdaAtiva = false;          // Se a proteção contra perda está ativa
    public int protecaoPerdaPercentagem = 0;         // Percentagem protegida ao perder moedas
    public string protecaoPerdaDuracao = "";         // Duração do power-up
    public string protecaoPerdaDataCompra = "";      // Data de ativação

    private void Awake()
    {
        // Garante que só existe uma instância (padrão Singleton)
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject); // Mantém entre cenas

        VerificarExpiracaoPowerUps(); // Verifica se os power-ups já expiraram ao iniciar
    }

    public void VerificarExpiracaoPowerUps()
    {
        // Verifica se o ganho extra expirou (caso não seja permanente)
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

        // Verifica se a proteção de perda expirou
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

    // Verifica se já passou tempo suficiente para o power-up expirar
    private bool DuracaoExpirada(string dataCompraISO, string duracao)
    {
        if (!DateTime.TryParse(dataCompraISO, out DateTime dataCompra))
            return true;

        TimeSpan tempoPassado = DateTime.UtcNow - dataCompra;

        // Compara com a duração indicada
        switch (duracao.ToLower())
        {
            case "10 minutos": return tempoPassado.TotalMinutes > 10;
            case "30 minutos": return tempoPassado.TotalMinutes > 30;
            case "1 hora":     return tempoPassado.TotalHours > 1;
            default:           return false; // "permanente" ou inválido
        }
    }

    // Aplica o bónus de ganho se o power-up estiver ativo
    public int AplicarBónusGanho(int ganho)
    {
        if (ganhoExtraAtivo && ganhoExtraPercentagem > 0)
        {
            float bonus = ganho * (ganhoExtraPercentagem / 100f);
            return ganho + Mathf.RoundToInt(bonus);
        }
        return ganho;
    }

    // Aplica a proteção de perda (reduzindo o valor perdido)
    public int AplicarProtecaoPerda(int perda)
    {
        if (protecaoPerdaAtiva && protecaoPerdaPercentagem > 0)
        {
            return Mathf.RoundToInt(perda * (protecaoPerdaPercentagem / 100f));
        }
        return 0;
    }
}