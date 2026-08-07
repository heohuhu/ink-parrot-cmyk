using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeShower : MonoBehaviour
{
    [SerializeField] private float fadeInTime = 0.3f;
    [SerializeField] private float stayTime = 2f;
    [SerializeField] private float fadeOutTime = 0.3f;

    private CanvasGroup canvasGroup;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void Play()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        gameObject.SetActive(true);

        yield return Fade(0f, 1f, fadeInTime);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        yield return new WaitForSeconds(stayTime);

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        yield return Fade(1f, 0f, fadeOutTime);

        gameObject.SetActive(false);
    }

    private IEnumerator Fade(float start, float end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = end;
    }
}