using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class YarnSceneLoader : MonoBehaviour
{
    [YarnCommand("loadScene")]
    public static void LoadScene(string sceneName)
    {
        if (SceneController.instance != null)
        {
            SceneController.instance.LoadSceneByName(sceneName);
        }
        else
        {
            Debug.LogWarning("SceneController not found, loading directly.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
