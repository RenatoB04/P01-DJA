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

            var dados = SaveManager.CarregarJogo();
            if (dados != null)
            {
                dinheiro = dados.dinheiro;
                if (PowerUpManager.instancia != null)
                {
                    PowerUpManager.instancia.ganhoExtraAtivo = dados.ganhoExtraAtivo;
                    PowerUpManager.instancia.protecaoPerdaAtiva = dados.protecaoPerdaAtiva;
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AdicionarDinheiro(int valor)
    {
        dinheiro += valor;
        Guardar();
    }

    public void RemoverDinheiro(int valor)
    {
        dinheiro -= valor;
        if (dinheiro < 0)
            dinheiro = 0;
        Guardar();
    }

    public int ObterDinheiro()
    {
        return dinheiro;
    }
    
    public void DefinirDinheiro(int novoValor)
    {
        dinheiro = Mathf.Max(0, novoValor);
        Guardar();
    }

    private void Guardar()
    {
        SaveManager.GuardarJogo(
            dinheiro,
            PowerUpManager.instancia != null && PowerUpManager.instancia.ganhoExtraAtivo,
            PowerUpManager.instancia != null && PowerUpManager.instancia.protecaoPerdaAtiva
        );
    }
}