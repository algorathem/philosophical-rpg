using UnityEngine;
using System.Collections;

public class PlatformReactivator : MonoBehaviour
{
    public static PlatformReactivator Instance;

    private void Awake()
    {
        // Ensure there is only one instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReactivatePlatform(GameObject platform, float reappearTime, AudioClip reappearSound)
    {
        StartCoroutine(ReactivationCoroutine(platform, reappearTime, reappearSound));
    }

    private IEnumerator ReactivationCoroutine(GameObject platform, float reappearTime, AudioClip reappearSound)
    {
        yield return new WaitForSeconds(reappearTime);

        // Reactivate the platform
        platform.SetActive(true);

        // Restore the platform's visuals
        Renderer renderer = platform.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color originalColor = renderer.material.color;
            renderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f); // Fully opaque
        }

        // Play reappear sound
        if (reappearSound != null)
        {
            AudioSource.PlayClipAtPoint(reappearSound, platform.transform.position);
        }
    }
}
