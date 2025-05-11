using UnityEngine;

public class FPSMouseLook : MonoBehaviour
{
    public Transform playerCamera; // Drag the camera Transform here
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;

    void Start()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Camera not assigned. Please drag your player camera into the 'Player Camera' field in the Inspector.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the camera up and down (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player left and right (yaw)
        transform.Rotate(Vector3.up * mouseX);
    }
}
