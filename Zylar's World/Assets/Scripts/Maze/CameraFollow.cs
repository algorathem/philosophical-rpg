using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The target the camera will follow
    public Transform target;

    // Offset values for the camera's position relative to the target
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    // Smoothness factor for camera movement
    public float smoothSpeed = 0.125f;

    // Rotation speed for camera rotation around the target
    public float rotationSpeed = 50f;

    // Current rotation angle
    private float currentRotation = 0f;

    void LateUpdate()
    {
        // Ensure the target is assigned
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: Target is not assigned.");
            return;
        }

        // Rotate the camera around the target when Q or E is pressed
        HandleCameraRotation();

        // Calculate the desired position based on the offset and current rotation
        Quaternion rotation = Quaternion.Euler(0f, currentRotation, 0f);
        Vector3 rotatedOffset = rotation * offset;
        Vector3 desiredPosition = target.position + rotatedOffset;

        // Smoothly move the camera to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;

        // Make the camera look at the target
        transform.LookAt(target);
    }

    void HandleCameraRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            currentRotation -= rotationSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E))
        {
            currentRotation += rotationSpeed * Time.deltaTime;
        }
    }
}