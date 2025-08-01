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
public float damageRate = 1f; // Damage every 1 second
private float lastDamageTime;

void Start()
 {
ResetHealth();
spriteRenderer = GetComponent<SpriteRenderer>();
GameController.OnReset += ResetHealth;
 }
private void OnTriggerEnter2D(Collider2D collision)
 {
// Take immediate damage on first contact
Enemy enemy = collision.GetComponent<Enemy>();
if (enemy)
 {
TakeDamage(enemy.damage);
lastDamageTime = Time.time; // Set the timer so continuous damage waits
 }
Trap trap = collision.GetComponent<Trap>();
if (trap && trap.damage > 0)
 {
TakeDamage(trap.damage);
lastDamageTime = Time.time; // Set the timer so continuous damage waits
 }
 }

private void OnTriggerStay2D(Collider2D collision)
{
    if (Time.time - lastDamageTime < damageRate) return;
    
    Enemy enemy = collision.GetComponent<Enemy>();
    if (enemy)
    {
        TakeDamage(enemy.damage);
        lastDamageTime = Time.time;
    }
    
    Trap trap = collision.GetComponent<Trap>();
    if (trap && trap.damage > 0)
    {
        TakeDamage(trap.damage);
        lastDamageTime = Time.time;
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