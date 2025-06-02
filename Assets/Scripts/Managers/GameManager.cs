using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia; // Singleton para acesso global ao GameManager
    private int dinheiro = 1000; // Valor inicial de moedas

    private void Awake()
    {
        // Garante que só existe uma instância do GameManager durante toda a execução do jogo
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // Impede que este objecto seja destruído ao mudar de cena

            // Tenta carregar dados guardados (dinheiro e estados dos power-ups)
            var dados = SaveManager.CarregarJogo();
            if (dados != null)
            {
                dinheiro = dados.dinheiro;

                if (PowerUpManager.instancia != null)
                {
                    // Restaura os estados dos power-ups guardados
                    PowerUpManager.instancia.ganhoExtraAtivo = dados.ganhoExtraAtivo;
                    PowerUpManager.instancia.ganhoExtraPercentagem = dados.ganhoExtraPercentagem;
                    PowerUpManager.instancia.ganhoExtraDuracao = dados.ganhoExtraDuracao;
                    PowerUpManager.instancia.ganhoExtraDataCompra = dados.ganhoExtraDataCompra;

                    PowerUpManager.instancia.protecaoPerdaAtiva = dados.protecaoPerdaAtiva;
                    PowerUpManager.instancia.protecaoPerdaPercentagem = dados.protecaoPerdaPercentagem;
                    PowerUpManager.instancia.protecaoPerdaDuracao = dados.protecaoPerdaDuracao;
                    PowerUpManager.instancia.protecaoPerdaDataCompra = dados.protecaoPerdaDataCompra;

                    // Garante que não estão ativos se já expiraram
                    PowerUpManager.instancia.VerificarExpiracaoPowerUps();
                }
            }
        }
        else
        {
            Destroy(gameObject); // Elimina duplicados se já existir uma instância
        }
    }

    public void AdicionarDinheiro(int valor)
    {
        // Aplica o bónus de ganho caso o power-up esteja ativo
        if (PowerUpManager.instancia != null)
        {
            PowerUpManager.instancia.VerificarExpiracaoPowerUps();
            valor = PowerUpManager.instancia.AplicarBónusGanho(valor);
        }

        dinheiro += valor;
        Guardar(); // Guarda imediatamente o novo estado
    }

    public void RemoverDinheiro(int valor)
    {
        // Aplica proteção contra perda se o power-up estiver ativo
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
        dinheiro = Mathf.Max(0, novoValor); // Garante que nunca fica negativo
        Guardar();
    }

    // Guarda o estado atual do jogo (moedas + estados dos power-ups)
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
            // Guarda apenas o dinheiro se não houver PowerUpManager disponível
            SaveManager.GuardarJogo(
                dinheiro,
                false, 0, "", "",
                false, 0, "", ""
            );
        }
    }
}