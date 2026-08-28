using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrotTemplate : MonoBehaviour
{
    public static ParrotTemplate Instance;

    private void Awake()
    {
        Instance = this;
    }

    private const float outline_size = 1.5f;

    public const float MoveDuration = 0.27f;
    public const float ScaleDuration = 0.17f;

    //==================================================

    public int CMYK;

    private RectTransform rectTransform;

    [SerializeField] private GameObject selectedAreaTarget;
    [SerializeField] private GameObject assembledAreaTarget;
    [SerializeField] private GameObject comingGIF;
    [SerializeField] private GameObject body;

    // 0이 가장 기본 material
    [SerializeField] private List<Material> target_Materials = new List<Material>();
    private Vector2 selectedArea;
    private Vector2 assembledArea;
    private Vector3 selectedScale;

    // 원래 위치와 크기
    private Vector2 basePosition;
    private Vector3 baseScale;
    private int baseSiblingIndex;

    // 마지막 인덱스는 윤곽선
    public GameObject[] BodyTemplates = new GameObject[Constants.TemplateSize + 1];

    // 0이면 잉크 없음
    // 1 이상이면 잉크 있음
    public int[] BodyTemplatesInk = new int[Constants.TemplateSize];
    private int[] TemplatesSiblingIndex = new int[Constants.TemplateSize + 1];

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        basePosition = rectTransform.localPosition;
        baseScale = rectTransform.localScale;
        baseSiblingIndex = rectTransform.GetSiblingIndex();
        selectedArea = selectedAreaTarget.GetComponent<RectTransform>().localPosition;
        selectedScale = selectedAreaTarget.GetComponent<RectTransform>().localScale;
        assembledArea = assembledAreaTarget.GetComponent<RectTransform>().localPosition;

        ChangeMaterial(0);

        for (int i = 0; i < Constants.TemplateSize; i++)
        {
            Outline outline = BodyTemplates[i].GetComponent<Outline>();
            outline.effectDistance = new Vector2(outline_size, outline_size);
            outline.effectColor = new Color(1f, 0.92f, 0.016f, 1f);
            outline.enabled = false;

            BodyTemplatesInk[i] = 3;
            TemplatesSiblingIndex[i] = BodyTemplates[i].GetComponent<RectTransform>().GetSiblingIndex();
            DrawColor(i);
        }
        TemplatesSiblingIndex[Constants.TemplateSize] = BodyTemplates[Constants.TemplateSize].GetComponent<RectTransform>().GetSiblingIndex();

        Image comingGIFImage = comingGIF.GetComponent<Image>();
        comingGIFImage.color = Constants.Instance.GetColor((Constants.ColorType)CMYK, 3);

        body.SetActive(true);
        comingGIF.SetActive(false);
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
        ChangeSiblingLast(true);

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
        rectTransform.localPosition = basePosition;

        ChangeSiblingLast(false);

        GameManager.Instance.selectedColor = -1;
        GameManager.Instance.processing = 0;
    }

    public IEnumerator ObjectAssembled()
    {
        yield return MoveCoroutine(
            assembledArea,
            0.2f);
        
        rectTransform.localPosition = assembledArea;
    }

    public IEnumerator ObjectPositionInit()
    {
        rectTransform.localPosition = basePosition;
        PlayComingGIF();
        yield return null;
        //yield return MoveCoroutine(
        //    basePosition,
        //    0.2f);
    }

    private IEnumerator MoveCoroutine(Vector2 targetPosition, float duration)
    {
        Vector2 start = rectTransform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += GameManager.GetdeltaTime;

            float t = elapsed / duration;

            rectTransform.localPosition =
                Vector2.Lerp(start, targetPosition, t);

            yield return null;
        }

        rectTransform.localPosition = targetPosition;
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

    public void ChangeMaterial(int target_material_index)
    {
        for (int i = 0; i < Constants.TemplateSize; i++)
        {
            BodyTemplates[i].GetComponent<Image>().material = target_Materials[target_material_index];
        }
    }

    //isLast 가 true면 가장 뒤로, isLast가 false면 원래 위치로
    public void ChangeSiblingLast(bool isLast)
    {
        if(isLast)
            GetComponent<RectTransform>().SetAsLastSibling();
        else
            GetComponent<RectTransform>().SetSiblingIndex(baseSiblingIndex);
    }

     //isLast 가 true면 가장 뒤로, isLast가 false면 원래 위치로
    public void ChangeTemplateSiblingLast(int index, bool isLast)
    {
        if(isLast)
            BodyTemplates[index].GetComponent<RectTransform>().SetAsLastSibling();
        else
            BodyTemplates[index].GetComponent<RectTransform>().SetSiblingIndex(TemplatesSiblingIndex[index]);
    }

    public void TemplateSelected(int template)
    {
        Outline outline = BodyTemplates[template].GetComponent<Outline>();
        outline.enabled = true;
        ChangeTemplateSiblingLast(template, true);
        ChangeTemplateSiblingLast(Constants.TemplateSize, true);
    }

    public void TemplateUnselected()
    {
        for(int template = 0; template < Constants.TemplateSize; template++){
            Outline outline = BodyTemplates[template].GetComponent<Outline>();
            outline.enabled = false;
            ChangeTemplateSiblingLast(template, false);
        }
        ChangeTemplateSiblingLast(Constants.TemplateSize, false);
    }

    public void PlayComingGIF()
    {
        body.SetActive(false);
        comingGIF.GetComponent<GIFShower>().Setting();
        comingGIF.SetActive(true);
        StartCoroutine(PlayComingGIFProcess());
    }

    private IEnumerator PlayComingGIFProcess()
    {
        comingGIF.GetComponent<GIFShower>().ActivatingPlay();
        yield return GameManager.Instance.WaitForGameTime(0.8f); //GIF 끝날 시간쯤??

        comingGIF.SetActive(false);
        body.SetActive(true);
    }

    public void hideParrot()
    {
        comingGIF.SetActive(false);
        body.SetActive(false);
    }

    public Color getTemplateColor(int template)
    {
        return Constants.Instance.GetColor(
            (Constants.ColorType)CMYK,
            BodyTemplatesInk[template]);
    }
}