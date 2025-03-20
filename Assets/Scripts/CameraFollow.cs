using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jogador;
    public float suavizacao = 0.1f;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - jogador.position;
    }

    void LateUpdate()
    {
        if (jogador != null)
        {
            Vector3 posicaoAlvo = jogador.position + offset;
            transform.position = Vector3.Lerp(transform.position, posicaoAlvo, suavizacao);
        }
    }
}