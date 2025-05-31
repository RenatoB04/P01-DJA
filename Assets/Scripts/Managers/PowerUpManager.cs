using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instancia;

    public bool ganhoExtraAtivo = false;
    public bool protecaoPerdaAtiva = false;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int AplicarBónusGanho(int ganho)
    {
        if (ganhoExtraAtivo)
            ganho += Mathf.RoundToInt(ganho * 0.10f);
        return ganho;
    }

    public int AplicarProtecaoPerda(int perda)
    {
        if (protecaoPerdaAtiva)
            return Mathf.RoundToInt(perda * 0.10f);
        return 0;
    }
}