using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int dinheiro;

    // Estado do power-up "Ganho Extra"
    public bool ganhoExtraAtivo;
    public int ganhoExtraPercentagem;
    public string ganhoExtraDuracao;
    public string ganhoExtraDataCompra;

    // Estado do power-up "Proteção de Perda"
    public bool protecaoPerdaAtiva;
    public int protecaoPerdaPercentagem;
    public string protecaoPerdaDuracao;
    public string protecaoPerdaDataCompra;

    public List<string> moedasApanhadas = new List<string>(); // IDs das moedas já recolhidas
}

public static class SaveManager
{
    private static string caminho => Application.persistentDataPath + "/save.json"; // Caminho onde o ficheiro de gravação será guardado
    private static SaveData dados;

    public static void GuardarJogo(
        int dinheiro,
        bool ganhoExtraAtivo,
        int ganhoExtraPercentagem,
        string ganhoExtraDuracao,
        string ganhoExtraDataCompra,
        bool protecaoPerdaAtiva,
        int protecaoPerdaPercentagem,
        string protecaoPerdaDuracao,
        string protecaoPerdaDataCompra)
    {
        // Cria um novo objeto com os dados atuais, preservando moedas já apanhadas (se existirem)
        dados = new SaveData
        {
            dinheiro = dinheiro,

            ganhoExtraAtivo = ganhoExtraAtivo,
            ganhoExtraPercentagem = ganhoExtraPercentagem,
            ganhoExtraDuracao = ganhoExtraDuracao,
            ganhoExtraDataCompra = ganhoExtraDataCompra,

            protecaoPerdaAtiva = protecaoPerdaAtiva,
            protecaoPerdaPercentagem = protecaoPerdaPercentagem,
            protecaoPerdaDuracao = protecaoPerdaDuracao,
            protecaoPerdaDataCompra = protecaoPerdaDataCompra,

            moedasApanhadas = dados?.moedasApanhadas ?? new List<string>()
        };

        GuardarDados();
    }

    public static SaveData CarregarJogo()
    {
        // Carrega os dados do ficheiro JSON, caso exista
        if (File.Exists(caminho))
        {
            string json = File.ReadAllText(caminho);
            dados = JsonUtility.FromJson<SaveData>(json);
            return dados;
        }

        // Se não existir, devolve dados vazios (primeira vez a jogar)
        dados = new SaveData();
        return dados;
    }

    public static void MarcarMoedaComoApanhada(string id)
    {
        if (dados == null)
            dados = CarregarJogo();

        // Só adiciona se ainda não tiver sido registada
        if (!dados.moedasApanhadas.Contains(id))
        {
            dados.moedasApanhadas.Add(id);
            GuardarDados();
        }
    }

    public static bool MoedaFoiApanhada(string id)
    {
        if (dados == null)
            dados = CarregarJogo();

        return dados.moedasApanhadas.Contains(id);
    }

    // Serializa e grava os dados no disco em formato JSON
    private static void GuardarDados()
    {
        string json = JsonUtility.ToJson(dados, true);
        File.WriteAllText(caminho, json);
        Debug.Log("Jogo guardado.");
    }

    [ContextMenu("Apagar Jogo (Save)")]
    public static void ApagarJogoDoInspector()
    {
        // Permite apagar o ficheiro de gravação a partir do Inspector (útil para testes)
        if (File.Exists(caminho))
        {
            File.Delete(caminho);
            Debug.Log("Save apagado com sucesso.");
        }
        else
        {
            Debug.Log("Nenhum save encontrado para apagar.");
        }
    }
}