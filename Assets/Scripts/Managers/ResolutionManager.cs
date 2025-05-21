using UnityEngine;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();

        foreach (var res in resolutions)
        {
            options.Add(res.width + "x" + res.height);
        }

        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = GetCurrentResolutionIndex();

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
    }

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
        return 0;
    }

    public void ChangeResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}