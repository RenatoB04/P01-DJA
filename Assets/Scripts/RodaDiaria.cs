using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RodaDiaria : MonoBehaviour
{
    public TextMeshProUGUI textoResultado;
    public Button botaoRodar;

    public int[] premios = { 0, 25, 50, 100, 200 };
    private bool jaRodou = false;

    void Start()
    {
        botaoRodar.onClick.AddListener(Rodar);
    }

    void Rodar()
    {
        if (jaRodou)
        {
            textoResultado.text = "⏳ Já rodaste hoje!";
            return;
        }

        int premio = premios[Random.Range(0, premios.Length)];

        if (premio > 0)
        {
            textoResultado.text = "🎉 Ganhaste " + premio + " moedas!";
            GameManager.instancia.AdicionarDinheiro(premio);
        }
        else
        {
            textoResultado.text = "😅 Não ganhaste nada desta vez!";
        }

        jaRodou = true;
    }
}
