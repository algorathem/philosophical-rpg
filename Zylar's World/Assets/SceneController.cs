using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [SerializeField] private Animator transitionAnim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadSceneCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(int buildIndex)
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1f); // Make sure it matches your animation duration

        yield return SceneManager.LoadSceneAsync(buildIndex);

        transitionAnim.SetTrigger("Start");
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1f);

        yield return SceneManager.LoadSceneAsync(sceneName);

        transitionAnim.SetTrigger("Start");
    }
}
