using System;
using UnityEngine;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;

    private SpriteRenderer spriteRenderer;

    public static event Action OnPlayerDied;

    void Start()
    {
        ResetHealth();

        spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.OnReset += ResetHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage); // Fixed: Added damage parameter
        }
        Trap trap = collision.GetComponent<Trap>();
        if (trap && trap.damage > 0)
        {
            TakeDamage(trap.damage);
        }
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            // Player dead -- call game over animation
            OnPlayerDied.Invoke();
        }
    }
    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}