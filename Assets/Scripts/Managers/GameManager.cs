using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    private int dinheiro = 1000;

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

    public void AdicionarDinheiro(int valor)
    {
        dinheiro += valor;
    }

    public void RemoverDinheiro(int valor)
    {
        dinheiro -= valor;
        if (dinheiro < 0)
            dinheiro = 0;
    }

    public int ObterDinheiro()
    {
        return dinheiro;
    }
}