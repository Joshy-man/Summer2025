using UnityEngine;

public class HitStomp : MonoBehaviour

{

    public float bounce;
    public Rigidbody2D rb2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bounce"))
        {
            Destroy(other.gameObject);
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, bounce);
        }
    }
}
