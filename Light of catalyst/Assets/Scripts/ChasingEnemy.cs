using UnityEngine;
using System.Collections;

public class ChasingEnemy : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2f;
    public float chaseDistance = 5f;
    public float jumpForce = 2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    public int damage = 1;
    public int maxHealth = 3;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color ogColor;

    [Header("Facing Settings")]
    public bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        ogColor = spriteRenderer.color;
    }

    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseDistance)
        {
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            FacePlayer(direction);

            bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1 << player.gameObject.layer);

            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);

                RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
                RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer);

                if (!groundInFront.collider && platformAbove.collider)
                {
                    shouldJump = true;
                }
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    void FacePlayer(float direction)
    {
        if (direction > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
{
    facingRight = !facingRight;

    Vector3 scale = transform.localScale;
    scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
    transform.localScale = scale;
}


    private void FixedUpdate()
    {
        if (isGrounded && shouldJump)
        {
            shouldJump = false;
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 jumpDirection = direction * jumpForce;
            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashWhite());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = ogColor;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
