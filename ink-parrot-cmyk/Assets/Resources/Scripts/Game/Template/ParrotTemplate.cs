using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ParrotTemplate: MonoBehaviour
{
    public int CMYK;
    Vector3 base_position; //초기 위치 기억
    Vector3 base_size;
    //앵무새 템플릿 부위별로
    public GameObject [] BodyTemplates = new GameObject[Constants.TemplateSize];

    //0이면 싹 다 짜낸 상태
    //1이면 짜내지 않은 상태
    public int[] BodyTemplatesInk = new int[Constants.TemplateSize];

    void Start()
    {
        for(int i = 0; i < 3; i++)
            DrawColor(i);
        base_position = this.transform.position;
        base_size = this.transform.localScale;
    }

    public void Resetting(int template)
    {
        BodyTemplatesInk[template] = 1;
        DrawColor(template);
    }

    public void DrawColor(int template)
    {
        SpriteRenderer spr = BodyTemplates[template].GetComponent<SpriteRenderer>();
        spr.color = SettingManager.Instance.GetColor((Constants.ColorType)this.CMYK);
        Debug.Log("오브젝트 색상 지정됨");
    }

    public IEnumerator ObjectSelected()
    {
        yield return StartCoroutine(MoveCoroutine(new Vector3(0, 0, 0), 1.5f));
        yield return StartCoroutine(ScalingCoroutine(new Vector3(5, 5, 1), 1f));
    }

    public IEnumerator ObjectUnSelected()
    {
        yield return StartCoroutine(MoveCoroutine(base_position, 1.5f));
        yield return StartCoroutine(ScalingCoroutine(base_size, 1f));
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
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
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            transform.localScale = Vector3.Lerp(startSize, targetSize, t);
            yield return null;
        }

        // 오차 방지용 보정
        transform.position = targetSize;
    }

    void OnMouseDown() //터치 입력
    {
        GameManager.Instance.InputDetected(this.CMYK);
    }
}
