using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    private Vector3 nextPosition;

    void Start()
    {
        // Debug the initial setup
        if (pointA == null || pointB == null)
        {
            Debug.LogError("PointA or PointB is not assigned!");
            return;
        }
        
        Debug.Log($"PointA position: {pointA.position}");
        Debug.Log($"PointB position: {pointB.position}");
        Debug.Log($"Platform starting position: {transform.position}");
        
        nextPosition = pointB.position;
        Debug.Log($"Initial target: {nextPosition}");
    }

    void Update()
    {
        Vector3 oldPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
        
        // Debug movement
        if (Vector3.Distance(oldPosition, transform.position) > 0.001f)
        {
            Debug.Log($"Moving from {oldPosition} to {transform.position}, target: {nextPosition}");
        }
        
        // Check if we've reached the target
        if (Vector3.Distance(transform.position, nextPosition) < 0.01f)
        {
            Debug.Log("Reached target! Switching direction.");
            nextPosition = (Vector3.Distance(nextPosition, pointA.position) < 0.01f) ? pointB.position : pointA.position;
            Debug.Log($"New target: {nextPosition}");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}