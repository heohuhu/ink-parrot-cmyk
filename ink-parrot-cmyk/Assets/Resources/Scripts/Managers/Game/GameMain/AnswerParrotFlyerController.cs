using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnswerParrotFlyerController : MonoBehaviour
{
    public static AnswerParrotFlyerController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public GameObject MainAnswerParrotFlyer; //날아가는 앵무새 부위 상위 오브젝트
    public GameObject[] MainAnswerParrotFlyerTemplates = new GameObject[Constants.TemplateSize + 1];

    //public IEnumerator PlayFlyer()
    
}
