using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ParrotTemplate: MonoBehaviour, InputInterface
{
    public static ParrotTemplate Instances;
    
    void Awake()
    {
        Instances = this;
    }

    public int CMYK;
    Vector3 base_position; //초기 위치 기억
    Vector3 base_size;
    //앵무새 템플릿 부위별로
    public GameObject [] BodyTemplates = new GameObject[Constants.TemplateSize];
    int [] base_orderlayer = {0, 1, 1, 0, 1, 0, 1};
    //0이면 싹 다 짜낸 상태
    //1 이상이면 짜내지 않은 상태
    public int[] BodyTemplatesInk = new int[Constants.TemplateSize];

    void Start()
    {
        for(int i = 0; i < Constants.TemplateSize; i++){
            BodyTemplatesInk[i] = 3;
            DrawColor(i);
        }
        base_position = this.transform.position;
        base_size = this.transform.localScale;
        SetBodyTemplatesOrderLayer(-1);
    }

    public void Resetting(int template)
    {
        BodyTemplatesInk[template] = 3;
        DrawColor(template);
        GameUiManager.Instance.SetLightManagingSlider(template);
    }

    public void SetBodyTemplatesOrderLayer(int N)
    {
        for(int i = 0; i < Constants.TemplateSize; i++)
        {
            if(N == -1)
                BodyTemplates[i].GetComponent<SpriteRenderer>().sortingOrder = base_orderlayer[i];
            else
                BodyTemplates[i].GetComponent<SpriteRenderer>().sortingOrder = N + base_orderlayer[i];
        }
    }

    public Color GetColor(int LightType)
    {
        Color tmp = SettingManager.Instance.GetColor((Constants.ColorType)this.CMYK);
        int N = 0;

        switch (LightType)
        {
            case 0:
            N = 0;
            break;

            case 1:
            N = 33;
            break;

            case 2:
            N = 66;
            break;

            case 3:
            N = 100;
            break;
        }

        float t = Mathf.Clamp01(N / 100f);
        tmp = Color.Lerp(Color.white, tmp, t);

        return tmp;
    }

    public void DrawColor(int template)
    {
        SpriteRenderer spr = BodyTemplates[template].GetComponent<SpriteRenderer>();
        
        spr.color = this.GetColor(this.BodyTemplatesInk[template]);
    }

    public IEnumerator ObjectSelected()
    {
        SetBodyTemplatesOrderLayer(10);
        yield return StartCoroutine(MoveCoroutine(new Vector3(3, 3, 0), 0.5f));
        yield return StartCoroutine(ScalingCoroutine(new Vector3(5, 5, 1), 0.3f));
        GameManager.Instance.processing = 0;
    }

    public IEnumerator ObjectUnSelected()
    {
        yield return StartCoroutine(MoveCoroutine(base_position, 0.35f));
        yield return StartCoroutine(ScalingCoroutine(base_size, 0.2f));
        SetBodyTemplatesOrderLayer(-1);
        GameManager.Instance.selectedColor = -1;
        GameManager.Instance.processing = 0;
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += GameManager.GetdeltaTime;
            float t = elapsedTime / duration;

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // 오차 방지용 보정
        transform.position = targetPosition;
    }

    private IEnumerator ScalingCoroutine(Vector3 targetSize, float duration)
    {
        Vector3 startSize = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += GameManager.GetdeltaTime;
            float t = elapsedTime / duration;

            transform.localScale = Vector3.Lerp(startSize, targetSize, t);
            yield return null;
        }

        // 오차 방지용 보정
        transform.localScale = targetSize;
    }

    public void OnTouch() //터치 입력
    {
        Debug.Log("터치 감지");
        GameManager.Instance.InputDetected(this.CMYK);
    }

    public void TemplateExtracted(int template)
    {
        this.BodyTemplatesInk[template] = 0;
        DrawColor(template);
    }
}
