using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider volumeSlider; // Referência ao slider na UI que controla o volume do jogo

    void Start()
    {
        // Define o valor inicial do slider com base no volume atual do sistema de áudio
        volumeSlider.value = AudioListener.volume;

        // Adiciona o método ChangeVolume como listener para quando o valor do slider mudar
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    public void ChangeVolume(float volume)
    {
        // Altera o volume global do jogo
        AudioListener.volume = volume;
    }
}