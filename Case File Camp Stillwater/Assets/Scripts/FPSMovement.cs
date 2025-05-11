using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public Transform playerCamera; // Assign this in the Inspector

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get input from WASD and Arrow Keys
        float horizontal = Input.GetAxisRaw("Horizontal") + (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        float vertical = Input.GetAxisRaw("Vertical") + (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);

        // Calculate movement direction relative to the camera
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;

        // Flatten to prevent vertical movement
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * horizontal + camForward * vertical;
        move = Vector3.ClampMagnitude(move, 1f);

        // Apply movement
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Reset vertical velocity if grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
}

