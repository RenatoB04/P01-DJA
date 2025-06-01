using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class LojaPowerUps : MonoBehaviour
{
    public TMP_Text textoSaldo;
    public TMP_Dropdown dropdownPercentagemGanho;
    public TMP_Dropdown dropdownDuracaoGanho;
    public TMP_Dropdown dropdownPercentagemProtecao;
    public TMP_Dropdown dropdownDuracaoProtecao;

    public Button botaoGanhoExtra;
    public Button botaoProtecaoPerda;
    public Button botaoVoltar;

    private void Start()
    {
        AtualizarUI();

        botaoGanhoExtra.onClick.AddListener(ComprarGanhoExtra);
        botaoProtecaoPerda.onClick.AddListener(ComprarProtecaoPerda);
        botaoVoltar.onClick.AddListener(() => SceneManager.LoadScene("Jogo"));

        dropdownPercentagemGanho.onValueChanged.AddListener(_ => AtualizarTextoBotao());
        dropdownDuracaoGanho.onValueChanged.AddListener(_ => AtualizarTextoBotao());
        dropdownPercentagemProtecao.onValueChanged.AddListener(_ => AtualizarTextoBotao());
        dropdownDuracaoProtecao.onValueChanged.AddListener(_ => AtualizarTextoBotao());
    }

    void AtualizarUI()
    {
        PowerUpManager.instancia.VerificarExpiracaoPowerUps();

        textoSaldo.text = "Saldo: " + GameManager.instancia.ObterDinheiro() + " moedas";

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

    void AtualizarTextoBotao()
    {
        int precoGanho = CalcularPreco(dropdownPercentagemGanho.value, dropdownDuracaoGanho.value);
        botaoGanhoExtra.GetComponentInChildren<TMP_Text>().text = $"{precoGanho} moedas)";

        int precoProtecao = CalcularPreco(dropdownPercentagemProtecao.value, dropdownDuracaoProtecao.value);
        botaoProtecaoPerda.GetComponentInChildren<TMP_Text>().text = $"{precoProtecao} moedas)";
    }

    int CalcularPreco(int percentIndex, int duracaoIndex)
    {
        int[] percentagens = { 10, 20, 30, 40, 50 };
        int[] multiplicadoresDuracao = { 2, 4, 6, 50 };

        int basePreco = percentagens[percentIndex] * multiplicadoresDuracao[duracaoIndex];
        return basePreco * 50;
    }

    void ComprarGanhoExtra()
    {
        int preco = CalcularPreco(dropdownPercentagemGanho.value, dropdownDuracaoGanho.value);
        if (GameManager.instancia.ObterDinheiro() >= preco)
        {
            GameManager.instancia.RemoverDinheiro(preco);

            PowerUpManager.instancia.ganhoExtraAtivo = true;
            PowerUpManager.instancia.ganhoExtraPercentagem = int.Parse(dropdownPercentagemGanho.options[dropdownPercentagemGanho.value].text.Replace("%", ""));
            PowerUpManager.instancia.ganhoExtraDuracao = dropdownDuracaoGanho.options[dropdownDuracaoGanho.value].text;

            if (!PowerUpManager.instancia.ganhoExtraDuracao.ToLower().Contains("permanente"))
            {
                PowerUpManager.instancia.ganhoExtraDataCompra = DateTime.UtcNow.ToString("o");
            }
            else
            {
                PowerUpManager.instancia.ganhoExtraDataCompra = "";
            }

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
            PowerUpManager.instancia.protecaoPerdaPercentagem = int.Parse(dropdownPercentagemProtecao.options[dropdownPercentagemProtecao.value].text.Replace("%", ""));
            PowerUpManager.instancia.protecaoPerdaDuracao = dropdownDuracaoProtecao.options[dropdownDuracaoProtecao.value].text;

            if (!PowerUpManager.instancia.protecaoPerdaDuracao.ToLower().Contains("permanente"))
            {
                PowerUpManager.instancia.protecaoPerdaDataCompra = DateTime.UtcNow.ToString("o");
            }
            else
            {
                PowerUpManager.instancia.protecaoPerdaDataCompra = "";
            }

            GuardarEstado();
            AtualizarUI();
        }
    }

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