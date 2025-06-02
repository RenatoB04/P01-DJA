using UnityEngine;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // Dropdown da UI para escolher a resolução

    private Resolution[] resolutions; // Lista de resoluções disponíveis no sistema

    void Start()
    {
        resolutions = Screen.resolutions; // Obtém todas as resoluções suportadas

        resolutionDropdown.ClearOptions(); // Limpa opções anteriores no dropdown

        var options = new System.Collections.Generic.List<string>();

        // Preenche a lista com resoluções no formato "largura x altura"
        foreach (var res in resolutions)
        {
            options.Add(res.width + "x" + res.height);
        }

        resolutionDropdown.AddOptions(options); // Adiciona ao dropdown

        resolutionDropdown.value = GetCurrentResolutionIndex(); // Define a opção atual como a resolução ativa

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution); // Liga o evento à função de alteração
    }

    // Determina o índice da resolução atualmente em uso
    private int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0; // Caso não encontre, retorna a primeira resolução
    }

    // Altera a resolução do ecrã com base na seleção do dropdown
    public void ChangeResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}