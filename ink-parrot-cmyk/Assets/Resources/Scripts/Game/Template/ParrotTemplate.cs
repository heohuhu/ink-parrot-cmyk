using UnityEngine;

public class ParrotTemplate : MonoBehaviour
{
    //앵무새 템플릿 부위별로
    public GameObject body, head, wing;
    public float body_ink, head_ink, wing_ink;
    
    /// <summary>
    /// 0: Template
    /// 1: Cyan
    /// 2: Magenta
    /// 3: Yellow
    /// </summary>
    public int ColorType;

}
