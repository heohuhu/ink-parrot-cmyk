using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [Header("Fade Option")]
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] public bool isGameTimeStopped = false;

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
        LoadSceneAdditiveAsActive("StartMenu");
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

    public void GameRestart()
    {
        StartCoroutine(GameRestartCoroutine());
    }

    private IEnumerator GameRestartCoroutine()
    {
        AsyncOperation loadOp =
            SceneManager.LoadSceneAsync(
                "MainSystem",
                LoadSceneMode.Single
            );

        yield return loadOp;

        yield return ChangeSceneCoroutine("StartMenu");
    }

    //** ActiveScene을 언로드 하기 전에 반드시 Active를 다른 씬에 넘겨주기!
    public void SetActiveScene(string sceneName)
    {
        Scene gameScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(gameScene);
    }

    public void LoadSceneAdditiveAsActive(string sceneName)
    {
        StartCoroutine(SceneAdditiveAsActive(sceneName));
    }

    private IEnumerator SceneAdditiveAsActive(string sceneName)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!loadOp.isDone)
            yield return null;

        Scene mainScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(mainScene);

        if(sceneName == "Game")
            MainGameLoader.Instacne.StartLoader();
    }

    /* =====================
     * Reload
     * ===================== */

    public void ReloadCurrentScene()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void ReloadScene(string CurrentSceneName)
    {
        UnloadScene(CurrentSceneName);
        LoadSceneAdditiveAsActive(CurrentSceneName);
    }

    /* =====================
     * Fade
     * 근데 필요할 지는 모름
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

    private Dictionary<string, AsyncOperation> loadingScenes = new Dictionary<string, AsyncOperation>();

    public void PreloadScene(string sceneName)
    {
        StartCoroutine(PreloadSceneCoroutine(sceneName));
    }

    private IEnumerator PreloadSceneCoroutine(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        // 이미 로드됨
        if (scene.isLoaded)
            yield break;

        // 이미 로딩 중
        if (loadingScenes.ContainsKey(sceneName))
            yield break;

        AsyncOperation loadOp =
            SceneManager.LoadSceneAsync(
                sceneName,
                LoadSceneMode.Additive
            );

        loadingScenes.Add(sceneName, loadOp);

        yield return loadOp;

        loadingScenes.Remove(sceneName);
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeSceneCoroutine(sceneName));
    }

    private IEnumerator ChangeSceneCoroutine(string sceneName)
    {
        Scene previousScene =
            SceneManager.GetActiveScene();

        Scene targetScene =
            SceneManager.GetSceneByName(sceneName);

        // 아직 로드되지 않았다면 로드
        if (!targetScene.isLoaded)
        {
            AsyncOperation loadOp =
                SceneManager.LoadSceneAsync(
                    sceneName,
                    LoadSceneMode.Additive
                );

            yield return loadOp;

            targetScene =
                SceneManager.GetSceneByName(sceneName);
        }

        // 새 씬 활성화
        SceneManager.SetActiveScene(targetScene);

        // 이전 Scene 제거
        if (previousScene.name != "MainSystem")
        {
            yield return SceneManager
                .UnloadSceneAsync(previousScene);
        }
    }
}
