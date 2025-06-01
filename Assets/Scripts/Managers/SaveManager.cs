using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int dinheiro;

    public bool ganhoExtraAtivo;
    public int ganhoExtraPercentagem;
    public string ganhoExtraDuracao;
    public string ganhoExtraDataCompra;

    public bool protecaoPerdaAtiva;
    public int protecaoPerdaPercentagem;
    public string protecaoPerdaDuracao;
    public string protecaoPerdaDataCompra;
}

public static class SaveManager
{
    private static string caminho => Application.persistentDataPath + "/save.json";

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
        SaveData dados = new SaveData
        {
            dinheiro = dinheiro,

            ganhoExtraAtivo = ganhoExtraAtivo,
            ganhoExtraPercentagem = ganhoExtraPercentagem,
            ganhoExtraDuracao = ganhoExtraDuracao,
            ganhoExtraDataCompra = ganhoExtraDataCompra,

            protecaoPerdaAtiva = protecaoPerdaAtiva,
            protecaoPerdaPercentagem = protecaoPerdaPercentagem,
            protecaoPerdaDuracao = protecaoPerdaDuracao,
            protecaoPerdaDataCompra = protecaoPerdaDataCompra
        };

        string json = JsonUtility.ToJson(dados, true);
        File.WriteAllText(caminho, json);
        Debug.Log("Jogo guardado.");
    }

    public static SaveData CarregarJogo()
    {
        if (File.Exists(caminho))
        {
            string json = File.ReadAllText(caminho);
            return JsonUtility.FromJson<SaveData>(json);
        }

        return null;
    }

    [ContextMenu("Apagar Jogo (Save)")]
    public static void ApagarJogoDoInspector()
    {
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