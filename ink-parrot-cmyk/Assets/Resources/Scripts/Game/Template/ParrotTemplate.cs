using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ParrotTemplate : MonoBehaviour
{
    public static ParrotTemplate Instance;

    private void Awake()
    {
        Instance = this;
    }

    public const float MoveDuration = 0.27f;
    public const float ScaleDuration = 0.17f;

    //==================================================

    public int CMYK;

    private RectTransform rectTransform;

    [SerializeField] private GameObject selectedAreaTarget;
    [SerializeField] private GameObject assembledAreaTarget;
    private Vector2 selectedArea;
    private Vector2 assembledArea;
    private Vector3 selectedScale;

    // 원래 위치와 크기
    private Vector2 basePosition;
    private Vector3 baseScale;

    // 마지막 인덱스는 윤곽선
    public GameObject[] BodyTemplates = new GameObject[Constants.TemplateSize + 1];

    // 0이면 잉크 없음
    // 1 이상이면 잉크 있음
    public int[] BodyTemplatesInk = new int[Constants.TemplateSize];

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        basePosition = rectTransform.position;
        baseScale = rectTransform.localScale;
        selectedArea = selectedAreaTarget.GetComponent<RectTransform>().position;
        selectedScale = selectedAreaTarget.GetComponent<RectTransform>().localScale;
        assembledArea = assembledAreaTarget.GetComponent<RectTransform>().position;
        for (int i = 0; i < Constants.TemplateSize; i++)
        {
            BodyTemplatesInk[i] = 3;
            DrawColor(i);
        }
    }

    public void Init()
    {
        for (int i = 0; i < Constants.TemplateSize; i++)
        {
            BodyTemplatesInk[i] = 3;
            DrawColor(i);
        }
    }

    public void Resetting(int template)
    {
        BodyTemplatesInk[template] = 3;
        DrawColor(template);

        GameUiManager.Instance.SetLightManagingSlider(template);
    }

    public void DrawColor(int template)
    {
        Image image = BodyTemplates[template].GetComponent<Image>();

        image.color = Constants.Instance.GetColor(
            (Constants.ColorType)CMYK,
            BodyTemplatesInk[template]);
    }

    public IEnumerator ObjectSelected()
    {
        yield return MoveCoroutine(
            selectedArea,
            MoveDuration);

        yield return ScalingCoroutine(
            selectedScale,
            ScaleDuration);

        GameManager.Instance.processing = 0;
    }

    public IEnumerator ObjectUnSelected()
    {
        yield return MoveCoroutine(
            basePosition,
            0.2f);

        yield return ScalingCoroutine(
            baseScale,
            0.1f);

        // 원래 위치 복원
        rectTransform.position = basePosition;

        GameManager.Instance.selectedColor = -1;
        GameManager.Instance.processing = 0;
    }

    private IEnumerator MoveCoroutine(Vector2 targetPosition, float duration)
    {
        Vector2 start = rectTransform.position;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += GameManager.GetdeltaTime;

            float t = elapsed / duration;

            rectTransform.anchoredPosition =
                Vector2.Lerp(start, targetPosition, t);

            yield return null;
        }

        rectTransform.position = targetPosition;
    }

    private IEnumerator ScalingCoroutine(Vector3 targetScale, float duration)
    {
        Vector3 start = rectTransform.localScale;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += GameManager.GetdeltaTime;

            float t = elapsed / duration;

            rectTransform.localScale =
                Vector3.Lerp(start, targetScale, t);

            yield return null;
        }

        rectTransform.localScale = targetScale;
    }

    public void OnTouch()
    {
        Debug.Log("터치 감지");
        GameManager.Instance.InputDetected(CMYK);
    }

    public void TemplateExtracted(int template)
    {
        BodyTemplatesInk[template] = 0;
        DrawColor(template);
    }
}