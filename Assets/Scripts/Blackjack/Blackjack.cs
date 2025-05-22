using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Blackjack : MonoBehaviour
{
    [Header("Referências")]
    public Baralho baralho;
    public Transform handJogador;
    public Transform handDealer;
    public GameObject cartaPrefab;

    [Header("Sprites")]
    public Sprite versoCarta;

    [Header("UI")]
    public TMP_InputField inputAposta;
    public TMP_Text txtSaldo;
    public TMP_Text txtMensagem;
    public TMP_Text txtTotalJogador;
    public TMP_Text txtTotalDealer;
    public Button botaoVoltar;

    private int apostaAtual = 0;

    private List<Carta> cartasJogador = new List<Carta>();
    private List<Carta> cartasDealer = new List<Carta>();

    private bool jogoAtivo = false;
    private bool dealerJogando = false;

    void Start()
    {
        baralho.Inicializar();
        AtualizarUI();
        txtMensagem.text = "Insere uma aposta e clica em Jogar.";

        if (botaoVoltar != null)
        {
            botaoVoltar.onClick.AddListener(VoltarAoJogo);
        }
    }

    public void Jogar()
    {
        int saldoAtual = GameManager.instancia.ObterDinheiro();

        if (!int.TryParse(inputAposta.text, out apostaAtual) || apostaAtual <= 0 || apostaAtual > saldoAtual)
        {
            txtMensagem.text = "Aposta inválida.";
            return;
        }

        GameManager.instancia.RemoverDinheiro(apostaAtual);
        jogoAtivo = true;
        dealerJogando = false;

        LimparMao(handJogador);
        LimparMao(handDealer);
        cartasJogador.Clear();
        cartasDealer.Clear();

        DarCarta(cartasJogador, handJogador, true);
        DarCarta(cartasDealer, handDealer, true);
        DarCarta(cartasJogador, handJogador, true);
        DarCarta(cartasDealer, handDealer, false);

        AtualizarUI();
        txtMensagem.text = "A tua vez. Pede ou fica.";
    }

    public void Pedir()
    {
        if (!jogoAtivo || dealerJogando) return;

        DarCarta(cartasJogador, handJogador, true);
        AtualizarUI();

        if (CalcularTotal(cartasJogador) > 21)
        {
            txtMensagem.text = "Estouraste! Perdeste.";
            jogoAtivo = false;
            AtualizarUI();
        }
    }

    public void Ficar()
    {
        if (!jogoAtivo || dealerJogando) return;

        dealerJogando = true;
        StartCoroutine(DealerJoga());
    }

    private IEnumerator DealerJoga()
    {
        yield return new WaitForSeconds(1f);
        var imgVirada = handDealer.GetChild(1).GetComponent<Image>();
        imgVirada.sprite = cartasDealer[1].imagem;
        AtualizarUI();

        while (CalcularTotal(cartasDealer) < 17)
        {
            yield return new WaitForSeconds(1f);
            DarCarta(cartasDealer, handDealer, true);
            AtualizarUI();
        }

        int totalJ = CalcularTotal(cartasJogador);
        int totalD = CalcularTotal(cartasDealer);
        string resultado;
        int ganho = 0;

        bool blackjack = (cartasJogador.Count == 2 && totalJ == 21);

        if (totalJ > 21)
        {
            resultado = $"Estouraste! Perdeste {apostaAtual} moedas.";
        }
        else if (totalD > 21 || totalJ > totalD)
        {
            if (blackjack)
            {
                ganho = apostaAtual * 3;
                resultado = $"Blackjack! Ganhaste {ganho} moedas. (x3)";
            }
            else
            {
                ganho = apostaAtual * 2;
                resultado = $"Ganhaste {ganho} moedas. (x2)";
            }
            GameManager.instancia.AdicionarDinheiro(ganho);
        }
        else if (totalJ == totalD)
        {
            GameManager.instancia.AdicionarDinheiro(apostaAtual);
            resultado = $"Empate.\nRecebeste de volta {apostaAtual} moedas.";
        }
        else
        {
            resultado = $"Perdeste {apostaAtual} moedas.";
        }

        jogoAtivo = false;
        dealerJogando = false;
        AtualizarUI();
        txtMensagem.text = resultado;
    }

    void DarCarta(List<Carta> lista, Transform destino, bool visivel)
    {
        Carta nova = baralho.TirarCarta();
        lista.Add(nova);

        GameObject cartaGO = Instantiate(cartaPrefab, destino);
        var img = cartaGO.GetComponent<Image>();
        img.sprite = visivel ? nova.imagem : versoCarta;
    }

    int CalcularTotal(List<Carta> cartas)
    {
        int total = 0, ases = 0;
        foreach (var c in cartas)
        {
            total += c.valor;
            if (c.valor == 11) ases++;
        }
        while (total > 21 && ases > 0)
        {
            total -= 10;
            ases--;
        }
        return total;
    }

    void LimparMao(Transform hand)
    {
        foreach (Transform filho in hand)
            Destroy(filho.gameObject);
    }

    void AtualizarUI()
    {
        txtSaldo.text = "Moedas: " + GameManager.instancia.ObterDinheiro();
        txtTotalJogador.text = "Total Jogador: " + CalcularTotal(cartasJogador);

        if (jogoAtivo && !dealerJogando)
        {
            int vis = cartasDealer.Count > 0 ? cartasDealer[0].valor : 0;
            txtTotalDealer.text = "Total Dealer: " + vis + "+";
        }
        else
        {
            txtTotalDealer.text = "Total Dealer: " + CalcularTotal(cartasDealer);
        }
    }

    public void VoltarAoJogo()
    {
        SceneManager.LoadScene("Jogo");
    }
}