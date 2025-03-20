using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        
        Vector3 newPosition = transform.position + (Vector3)(movement * moveSpeed * Time.deltaTime);
        
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        transform.position = newPosition;

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetBool("IsMoving", movement.magnitude > 0);
    }
}