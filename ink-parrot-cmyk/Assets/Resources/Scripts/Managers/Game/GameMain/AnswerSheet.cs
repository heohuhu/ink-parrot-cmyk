using UnityEngine;
using UnityEngine.UI;
public class AnswerSheet : MonoBehaviour
{
    public Sprite[] AnswerImages;
    public AnswerSheetType[] answerSheet =
    {
       new (new [] {new Vector3(1f, 1f, 1f), new Vector3(2f, 2f, 3f)})
    };
}

public class AnswerSheetType
{
    public Vector3[] bodyTemplates;

    public AnswerSheetType(Vector3 [] bodyTemplates)
    {
        this.bodyTemplates = bodyTemplates;
    }
}
