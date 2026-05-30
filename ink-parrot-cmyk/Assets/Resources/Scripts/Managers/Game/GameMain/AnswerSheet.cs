using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class AnswerSheet : MonoBehaviour
{
    static public AnswerSheet Instance;

    public GameObject AnswerImageUI;
    public GameObject BodyTemplates_Parents;
    public GameObject [] BodyTemplates = new GameObject[Constants.TemplateSize];
    public List<bool> is_corrected = new List<bool>();
    void Awake()
    {
        Instance = this;
    }

    public Sprite[] AnswerImages;
    
    public AnswerType Answer = new AnswerType();

    void Start()
    {
        Setting();
    }
    public void Setting()
    {
        int Size = ParrotDataManager.Instance.ParrotSheet.GetLength(0);
        is_corrected = new List<bool>();

        for(int i = 0; i < Size; i++)
            is_corrected.Add(false);
    }

    //option이 1이면 항상 정답만 제시되도록 함
    public void GiveProblem(int option)
    {
        int randInt = Utility.GetRandomInt(0, 8); // 0 ~ 7
        AudioManager.Instance.PlaySFX("문조지저귐");

        if(option == 1 || randInt < 7) {
            MakeAnswer();
        }else if(randInt < 10)
        {
            MakeItem();
        }
    }
    public void GiveProblem()
    {
        int randInt = Utility.GetRandomInt(0, 8); // 0 ~ 7
        AudioManager.Instance.PlaySFX("문조지저귐");

        if(randInt < 7) {
            MakeAnswer();
            StartCoroutine(PlaySoundAfterSeconds(0.5f, "문제넘기기"));
        }else if(randInt < 10)
        {
            MakeItem();
        }
    }
    
    private IEnumerator PlaySoundAfterSeconds(float delay, string Name)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.Play(Name);
    }

    public void MakeItem()
    {
        Answer.answer = Utility.GetRandomInt(0, 2);;
        Answer.answerType = 1;

        ShowItemImage(Answer.answer);
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

    public void AnswerSubmit()
    {
        if(Answer.answerType == 0)
        {
            int score = GameManager.Instance.GetCurrentScore();

            if(score == Constants.TemplateSize * 3){ // 모든 부위가 정답일 경우
                ScoreManager.Instance.GetScore(this.GetAnswerScore());
                this.CorrectAnswer();
            }
            this.GiveProblem();
        }else if(Answer.answerType == 1)
        {
            switch (Answer.answer)
            {
                case 0:
                Timer.Instance.TimeModify(5f);
                break;

                case 1:
                Timer.Instance.TimeModify(-5f);
                break;
            }
            this.GiveProblem();
        }
    }

    public void ShowItemImage(int index)
    {
        AnswerImageUI.SetActive(true);
        BodyTemplates_Parents.SetActive(false);
        AnswerImageUI.GetComponent<Image>().sprite = this.AnswerImages[index];
    }

    public void ShowAnswerImage(int index)
    {
        BodyTemplates_Parents.SetActive(true);
        AnswerImageUI.SetActive(false);
        //AnswerImageUI.GetComponent<Image>().sprite = this.AnswerImages[index];

        for(int template = 0; template < Constants.TemplateSize; template++)
        {
            Color C = Constants.Instance.GetColor(Constants.ColorType.Cyan, this.Answer.C[template]);
            Color M = Constants.Instance.GetColor(Constants.ColorType.Magenta, this.Answer.M[template]);
            Color Y = Constants.Instance.GetColor(Constants.ColorType.Yellow, this.Answer.Y[template]);

            Debug.Log($"[{ParrotDataManager.Instance.ParrotSheet[Answer.answer].name} 정답 이미지 출력]\nTemplate : {template}\nM : {this.Answer.M[template]}\nY : {this.Answer.Y[template]}\nC : {this.Answer.C[template]}\n");
            
            Color result = Utility.CombineColor(C, M, Y);

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
        Debug.Log($"Answer : {Answer.answer}");
        ParrotDataManager.Instance.ParrotCollect(this.Answer.answer);
        is_corrected[this.Answer.answer] = true;
    }

    public List<List<int>> GetAllCorrectedParrotData()
    {
        List<List<int>> result = new List<List<int>>();
        int Size = ParrotDataManager.Instance.ParrotSheet.GetLength(0);

        for(int i = 0; i < Size; i++)
        {
            if(is_corrected[i] == true)
            {
                result.Add(ParrotDataManager.Instance.GetParrotBodyDataIntoInt(i));
            }
        }

        return result;
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