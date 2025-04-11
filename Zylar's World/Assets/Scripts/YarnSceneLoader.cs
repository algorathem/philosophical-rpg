using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class YarnSceneLoader : MonoBehaviour
{
    [YarnCommand("loadScene")]
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    [YarnCommand("fadeAndLoad")]
    public static void FadeAndLoad(string sceneName)
    {
        SceneFader fader = GameObject.FindObjectOfType<SceneFader>();
        if (fader != null)
        {
            fader.StartCoroutine(fader.FadeOutAndLoadScene(sceneName));
        }
        else
        {
            Debug.LogWarning("SceneFader not found in scene!");
        }
    }
}
