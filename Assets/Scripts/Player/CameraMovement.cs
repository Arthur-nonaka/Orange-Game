using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cameraTransform;

    public Vector3 offset;
    public float sensitivity = 2f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private Vector2 mouseDelta;
    private float pitch = 20f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraTransform.position = playerTransform.position + offset;
    }

    void LateUpdate()
    {
        float yaw = mouseDelta.x * sensitivity;
        float pitchDelta = -mouseDelta.y * sensitivity;

        pitch = Mathf.Clamp(pitch + pitchDelta, minPitch, maxPitch);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        playerTransform.Rotate(Vector3.up, yaw, Space.World);
    }

    public void SetCamera(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
}
