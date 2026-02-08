using UnityEngine;

public class ParrotTemplate : MonoBehaviour
{
    //앵무새 템플릿 부위별로
    public GameObject [] BodyTemplates = new GameObject[Constants.TemplateSize];

    //0이면 싹 다 짜낸 상태
    //1이면 짜내지 않은 상태
    public int[] BodyTemplatesInk = new int[Constants.TemplateSize];
}
