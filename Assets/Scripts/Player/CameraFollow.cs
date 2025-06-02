using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jogador;          // Referência ao transform do jogador
    public float suavizacao = 0.1f;    // Controla a suavidade da transição da câmara

    private Vector3 offset;            // Distância fixa entre a câmara e o jogador

    void Start()
    {
        // Calcula o offset inicial entre a câmara e o jogador
        offset = transform.position - jogador.position;
    }

    void LateUpdate()
    {
        if (jogador != null)
        {
            // Define a posição alvo da câmara com base na posição do jogador e o offset
            Vector3 posicaoAlvo = jogador.position + offset;

            // Move a câmara suavemente em direção à posição alvo
            transform.position = Vector3.Lerp(transform.position, posicaoAlvo, suavizacao);
        }
    }
}