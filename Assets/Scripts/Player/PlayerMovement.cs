using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 movementInput;

    [SerializeField]
    private float speed = 300f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1f;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementInput.x, 0.0f, movementInput.y);
        Vector3 localMovement = transform.TransformDirection(movement);
        Vector3 velocity = localMovement * speed;
        rb.linearVelocity = velocity;
    }

    public void SetMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}
