using UnityEngine;

public class PlatformDematerialize : MonoBehaviour
{
    [Header("Disappearance Settings")]
    [SerializeField] private float delayBeforeDisappearing = 0.01f; // Time before the platform disappears
    [SerializeField] private bool destroyPlatform = false; // Whether to destroy or deactivate the platform
    [SerializeField] private float reappearTime = 5f; // Time before the platform reappears (if not destroyed)

    [Header("Visual Effects")]
    [SerializeField] private bool enableFadeEffect = true; // Whether to enable fading effect
    [SerializeField] private float fadeSpeed = 1f; // Speed of fading

    [Header("Sound Effects")]
    [SerializeField] private AudioClip dematerializeSound; // Sound to play when disappearing
    [SerializeField] private AudioClip reappearSound; // Sound to play when reappearing

    private Renderer platformRenderer;
    private Color originalColor;
    private bool isTriggered = false;

    private void Start()
    {
        // Get the platform's renderer and save the original color
        platformRenderer = GetComponent<Renderer>();
        if (platformRenderer != null)
        {
            originalColor = platformRenderer.material.color;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player steps on the platform
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // Prevent multiple triggers during a single cycle
            StartCoroutine(DematerializePlatform());
        }
    }

    private System.Collections.IEnumerator DematerializePlatform()
    {
        // Optional: Play sound effect for disappearing
        if (dematerializeSound != null)
        {
            AudioSource.PlayClipAtPoint(dematerializeSound, transform.position);
        }

        // Optional: Add fade effect before disappearing
        if (enableFadeEffect && platformRenderer != null)
        {
            yield return StartCoroutine(FadeOut());
        }
        else
        {
            yield return new WaitForSeconds(delayBeforeDisappearing);
        }

        if (destroyPlatform)
        {
            Destroy(gameObject); // Destroy the platform
        }
        else
        {
            gameObject.SetActive(false); // Deactivate the platform
            Invoke(nameof(ReactivatePlatform), reappearTime); // Schedule reactivation
        }
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float alpha = originalColor.a;

        while (alpha > 0)
        {
            alpha -= fadeSpeed * Time.deltaTime;
            platformRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }

    private void ReactivatePlatform()
    {
        // Reactivate the platform
        gameObject.SetActive(true);

        // Restore visual properties
        if (platformRenderer != null)
        {
            platformRenderer.material.color = originalColor; // Restore the original color
        }

        // Optional: Play sound effect for reappearing
        if (reappearSound != null)
        {
            AudioSource.PlayClipAtPoint(reappearSound, transform.position);
        }

        // Reset the trigger so the platform can dematerialize again
        isTriggered = false;
    }
}
