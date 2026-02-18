using UnityEngine;
using UnityEngine.UI;
public class AnswerSheet : MonoBehaviour
{
    static public AnswerSheet Instance;

    void Awake()
    {
        Instance = this;
    }

    public Sprite[] AnswerImages;
    public AnswerSheetType[] answerSheet =
    {
       new (new [] {new Vector3(1f, 1f, 1f), new Vector3(2f, 2f, 3f)})
    };

    public Vector3 Decryption(int answer)
    {
        Vector3 result;
        result.z = answer % 4;
        answer /= 4;
        result.y = answer % 4;
        answer /= 4;
        result.x = answer;

        return result;
    }
}

public class AnswerSheetType
{
    public Vector3[] bodyTemplates;

    public AnswerSheetType(Vector3 [] bodyTemplates)
    {
        this.bodyTemplates = bodyTemplates;
    }
}

public class AnswerType
{
    ///<summary>
    /// answerType : 정답 종류를 기록합니다.
    /// -1 : 정답 아직 제시되지 않음
    /// 0 : 보통 앵무새
    /// 1 : 아이템
    /// 
    /// answer : 정답의 인덱스를 기록합니다
    ///</summary>
    public int answerType, answer;
    public int C, M, Y;

    public void SetAnswer(Vector3 answer)
    {
        C = (int)answer.x;
        M = (int)answer.y;
        Y = (int)answer.z;
    }
}