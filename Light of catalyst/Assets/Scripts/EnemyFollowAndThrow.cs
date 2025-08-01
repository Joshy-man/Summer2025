using UnityEngine;
using System.Collections;

public class EnemyFollowAndThrow : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float followRange = 10f;
    public float stopDistance = 4f; // Distance where enemy stops to throw
    
    [Header("Throwing Settings")]
    public GameObject projectilePrefab;
    public Transform throwPoint;
    public float throwForce = 10f;
    public float throwAngle = 45f;
    public float throwRate = 1f; // Throws per second
    public float throwRange = 8f;
    
    [Header("AI Behavior")]
    public bool smoothMovement = true;
    public float rotationSpeed = 5f;
    public bool facePlayer = true;
    
    private Transform player;
    private Rigidbody2D rb;
    private float nextThrowTime;
    private bool isInThrowRange;
    private bool isFollowing;
    
    public enum EnemyState { Idle, Following, Throwing }
    public EnemyState currentState = EnemyState.Idle;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Update state based on distance
        UpdateState(distanceToPlayer);
        
        // Handle state behavior
        switch (currentState)
        {
            case EnemyState.Idle:
                rb.linearVelocity = Vector2.zero;
                break;
                
            case EnemyState.Following:
                FollowPlayer();
                break;
                
            case EnemyState.Throwing:
                rb.linearVelocity = Vector2.zero; // Stop moving
                TryThrowProjectile();
                break;
        }
        
        // Face the player
        if (facePlayer && distanceToPlayer <= followRange)
        {
            FacePlayer();
        }
    }
    
    void UpdateState(float distanceToPlayer)
    {
        if (distanceToPlayer > followRange)
        {
            // Player too far - go idle
            currentState = EnemyState.Idle;
        }
        else if (distanceToPlayer <= stopDistance && distanceToPlayer <= throwRange)
        {
            // Close enough to throw
            currentState = EnemyState.Throwing;
        }
        else if (distanceToPlayer > stopDistance && distanceToPlayer <= followRange)
        {
            // Follow player to get closer
            currentState = EnemyState.Following;
        }
    }
    
    void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        
        if (smoothMovement)
        {
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }
    
    void TryThrowProjectile()
    {
        if (Time.time >= nextThrowTime && HasClearShot())
        {
            ThrowProjectile();
            nextThrowTime = Time.time + 1f / throwRate;
        }
    }
    
    bool HasClearShot()
    {
        Vector2 direction = (player.position - throwPoint.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(throwPoint.position, direction, throwRange);
        
        // Return true if we hit the player or nothing (clear shot)
        return hit.collider == null || hit.collider.CompareTag("Player");
    }
    
    void ThrowProjectile()
    {
        if (projectilePrefab == null || throwPoint == null) return;
        
        Vector2 throwVelocity = CalculateThrowVelocity();
        
        // Create projectile
        GameObject projectile = Instantiate(projectilePrefab, throwPoint.position, Quaternion.identity);
        
        // Set velocity
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.linearVelocity = throwVelocity;
        }
        
        // Optional: Rotate projectile to face direction
        float angle = Mathf.Atan2(throwVelocity.y, throwVelocity.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    Vector2 CalculateThrowVelocity()
    {
        Vector2 displacement = player.position - throwPoint.position;
        float horizontalDistance = displacement.x;
        float verticalDistance = displacement.y;
        
        // Physics calculation for projectile motion
        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float angleRad = throwAngle * Mathf.Deg2Rad;
        
        // Calculate required velocity to reach target
        float velocityMagnitude = Mathf.Sqrt(
            (gravity * horizontalDistance * horizontalDistance) /
            (2 * Mathf.Cos(angleRad) * Mathf.Cos(angleRad) * 
             (horizontalDistance * Mathf.Tan(angleRad) - verticalDistance))
        );
        
        // Handle case where calculation fails
        if (float.IsNaN(velocityMagnitude) || float.IsInfinity(velocityMagnitude))
        {
            velocityMagnitude = throwForce;
        }
        
        // Calculate velocity components
        float velocityX = velocityMagnitude * Mathf.Cos(angleRad) * Mathf.Sign(horizontalDistance);
        float velocityY = velocityMagnitude * Mathf.Sin(angleRad);
        
        return new Vector2(velocityX, velocityY);
    }
    
    void FacePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        
        // Flip sprite based on direction
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw follow range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRange);
        
        // Draw stop distance
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        
        // Draw throw range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, throwRange);
        
        // Draw state indicator
        Gizmos.color = GetStateColor();
        Gizmos.DrawWireCube(transform.position + Vector3.up * 2, Vector3.one * 0.5f);
        
        // Draw line to player when following/throwing
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= followRange)
            {
                Gizmos.color = currentState == EnemyState.Throwing ? Color.red : Color.green;
                Gizmos.DrawLine(transform.position, player.position);
            }
        }
        
        // Draw trajectory when in throwing state
        if (currentState == EnemyState.Throwing && player != null && throwPoint != null)
        {
            Vector2 velocity = CalculateThrowVelocity();
            DrawTrajectory(throwPoint.position, velocity, 2f);
        }
    }
    
    void DrawTrajectory(Vector2 startPos, Vector2 velocity, float time)
    {
        Gizmos.color = Color.green;
        Vector2 previousPos = startPos;
        
        int steps = 20;
        for (int i = 1; i <= steps; i++)
        {
            float t = (time / steps) * i;
            Vector2 pos = startPos + velocity * t + 0.5f * Physics2D.gravity * t * t;
            
            Gizmos.DrawLine(previousPos, pos);
            previousPos = pos;
        }
    }
    
    Color GetStateColor()
    {
        switch (currentState)
        {
            case EnemyState.Idle: return Color.white;
            case EnemyState.Following: return Color.yellow;
            case EnemyState.Throwing: return Color.red;
            default: return Color.white;
        }
    }
}