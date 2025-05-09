using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 moveDirection = Vector3.right; // Direction to move
    public float moveDistance = 5f; // Distance to move in the specified direction
    public float moveSpeed = 2f; // Speed of the platform's movement
    public float pauseTime = 1f; // Time to pause at the target or original position

    private Vector3 originalPosition; // Starting position of the platform
    private Vector3 targetPosition; // Furthest position of the platform
    private bool isMoving = false; // Whether the platform is currently moving
    private bool completeJourney = false; // Whether the platform should return to its original position after the player leaves
    private bool movingToTarget = true; // Direction of movement

    void Start()
    {
        // Set the original and target positions
        originalPosition = transform.position;
        targetPosition = originalPosition + moveDirection.normalized * moveDistance;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 target = movingToTarget ? targetPosition : originalPosition;

            // Smoothly move the platform
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime);

            // Check if the platform reached the target
            if (Vector3.Distance(transform.position, target) <= 0.01f)
            {
                StartCoroutine(PauseBeforeSwitchingDirection());
            }
        }
    }

    private System.Collections.IEnumerator PauseBeforeSwitchingDirection()
    {
        isMoving = false; // Temporarily stop movement
        yield return new WaitForSeconds(pauseTime);

        // Check if the journey should complete after the player steps off
        if (completeJourney)
        {
            if (!movingToTarget)
            {
                // If the platform is back at the original position, stop completely
                completeJourney = false;
                isMoving = false;
                yield break;
            }
        }

        // Switch direction and continue moving
        movingToTarget = !movingToTarget;
        isMoving = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isMoving = true; // Start moving the platform
            completeJourney = false; // Reset journey completion behavior

            Transform playerTransform = collision.transform;

            // Create a helper GameObject to decouple the scale
            GameObject rotationHelper = new GameObject("RotationHelper");
            rotationHelper.transform.position = playerTransform.position;
            rotationHelper.transform.rotation = playerTransform.rotation;

            // Parent the helper to the platform
            rotationHelper.transform.SetParent(transform, true);

            // Parent the player to the helper (preserves scale and allows rotation)
            playerTransform.SetParent(rotationHelper.transform, true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Transform playerTransform = collision.transform;

            // Detach the player from the helper
            playerTransform.SetParent(null, true);

            // Find and destroy the rotation helper
            Transform rotationHelper = transform.Find("RotationHelper");
            if (rotationHelper != null)
            {
                Destroy(rotationHelper.gameObject);
            }

            playerTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            completeJourney = true; // Flag the platform to return to its original position
        }
    }
}
