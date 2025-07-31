using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Trap : MonoBehaviour
{
    public float bounceForce = 10f;
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerBounce(collision.gameObject);

        }
    }
    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb)
        {
            //Resets player velocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            //Apply Bounce Force
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        }
    }
}
