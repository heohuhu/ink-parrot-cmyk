using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnswerSheet : MonoBehaviour
{
    static public AnswerSheet Instance;
    public GameObject AnswerImageUI;
    
    void Awake()
    {
        Instance = this;
        //기본 앵무새 정보 불러오기
        List<List<string>> basic_parrot_data = DataManager.Instance.LoadCSV("Data/Parrot Data");

        ParrotsVariable custom_parrot_data = new ParrotsVariable();
        //커스텀 앵무새 정보 불러오기
        if(!DataManager.Instance.tryLoadJson<ParrotsVariable>("custom-parrots.json", out custom_parrot_data)){
            Debug.Log("세이브된 설정 데이터가 없어 새로이 생성합니다.");
            custom_parrot_data = new ParrotsVariable();
        }
        else
        {
            Debug.Log("세이브된 설정 데이터가 있어 불러옵니다.");
        }

        basic_parrot_data.AddRange(custom_parrot_data.parrot_data);
        DataProcess(basic_parrot_data);
    }

    public Sprite[] AnswerImages;
    public ParrotSheetType[] ParrotSheet;
    public AnswerType Answer = new AnswerType();

    public void DataProcess(List<List<string>> data)
    {
        int rowCount = data.Count;
        ParrotSheet = new ParrotSheetType [rowCount - 1];
        int colCount = data[0].Count;
        for(int i = 1; i < rowCount; i++) //idx 날림
        {
            ParrotSheet[i - 1] = new ParrotSheetType();
            ParrotSheet[i - 1].name = data[i][1];
            
            for(int t = 0; t < Constants.TemplateSize; t++)
            {
                ParrotSheet[i - 1].bodyTemplates[t] = Decryption(int.Parse(data[i][t + 2]));
            }

            ParrotSheet[i - 1].score = int.Parse(data[i][2 + Constants.TemplateSize]);
        }
    }

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

    public void MakeAnswer()
    {
        int size = this.ParrotSheet.GetLength(0);

        int randIndex = Utility.GetRandomInt(0, size);
        Answer.SetAnswer(this.ParrotSheet[randIndex].bodyTemplates);
        Answer.answer = randIndex;
        Answer.answerType = 0;

        Debug.Log($"현재 정답 : {this.ParrotSheet[Answer.answer].name}");
        ShowAnswerImage(Answer.answer);
    }

    public void ShowAnswerImage(int index)
    {
        AnswerImageUI.GetComponent<Image>().sprite = this.AnswerImages[index];
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
}

public class ParrotSheetType
{
    public string name;
    public Vector3[] bodyTemplates = new Vector3[Constants.TemplateSize];
    public int score;
    public ParrotSheetType(Vector3 [] bodyTemplates)
    {
        this.bodyTemplates = bodyTemplates;
    }

    public ParrotSheetType()
    {
        name = "";
        score = 0;
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

        C = M = Y = new int[size];

        for(int i = 0; i < size; i++)
        {
            C[i] = (int)answer[i].x;
            M[i] = (int)answer[i].y;
            Y[i] = (int)answer[i].z;
        }
    }
}

[System.Serializable]
public class ParrotsVariable
{
    public List<List<string>> parrot_data;
}