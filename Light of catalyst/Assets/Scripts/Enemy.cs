using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2f;
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
    public bool facingRight = true; // Which direction enemy starts facing
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        ogColor = spriteRenderer.color;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Is Grounded?
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        
        // Player direction
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        
        // Face the player
        FacePlayer(direction);
        
        // Player above detection
        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1 << player.gameObject.layer);
        
        if (isGrounded)
        {
            // Chase player
            rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);
            
            // Jump if there's gap ahead && no ground in front
            // else if there's player above and platform above
            // if Ground
            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
            
            // If platform above
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer);
            
            if (!groundInFront.collider && platformAbove.collider)
            {
                shouldJump = true;
            }
        }
    }
    
    void FacePlayer(float direction)
    {
        // Check if we need to flip
        if (direction > 0 && !facingRight)
        {
            // Player is to the right, flip to face right
            Flip();
        }
        else if (direction < 0 && facingRight)
        {
            // Player is to the left, flip to face left
            Flip();
        }
    }
    
    void Flip()
    {
        // Switch the direction flag
        facingRight = !facingRight;
        
        // Method 1: Using transform.localScale (most common)
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        
        // Alternative Method 2: Using SpriteRenderer.flipX (uncomment if you prefer this)
        // spriteRenderer.flipX = !spriteRenderer.flipX;
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
}