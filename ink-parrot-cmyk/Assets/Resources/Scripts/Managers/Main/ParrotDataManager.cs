using UnityEngine;
using System.Collections.Generic;
public class ParrotDataManager : MonoBehaviour
{
    static public ParrotDataManager Instance;
    public GameObject AnswerImageUI;
    public ParrotSheetType[] ParrotSheet;
    ParrotsCorrectedVariable parrots_collected_data = new ParrotsCorrectedVariable();
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        List<List<string>> basic_parrot_data = DataManager.Instance.LoadCSV("Data/Parrot Data");

        ParrotsVariable custom_parrot_data = new ParrotsVariable();

        //커스텀 앵무새 정보 불러오기
        if(!DataManager.Instance.tryLoadJson<ParrotsVariable>("custom-parrots.json", out custom_parrot_data)){
            Debug.Log("세이브된 커스텀 앵무새 데이터가 없어 새로이 생성합니다.");
            custom_parrot_data = new ParrotsVariable();
        }
        else
        {
            Debug.Log("세이브된 커스텀 앵무새 데이터가 있어 불러옵니다.");
        }

        //앵무새 성공 정보 불러오기
        if(!DataManager.Instance.tryLoadJson<ParrotsCorrectedVariable>("parrots-corrected.json", out parrots_collected_data)){
            Debug.Log("세이브된 앵무새 정답 여부 정보 데이터가 없어 새로이 생성합니다.");
            parrots_collected_data = new ParrotsCorrectedVariable();
        }
        else
        {
            Debug.Log("세이브된 앵무새 정답 여부 정보 데이터가 있어 불러옵니다.");
        }

        basic_parrot_data.AddRange(custom_parrot_data.parrot_data);
        DataProcess(basic_parrot_data);
    }

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
                ParrotSheet[i - 1].bodyTemplates[t].x = int.Parse(data[i][t * 3 + 2]);
                ParrotSheet[i - 1].bodyTemplates[t].y = int.Parse(data[i][t * 3 + 3]);
                ParrotSheet[i - 1].bodyTemplates[t].z = int.Parse(data[i][t * 3 + 4]);
                //Debug.Log($"Name : {ParrotSheet[i - 1].name}\nTemplate : {t}\nC : {ParrotSheet[i - 1].bodyTemplates[t].x}\nM : {ParrotSheet[i - 1].bodyTemplates[t].y}\nY : {ParrotSheet[i - 1].bodyTemplates[t].z}");
            }

            ParrotSheet[i - 1].score = int.Parse(data[i][Constants.TemplateSize * 3 + 2]);
            ParrotSheet[i - 1].difficulty = data[i][Constants.TemplateSize * 3 + 3];
            ParrotSheet[i - 1].isCompleted = false;
        }
    }

    public void ParrotCollect(int index)
    {
        if(index > parrots_collected_data.have_corrected.Count)
            return ;

        parrots_collected_data.have_corrected[index] = true;
    }
}

[System.Serializable]
public class ParrotsVariable
{
    public List<List<string>> parrot_data = new List<List<string>>();
}

public class ParrotsCorrectedVariable
{
    public List<bool> have_corrected = new List<bool>();

    public ParrotsCorrectedVariable()
    {
        for(int i = 0; i < Constants.BasicParrotsSize; i++)
            this.have_corrected.Add(false);
    }
}

public class ParrotSheetType
{
    public string name;
    public Vector3[] bodyTemplates = new Vector3[Constants.TemplateSize];
    public int score;
    public string difficulty;
    public bool isCompleted;
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