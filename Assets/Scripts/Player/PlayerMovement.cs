using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 movementInput;

    [SerializeField]
    private float speed = 300f;

    [SerializeField]
    private float jumpForce = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementInput.x, 0.0f, movementInput.y);
        Vector3 localMovement = transform.TransformDirection(movement);
        Vector3 velocity = localMovement * speed;

        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    public void SetMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void SetJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void SetSprint(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            speed *= 2f;
        }
        else if (context.canceled)
        {
            speed /= 2f;
        }
    }

    private bool IsGrounded()
    {
        float distance = 1.1f;
        bool hit = Physics.Raycast(transform.position, Vector3.down, distance);
        return hit;
    }
}
