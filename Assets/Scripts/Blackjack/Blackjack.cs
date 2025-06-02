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

    [Header("Áudio")]
    public AudioClip somCarta;
    public AudioClip somVitoria;
    private AudioSource audioSource;

    private int apostaAtual = 0;

    private List<Carta> cartasJogador = new List<Carta>();
    private List<Carta> cartasDealer = new List<Carta>();

    private bool jogoAtivo = false;
    private bool dealerJogando = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        baralho.Inicializar(); // Garante que o baralho está pronto para ser usado
        AtualizarUI();
        txtMensagem.text = "Insere uma aposta e clica em Jogar.";

        // Associa o botão de voltar ao método correspondente, se o botão estiver presente na cena
        if (botaoVoltar != null)
        {
            botaoVoltar.onClick.AddListener(VoltarAoJogo);
        }
    }

    public void Jogar()
    {
        int saldoAtual = GameManager.instancia.ObterDinheiro();

        // Verifica se a aposta é válida (número positivo e dentro do saldo disponível)
        if (!int.TryParse(inputAposta.text, out apostaAtual) || apostaAtual <= 0 || apostaAtual > saldoAtual)
        {
            txtMensagem.text = "Aposta inválida.";
            return;
        }

        // Deduz a aposta do saldo do jogador
        GameManager.instancia.RemoverDinheiro(apostaAtual);
        jogoAtivo = true;
        dealerJogando = false;

        // Reinicia as mãos (jogador e dealer) e limpa o ecrã
        LimparMao(handJogador);
        LimparMao(handDealer);
        cartasJogador.Clear();
        cartasDealer.Clear();

        // Dá duas cartas ao jogador e ao dealer (sendo que a 2ª carta do dealer fica virada para baixo)
        DarCarta(cartasJogador, handJogador, true);
        DarCarta(cartasDealer, handDealer, true);
        DarCarta(cartasJogador, handJogador, true);
        DarCarta(cartasDealer, handDealer, false);

        AtualizarUI();
        txtMensagem.text = "A tua vez. Pede ou fica.";
    }

    public void Pedir()
    {
        // Verifica se o jogo está em andamento e se ainda é a vez do jogador
        if (!jogoAtivo || dealerJogando) return;

        DarCarta(cartasJogador, handJogador, true);
        AtualizarUI();

        // Verifica se o jogador ultrapassou 21 pontos
        if (CalcularTotal(cartasJogador) > 21)
        {
            txtMensagem.text = "Estouraste! Perdeste.";
            jogoAtivo = false;
            AtualizarUI();
        }
    }

    public void Ficar()
    {
        // Passa a vez para o dealer
        if (!jogoAtivo || dealerJogando) return;

        dealerJogando = true;
        StartCoroutine(DealerJoga());
    }

    private IEnumerator DealerJoga()
    {
        yield return new WaitForSeconds(1f);

        // Revela a carta virada do dealer
        var imgVirada = handDealer.GetChild(1).GetComponent<Image>();
        imgVirada.sprite = cartasDealer[1].imagem;
        AtualizarUI();

        // O dealer continua a tirar cartas até ter pelo menos 17 pontos
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

        // Determina o resultado do jogo com base nos totais
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

            if (somVitoria != null && audioSource != null)
            {
                audioSource.PlayOneShot(somVitoria);
            }
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
        // Retira uma carta do baralho e adiciona à lista de cartas da mão (jogador ou dealer)
        Carta nova = baralho.TirarCarta();
        lista.Add(nova);

        // Cria visualmente a carta na interface
        GameObject cartaGO = Instantiate(cartaPrefab, destino);
        var img = cartaGO.GetComponent<Image>();
        img.sprite = visivel ? nova.imagem : versoCarta;

        if (somCarta != null && audioSource != null)
        {
            audioSource.PlayOneShot(somCarta);
        }
    }

    int CalcularTotal(List<Carta> cartas)
    {
        int total = 0, ases = 0;

        // Soma os valores das cartas, tratando os ases como 11 inicialmente
        foreach (var c in cartas)
        {
            total += c.valor;
            if (c.valor == 11) ases++;
        }

        // Se o total ultrapassar 21, converte ases de 11 para 1 até o total ser aceitável
        while (total > 21 && ases > 0)
        {
            total -= 10;
            ases--;
        }

        return total;
    }

    void LimparMao(Transform hand)
    {
        // Remove todas as cartas visuais da mão (útil para reiniciar jogadas)
        foreach (Transform filho in hand)
            Destroy(filho.gameObject);
    }

    void AtualizarUI()
    {
        txtSaldo.text = "Moedas: " + GameManager.instancia.ObterDinheiro();
        txtTotalJogador.text = "Total Jogador: " + CalcularTotal(cartasJogador);

        // Durante o jogo, mostra apenas o valor da carta visível do dealer
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
        SceneManager.LoadScene("Jogo"); // Volta para a cena principal do jogo
    }
}