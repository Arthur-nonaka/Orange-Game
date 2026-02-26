using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 movementInput;
    public Transform head;

    public bool sprintUnlocked = false;

    [SerializeField]
    private float speed = 300f;

    [SerializeField]
    private float footstepInterval = 0.5f;

    [SerializeField]
    private float sprintFootstepInterval = 0.3f;

    private float footstepTimer = 0f;
    private bool isSprinting = false;

    [SerializeField]
    private float jumpForce = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        TerrainUpgradeManager.Instance.ResetToBasic();
    }

    void FixedUpdate()
    {
        Vector3 headForward = head.forward;
        headForward.y = 0;

        if (headForward.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(headForward);
        }

        Vector3 movement = new Vector3(movementInput.x, 0.0f, movementInput.y);
        Vector3 localMovement = transform.TransformDirection(movement);
        Vector3 velocity = localMovement * speed;

        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        bool isMoving = movementInput.sqrMagnitude > 0.01f;

        if (isMoving && IsGrounded())
        {
            footstepTimer -= Time.fixedDeltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootstepSound();
                footstepTimer = isSprinting ? sprintFootstepInterval : footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void PlayFootstepSound()
    {
        SoundManager.PlaySound(SoundType.FOOTSTEP, 0.25f);
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
        if (!sprintUnlocked)
            return;

        if (context.performed)
        {
            isSprinting = true;
            speed *= 2f;
        }
        else if (context.canceled)
        {
            isSprinting = false;
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
