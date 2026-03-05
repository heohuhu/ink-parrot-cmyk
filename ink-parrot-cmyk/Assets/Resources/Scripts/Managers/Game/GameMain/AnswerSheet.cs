using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnswerSheet : MonoBehaviour
{
    static public AnswerSheet Instance;
    public GameObject [] BodyTemplates = new GameObject[Constants.TemplateSize];
    
    void Awake()
    {
        Instance = this;
    }

    public Sprite[] AnswerImages;
    
    public AnswerType Answer = new AnswerType();

    public void GiveProblem()
    {
        int randInt = Utility.GetRandomInt(0, 10);

        if(randInt < 100) { //테스트 단계 - 항상 정답만 제시되도록함
            MakeAnswer();
        }
    }

    public void MakeAnswer()
    {
        int size = ParrotDataManager.Instance.ParrotSheet.GetLength(0);
        int randIndex = Utility.GetRandomInt(0, size);

        Answer.SetAnswer(ParrotDataManager.Instance.ParrotSheet[randIndex].bodyTemplates);
        Answer.answer = randIndex;
        Answer.answerType = 0;

        ShowAnswerImage(Answer.answer);
    }

    public void ShowAnswerImage(int index)
    {
        //AnswerImageUI.GetComponent<Image>().sprite = this.AnswerImages[index];

        for(int template = 0; template < Constants.TemplateSize; template++)
        {
            Color C = Constants.Instance.GetColor(Constants.ColorType.Cyan, this.Answer.C[template]);
            Color M = Constants.Instance.GetColor(Constants.ColorType.Magenta, this.Answer.M[template]);
            Color Y = Constants.Instance.GetColor(Constants.ColorType.Yellow, this.Answer.Y[template]);

            Debug.Log($"[{ParrotDataManager.Instance.ParrotSheet[Answer.answer].name} 정답 이미지 출력]\nTemplate : {template}\nC : {this.Answer.C[template]}\nM : {this.Answer.M[template]}\nY : {this.Answer.Y[template]}");
            
            Color result = new Color(
                C.r * M.r * Y.r,
                C.g * M.g * Y.g,
                C.b * M.b * Y.b,
                1f
            );

            Image spr = this.BodyTemplates[template].GetComponent<Image>();

            spr.color = result;
        }
    }

    public int CompareAnswer(int [] C, int [] M, int [] Y)
    {
        int result = 0;

        for(int i = 0; i < Constants.TemplateSize; i++)
        {
            if(Answer.C[i] == C[i])
                result++;
            if(Answer.M[i] == M[i])
                result++;
            if(Answer.Y[i] == Y[i])
                result++;
        }

        return result;
    }

    public int GetAnswerScore()
    {
        return ParrotDataManager.Instance.ParrotSheet[this.Answer.answer].score;
    }

    public void CorrectAnswer()
    {
        ParrotDataManager.Instance.ParrotCollect(this.Answer.answer);
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
    public int [] C, M, Y;

    public void SetAnswer(Vector3 [] answer)
    {
        int size = answer.GetLength(0);

        C = new int[size];
        M = new int[size];
        Y = new int[size];

        for(int i = 0; i < size; i++)
        {
            C[i] = (int)answer[i].x;
            M[i] = (int)answer[i].y;
            Y[i] = (int)answer[i].z;
        }
    }
}