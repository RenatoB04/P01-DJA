using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento do jogador

    private Rigidbody2D rb;      // Referência ao Rigidbody2D para movimentação física
    private Animator animator;   // Referência ao Animator para controlar animações
    private Vector2 movement;    // Direção do movimento

    void Start()
    {
        // Obtém referências aos componentes necessários
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Lê o input do teclado (setas ou WASD)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Atualiza os parâmetros do Animator com base no movimento
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetBool("IsMoving", movement.magnitude > 0); // Activa/desactiva a animação de andar
    }

    void FixedUpdate()
    {
        // Move o jogador fisicamente com base na direção e velocidade
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}