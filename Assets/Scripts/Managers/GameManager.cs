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
                    PowerUpManager.instancia.ganhoExtraPercentagem = dados.ganhoExtraPercentagem;
                    PowerUpManager.instancia.ganhoExtraDuracao = dados.ganhoExtraDuracao;
                    PowerUpManager.instancia.ganhoExtraDataCompra = dados.ganhoExtraDataCompra;

                    PowerUpManager.instancia.protecaoPerdaAtiva = dados.protecaoPerdaAtiva;
                    PowerUpManager.instancia.protecaoPerdaPercentagem = dados.protecaoPerdaPercentagem;
                    PowerUpManager.instancia.protecaoPerdaDuracao = dados.protecaoPerdaDuracao;
                    PowerUpManager.instancia.protecaoPerdaDataCompra = dados.protecaoPerdaDataCompra;

                    // Verifica se já expiraram ao iniciar
                    PowerUpManager.instancia.VerificarExpiracaoPowerUps();
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
        if (PowerUpManager.instancia != null)
        {
            PowerUpManager.instancia.VerificarExpiracaoPowerUps();
            valor = PowerUpManager.instancia.AplicarBónusGanho(valor);
        }

        dinheiro += valor;
        Guardar();
    }

    public void RemoverDinheiro(int valor)
    {
        if (PowerUpManager.instancia != null)
        {
            PowerUpManager.instancia.VerificarExpiracaoPowerUps();
            int reducao = PowerUpManager.instancia.AplicarProtecaoPerda(valor);
            valor -= reducao;
        }

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
        if (PowerUpManager.instancia != null)
        {
            SaveManager.GuardarJogo(
                dinheiro,
                PowerUpManager.instancia.ganhoExtraAtivo,
                PowerUpManager.instancia.ganhoExtraPercentagem,
                PowerUpManager.instancia.ganhoExtraDuracao,
                PowerUpManager.instancia.ganhoExtraDataCompra,
                PowerUpManager.instancia.protecaoPerdaAtiva,
                PowerUpManager.instancia.protecaoPerdaPercentagem,
                PowerUpManager.instancia.protecaoPerdaDuracao,
                PowerUpManager.instancia.protecaoPerdaDataCompra
            );
        }
        else
        {
            SaveManager.GuardarJogo(
                dinheiro,
                false, 0, "", "",
                false, 0, "", ""
            );
        }
    }
}