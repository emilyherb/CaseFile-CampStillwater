using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Input: WASD + Arrow Keys
        float horizontal = Input.GetAxisRaw("Horizontal") 
                         + (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) 
                         - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);

        float vertical = Input.GetAxisRaw("Vertical") 
                       + (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) 
                       - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);

        // Movement input (relative to character's facing direction)
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        move = Vector3.ClampMagnitude(move, 1f); // Normalize diagonal speed

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset slight downward force to stay grounded
        }
        velocity.y += gravity * Time.deltaTime;

        // Combine movement + gravity
        Vector3 finalMove = (move * moveSpeed) + (Vector3.up * velocity.y);

        // Apply movement
        controller.Move(finalMove * Time.deltaTime);
    }
}
