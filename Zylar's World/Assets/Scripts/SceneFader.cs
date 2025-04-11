using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage; // ← Drag your FadePanel's Image component here
    public float fadeDuration = 1f;

    public IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);

        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Clamp((t / fadeDuration) * 255f, 0f, 255f);
            fadeImage.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // Optional tiny pause

        SceneManager.LoadScene(sceneName);
    }
}
