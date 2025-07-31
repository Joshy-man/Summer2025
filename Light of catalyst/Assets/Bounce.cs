using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bounce : MonoBehaviour
{
    public float bounceForce = 10f;
    public int damage = 1;
    public bool destroyOnContact = true; // Toggle in inspector
    public float destroyDelay = 0f; // Optional delay before destruction
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerBounce(collision.gameObject);
            
            // Option 1: Destroy immediately
            if (destroyOnContact && destroyDelay == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb)
        {
            // Resets player velocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            // Apply Bounce Force
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}