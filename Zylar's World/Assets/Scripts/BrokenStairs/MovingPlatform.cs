using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 moveDirection = Vector3.right; // Direction to move (e.g., Vector3.right or Vector3.forward)
    public float moveDistance = 5f; // Distance to move in the specified direction
    public float moveSpeed = 2f; // Speed of the platform's movement
    public float pauseTime = 1f; // Time to pause at the furthest position before returning

    private Vector3 originalPosition; // Starting position of the platform
    private Vector3 targetPosition; // Furthest position of the platform
    private bool isMoving = false; // Whether the platform is currently moving

    void Start()
    {
        // Set the original and target positions
        originalPosition = transform.position;
        targetPosition = originalPosition + moveDirection.normalized * moveDistance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When the player steps on the platform, attach them to the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            Transform playerTransform = collision.transform;

            // Store the player's original scale
            Vector3 originalScale = playerTransform.localScale;

            // Parent the player to the platform
            playerTransform.SetParent(transform);

            // Restore the player's original scale to prevent distortion
            playerTransform.localScale = originalScale;
        }

        // Trigger movement when the platform is not already moving
        if (!isMoving)
        {
            StartCoroutine(MovePlatform());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Detach the player from the platform when they step off
        if (collision.gameObject.CompareTag("Player"))
        {
            Transform playerTransform = collision.transform;

            // Store the player's original world scale
            Vector3 originalScale = playerTransform.localScale;

            // Detach the player from the platform
            playerTransform.SetParent(null);

            // Restore the player's original scale to prevent distortion
            playerTransform.localScale = originalScale;
        }
    }

    private System.Collections.IEnumerator MovePlatform()
    {
        isMoving = true;

        // Move to the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Pause at the target position
        yield return new WaitForSeconds(pauseTime);

        // Move back to the original position
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}
