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

    //0이면 싹 다 짜낸 상태
    //1 이상이면 짜내지 않은 상태
    public int[] BodyTemplatesInk = new int[Constants.TemplateSize];

    void Start()
    {
        for(int i = 0; i < 7; i++){
            BodyTemplatesInk[i] = 3;
            DrawColor(i);
        }
        base_position = this.transform.position;
        base_size = this.transform.localScale;
    }

    public void Resetting(int template)
    {
        BodyTemplatesInk[template] = 3;
        DrawColor(template);
    }

    public void DrawColor(int template)
    {
        SpriteRenderer spr = BodyTemplates[template].GetComponent<SpriteRenderer>();
        Color tmp = SettingManager.Instance.GetColor((Constants.ColorType)this.CMYK);
        int N = 0;

        switch (this.BodyTemplatesInk[template])
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
        spr.color = tmp;
    }

    public IEnumerator ObjectSelected()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y, 12);
        yield return StartCoroutine(MoveCoroutine(new Vector3(3, 3, 12), 1f));
        yield return StartCoroutine(ScalingCoroutine(new Vector3(5, 5, 1), 0.5f));
        GameManager.Instance.processing = 0;
    }

    public IEnumerator ObjectUnSelected()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y, base_position.z);
        yield return StartCoroutine(MoveCoroutine(base_position, 0.7f));
        yield return StartCoroutine(ScalingCoroutine(base_size, 0.35f));
        GameManager.Instance.selectedColor = -1;
        GameManager.Instance.processing = 0;
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if(GameManager.Instance.isTimeStopped)
                continue;
            elapsedTime += Time.deltaTime;
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
            if(GameManager.Instance.isTimeStopped)
                continue;
            elapsedTime += Time.deltaTime;
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
        if(--this.BodyTemplatesInk[template] < 0)
            this.BodyTemplatesInk[template] = 0;
        DrawColor(template);
    }
}
