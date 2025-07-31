using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMovement : MonoBehaviour
{
public Rigidbody2D rb;
public float jump = 10f;
public float moveSpeed = 10f;
bool isFacingRight = false;
bool isGrounded = false;
float horizontalMovement;
Animator animator;
// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
 {
rb = GetComponent<Rigidbody2D>();
animator = GetComponent<Animator>();
 }
    // Update is called once per frame
    void Update()
    {
        FlipSprite();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }
 
 }
 
private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", (rb.linearVelocity.y));
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
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);
    }
    private void OnTriggerExit2D(Collider2D collision)
 {
isGrounded = false;
animator.SetBool("isJumping", !isGrounded);
 }
 
public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
    
}