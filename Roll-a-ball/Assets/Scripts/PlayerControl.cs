using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{// delcare variable
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    [SerializeField] private float speed = 0;



    void Start()
    {
        //Gets and store the RigidBody component attached to player
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // creates movement vector using x and y inputs
        //vector3 is using x, y, and z as your position
        //vector2 is just x and y
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }
    void OnMove(InputValue movementValue)
    {
        // stores the x and y components of the movement
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }
}
