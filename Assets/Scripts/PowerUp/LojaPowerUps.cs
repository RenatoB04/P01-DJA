using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class LojaPowerUps : MonoBehaviour
{
    public TMP_Text textoSaldo;

    // Dropdowns para configurar os power-ups
    public TMP_Dropdown dropdownPercentagemGanho;
    public TMP_Dropdown dropdownDuracaoGanho;
    public TMP_Dropdown dropdownPercentagemProtecao;
    public TMP_Dropdown dropdownDuracaoProtecao;

    // Botões da loja
    public Button botaoGanhoExtra;
    public Button botaoProtecaoPerda;
    public Button botaoVoltar;

    private void Start()
    {
        AtualizarUI(); // Atualiza os estados iniciais da UI

        // Ligações dos botões aos métodos de compra
        botaoGanhoExtra.onClick.AddListener(ComprarGanhoExtra);
        botaoProtecaoPerda.onClick.AddListener(ComprarProtecaoPerda);
        botaoVoltar.onClick.AddListener(() => SceneManager.LoadScene("Jogo"));

        // Atualiza os textos dos botões sempre que se altera uma opção nos dropdowns
        dropdownPercentagemGanho.onValueChanged.AddListener(_ => AtualizarTextoBotao());
        dropdownDuracaoGanho.onValueChanged.AddListener(_ => AtualizarTextoBotao());
        dropdownPercentagemProtecao.onValueChanged.AddListener(_ => AtualizarTextoBotao());
        dropdownDuracaoProtecao.onValueChanged.AddListener(_ => AtualizarTextoBotao());
    }

    void AtualizarUI()
    {
        PowerUpManager.instancia.VerificarExpiracaoPowerUps(); // Atualiza estados com base em tempo

        textoSaldo.text = "Saldo: " + GameManager.instancia.ObterDinheiro() + " moedas";

        // Verifica se o power-up de ganho extra já está comprado
        if (PowerUpManager.instancia.ganhoExtraAtivo)
        {
            botaoGanhoExtra.interactable = false;
            botaoGanhoExtra.GetComponentInChildren<TMP_Text>().text = "Já comprado";
        }
        else
        {
            botaoGanhoExtra.interactable = true;
            AtualizarTextoBotao();
        }

        // Verifica se o power-up de proteção de perda já está comprado
        if (PowerUpManager.instancia.protecaoPerdaAtiva)
        {
            botaoProtecaoPerda.interactable = false;
            botaoProtecaoPerda.GetComponentInChildren<TMP_Text>().text = "Já comprado";
        }
        else
        {
            botaoProtecaoPerda.interactable = true;
            AtualizarTextoBotao();
        }
    }

    // Atualiza os textos dos botões com os preços calculados
    void AtualizarTextoBotao()
    {
        int precoGanho = CalcularPreco(dropdownPercentagemGanho.value, dropdownDuracaoGanho.value);
        botaoGanhoExtra.GetComponentInChildren<TMP_Text>().text = $"{precoGanho} moedas";

        int precoProtecao = CalcularPreco(dropdownPercentagemProtecao.value, dropdownDuracaoProtecao.value);
        botaoProtecaoPerda.GetComponentInChildren<TMP_Text>().text = $"{precoProtecao} moedas";
    }

    // Calcula o preço com base na percentagem e duração selecionadas
    int CalcularPreco(int percentIndex, int duracaoIndex)
    {
        int[] percentagens = { 10, 20, 30, 40, 50 };
        int[] multiplicadoresDuracao = { 2, 4, 6, 50 }; // Valores maiores para durações mais longas

        int basePreco = percentagens[percentIndex] * multiplicadoresDuracao[duracaoIndex];
        return basePreco * 50; // Multiplicador final que define o custo em moedas
    }

    void ComprarGanhoExtra()
    {
        int preco = CalcularPreco(dropdownPercentagemGanho.value, dropdownDuracaoGanho.value);

        if (GameManager.instancia.ObterDinheiro() >= preco)
        {
            GameManager.instancia.RemoverDinheiro(preco);

            // Ativa e configura o power-up com os valores escolhidos
            PowerUpManager.instancia.ganhoExtraAtivo = true;
            PowerUpManager.instancia.ganhoExtraPercentagem = int.Parse(
                dropdownPercentagemGanho.options[dropdownPercentagemGanho.value].text.Replace("%", "")
            );
            PowerUpManager.instancia.ganhoExtraDuracao = dropdownDuracaoGanho.options[dropdownDuracaoGanho.value].text;

            if (!PowerUpManager.instancia.ganhoExtraDuracao.ToLower().Contains("permanente"))
                PowerUpManager.instancia.ganhoExtraDataCompra = DateTime.UtcNow.ToString("o");
            else
                PowerUpManager.instancia.ganhoExtraDataCompra = "";

            GuardarEstado();
            AtualizarUI();
        }
    }

    void ComprarProtecaoPerda()
    {
        int preco = CalcularPreco(dropdownPercentagemProtecao.value, dropdownDuracaoProtecao.value);

        if (GameManager.instancia.ObterDinheiro() >= preco)
        {
            GameManager.instancia.RemoverDinheiro(preco);

            PowerUpManager.instancia.protecaoPerdaAtiva = true;
            PowerUpManager.instancia.protecaoPerdaPercentagem = int.Parse(
                dropdownPercentagemProtecao.options[dropdownPercentagemProtecao.value].text.Replace("%", "")
            );
            PowerUpManager.instancia.protecaoPerdaDuracao = dropdownDuracaoProtecao.options[dropdownDuracaoProtecao.value].text;

            if (!PowerUpManager.instancia.protecaoPerdaDuracao.ToLower().Contains("permanente"))
                PowerUpManager.instancia.protecaoPerdaDataCompra = DateTime.UtcNow.ToString("o");
            else
                PowerUpManager.instancia.protecaoPerdaDataCompra = "";

            GuardarEstado();
            AtualizarUI();
        }
    }

    // Grava o estado atual do jogo com os power-ups comprados
    void GuardarEstado()
    {
        SaveManager.GuardarJogo(
            GameManager.instancia.ObterDinheiro(),
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
}