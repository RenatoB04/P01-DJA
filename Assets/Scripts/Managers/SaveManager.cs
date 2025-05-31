using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int dinheiro;
    public bool ganhoExtraAtivo;
    public bool protecaoPerdaAtiva;
}

public class SaveManager : MonoBehaviour
{
    private static string caminho => Application.persistentDataPath + "/save.json";

    public static void GuardarJogo(int dinheiro, bool ganhoExtra, bool protecaoPerda)
    {
        SaveData dados = new SaveData
        {
            dinheiro = dinheiro,
            ganhoExtraAtivo = ganhoExtra,
            protecaoPerdaAtiva = protecaoPerda
        };

        string json = JsonUtility.ToJson(dados);
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
    public void ApagarJogoDoInspector()
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