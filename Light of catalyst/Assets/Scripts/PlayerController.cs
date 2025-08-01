using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jump = 10f;
    public float moveSpeed = 10f;

    public Text WINTEXT;
    
    bool isFacingRight = false;
    bool isGrounded = false;
    float horizontalMovement;
    Animator animator;
    
    // Get references to specific colliders
    private BoxCollider2D groundCollider;
    private CapsuleCollider2D triggerCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // Get specific collider references
        groundCollider = GetComponent<BoxCollider2D>();
        triggerCollider = GetComponent<CapsuleCollider2D>();
        
        Debug.Log("BoxCollider2D found: " + (groundCollider != null));
        Debug.Log("CapsuleCollider2D found: " + (triggerCollider != null));
        if (triggerCollider != null) Debug.Log("CapsuleCollider is trigger: " + triggerCollider.isTrigger);
        if (groundCollider != null) Debug.Log("BoxCollider is trigger: " + groundCollider.isTrigger);
    }

    void Update()
    {
        FlipSprite();
        
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump pressed! isGrounded: " + isGrounded);
            if (isGrounded)
            {
                rb.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
                Debug.Log("Jump executed!");
            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        if (animator != null)
        {
            animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
            animator.SetFloat("yVelocity", (rb.linearVelocity.y));
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontalMovement < 0f || !isFacingRight && horizontalMovement > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x = isFacingRight ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }

    // COLLISION DETECTION (BoxCollider2D - for solid ground)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ")");
        
        // Check if it's ground or moving platform
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.GetComponent<MovingPlatform>() != null)
        {
            // Safety check for contacts array
            if (collision.contacts != null && collision.contacts.Length > 0)
            {
                Vector2 normal = collision.contacts[0].normal;
                if (normal.y > 0.5f) // Landing on top (more forgiving than 0.8f)
                {
                    isGrounded = true;
                    Debug.Log("Grounded = true (collision)");
                }
            }
            else
            {
                // Fallback if no contact info available
                isGrounded = true;
                Debug.Log("Grounded = true (collision - fallback)");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.GetComponent<MovingPlatform>() != null)
        {
            isGrounded = false;
            Debug.Log("Grounded = false (collision)");
        }
    }

    // TRIGGER DETECTION (CapsuleCollider2D - for damage/pickups ONLY)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger with: " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ")");

        // Handle damage/pickups here - DON'T change isGrounded in triggers
        // Example: if (collision.CompareTag("Enemy")) { TakeDamage(); }
        if (collision.tag == "Win")
        {
            WINTEXT.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exit with: " + collision.gameObject.name);
        // Handle trigger exits - DON'T change isGrounded in triggers
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
}