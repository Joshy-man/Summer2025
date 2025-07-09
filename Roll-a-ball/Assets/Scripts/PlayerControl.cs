using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerControl : MonoBehaviour
{// delcare variable
    private Rigidbody rb;

    private int count;
    private float movementX;
    private float movementY;

    [SerializeField] private float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject WinTextObject;


    void Start()
    {
        //Gets and store the RigidBody component attached to player
        rb = GetComponent<Rigidbody>();

        count = 0;

        SetCountText();
        WinTextObject.SetActive(false);
    }

    void SetCountText()
    {
        countText.text = "Count:" + count.ToString();
        if (count >= 7)
        {
            WinTextObject.SetActive(true);
        }
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);

            count = count + 1;

            SetCountText();
        }

    }
}