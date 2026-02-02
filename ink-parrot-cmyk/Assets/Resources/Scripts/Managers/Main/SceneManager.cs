using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [Header("Fade Option")]
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        // 싱글톤 처리
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadSceneAdditive("StartMenu");
    }

    /* =====================
     * Scene Load
     * ===================== */

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        yield return FadeOut();

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOp.isDone)
        {
            yield return null;
        }

        yield return FadeIn();
    }

    public void LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    /* =====================
     * Reload
     * ===================== */

    public void ReloadCurrentScene()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    /* =====================
     * Fade
     * ===================== */

    private IEnumerator FadeOut()
    {
        if (fadeCanvas == null)
            yield break;

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 1f;
    }

    private IEnumerator FadeIn()
    {
        if (fadeCanvas == null)
            yield break;

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 0f;
    }

    /* =====================
     * Utility
     * ===================== */

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
