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

    //==================================================
    // 선택 애니메이션 설정
    //==================================================

    // 선택 시 이동할 위치(SelectedArea 기준)
    public static readonly Vector2 SelectedPosition = Vector2.zero;

    // 선택 시 확대 배율
    public static readonly Vector3 SelectedScale = new Vector3(2f, 2f, 1f);

    public const float MoveDuration = 0.27f;
    public const float ScaleDuration = 0.17f;

    //==================================================

    public int CMYK;

    private RectTransform rectTransform;

    // 선택 시 이동할 부모(Inspector에서 지정)
    [Header("선택 시 이동할 부모")]
    [SerializeField] private RectTransform selectedArea;

    // 원래 부모
    private Transform originalParent;

    // 원래 위치와 크기
    private Vector2 basePosition;
    private Vector3 baseScale;

    // 마지막 인덱스는 윤곽선
    public GameObject[] BodyTemplates = new GameObject[Constants.TemplateSize + 1];

    // 원래의 형제 순서 저장
    private int[] baseSiblingIndex = new int[Constants.TemplateSize + 1];

    // 0이면 잉크 없음
    // 1 이상이면 잉크 있음
    public int[] BodyTemplatesInk = new int[Constants.TemplateSize];

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        originalParent = transform.parent;

        basePosition = rectTransform.anchoredPosition;
        baseScale = rectTransform.localScale;

        for (int i = 0; i < Constants.TemplateSize; i++)
        {
            BodyTemplatesInk[i] = 3;
            DrawColor(i);
        }

        SaveSiblingOrder();
    }

    public void Init()
    {
        for (int i = 0; i < Constants.TemplateSize; i++)
        {
            BodyTemplatesInk[i] = 3;
            DrawColor(i);
        }

        RestoreSiblingOrder();
    }

    public void Resetting(int template)
    {
        BodyTemplatesInk[template] = 3;
        DrawColor(template);

        GameUiManager.Instance.SetLightManagingSlider(template);
    }

    /// <summary>
    /// 처음 형제 순서를 저장
    /// </summary>
    private void SaveSiblingOrder()
    {
        for (int i = 0; i < BodyTemplates.Length; i++)
        {
            baseSiblingIndex[i] =
                BodyTemplates[i].transform.GetSiblingIndex();
        }
    }

    /// <summary>
    /// 선택 시 가장 앞으로 이동
    /// </summary>
    private void BringToFront()
    {
        foreach (GameObject obj in BodyTemplates)
        {
            obj.transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// 원래 순서 복원
    /// </summary>
    private void RestoreSiblingOrder()
    {
        for (int i = 0; i < BodyTemplates.Length; i++)
        {
            BodyTemplates[i].transform.SetSiblingIndex(baseSiblingIndex[i]);
        }
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
        BringToFront();

        // 선택 전 부모 저장
        originalParent = transform.parent;

        // SelectedArea로 이동
        transform.SetParent(selectedArea, false);

        // 중앙 정렬
        rectTransform.anchoredPosition = basePosition;

        yield return MoveCoroutine(
            SelectedPosition,
            MoveDuration);

        yield return ScalingCoroutine(
            SelectedScale,
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

        // 원래 부모로 복귀
        transform.SetParent(originalParent, false);

        // 원래 위치 복원
        rectTransform.anchoredPosition = basePosition;

        RestoreSiblingOrder();

        GameManager.Instance.selectedColor = -1;
        GameManager.Instance.processing = 0;
    }

    private IEnumerator MoveCoroutine(Vector2 targetPosition, float duration)
    {
        Vector2 start = rectTransform.anchoredPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += GameManager.GetdeltaTime;

            float t = elapsed / duration;

            rectTransform.anchoredPosition =
                Vector2.Lerp(start, targetPosition, t);

            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
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