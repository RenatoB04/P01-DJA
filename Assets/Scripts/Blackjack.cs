using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private int saldo = 1000;
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
    }

    public void Jogar()
    {
        if (!int.TryParse(inputAposta.text, out apostaAtual) || apostaAtual <= 0 || apostaAtual > saldo)
        {
            txtMensagem.text = "Aposta inválida.";
            return;
        }

        saldo -= apostaAtual;
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

        if (totalJ > 21) resultado = "Estouraste! Perdeste.";
        else if (totalD > 21 || totalJ > totalD)
        {
            resultado = "Ganhaste!";
            saldo += apostaAtual * 2;
        }
        else if (totalJ == totalD)
        {
            resultado = "Empate.";
            saldo += apostaAtual;
        }
        else resultado = "Perdeste.";

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
        txtSaldo.text = "Saldo: " + saldo + "€";
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
}
