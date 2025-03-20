using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento
    public Vector2 minBounds;    // Limite mínimo (canto inferior esquerdo)
    public Vector2 maxBounds;    // Limite máximo (canto superior direito)

    private Animator animator;   // Referência ao Animator
    private Vector2 movement;    // Direção do movimento

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Captura o input do teclado (WASD ou setas)
        movement.x = Input.GetAxis("Horizontal"); // Esquerda/Direita (A/D ou setas)
        movement.y = Input.GetAxis("Vertical");   // Cima/Baixo (W/S ou setas)

        // Move o personagem
        Vector3 newPosition = transform.position + (Vector3)(movement * moveSpeed * Time.deltaTime);

        // Restringe a posição do jogador aos limites
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        // Aplica a nova posição
        transform.position = newPosition;

        // Atualiza os parâmetros do Animator
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetBool("IsMoving", movement.magnitude > 0);
    }
}