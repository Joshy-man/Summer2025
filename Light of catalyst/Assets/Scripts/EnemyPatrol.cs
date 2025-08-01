using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;   // Left patrol point
    public Transform pointB;   // Right patrol point
    public float speed = 2f;   // Movement speed

    private Rigidbody2D rb;
    private Transform currentPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (pointA == null || pointB == null)
        {
            Debug.LogError("Please assign PointA and PointB in the inspector!");
            enabled = false;
            return;
        }

        // Start moving towards pointB
        currentPoint = pointB;
    }

    void Update()
    {
        Vector2 direction = (currentPoint.position - transform.position).normalized;

        // Move enemy horizontally toward currentPoint
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        // Check if close to the target point
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
        {
            // Switch target point
            currentPoint = (currentPoint == pointA) ? pointB : pointA;
        }

        // Flip sprite based on movement direction
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);  // Face right
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Face left
    }
}
