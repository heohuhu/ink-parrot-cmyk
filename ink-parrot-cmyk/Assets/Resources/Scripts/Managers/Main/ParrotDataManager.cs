using UnityEngine;
using System.Collections.Generic;
//using Unity.Android.Gradle.Manifest;
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
        ParrotSheetUpdate();
    }

    public void ParrotSheetUpdate()
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

        basic_parrot_data.AddRange(custom_parrot_data.GetListData());
        DataProcess(basic_parrot_data);
    }

    public void DataProcess(List<List<string>> data)
    {
        int rowCount = data.Count;
        ParrotSheet = new ParrotSheetType [rowCount - 1];
        for(int i = 1; i < rowCount; i++) //idx 날림
        {
            int colCount = data[i].Count;
            ParrotSheet[i - 1] = new ParrotSheetType();
            ParrotSheet[i - 1].name = data[i][1];
            
            for(int t = 0; t < Constants.TemplateSize; t++)
            {
                ParrotSheet[i - 1].bodyTemplates[t].x = int.Parse(data[i][t * 3 + 2]);
                ParrotSheet[i - 1].bodyTemplates[t].y = int.Parse(data[i][t * 3 + 3]);
                ParrotSheet[i - 1].bodyTemplates[t].z = int.Parse(data[i][t * 3 + 4]);
                //Debug.Log($"Name : {ParrotSheet[i - 1].name}\nTemplate : {t}\nC : {ParrotSheet[i - 1].bodyTemplates[t].x}\nM : {ParrotSheet[i - 1].bodyTemplates[t].y}\nY : {ParrotSheet[i - 1].bodyTemplates[t].z}");
            }

            if(colCount <= Constants.TemplateSize * 3 + 2)
            {
                ParrotSheet[i - 1].score = 30;
                ParrotSheet[i - 1].difficulty = "easy";
            }else{
                ParrotSheet[i - 1].score = int.Parse(data[i][Constants.TemplateSize * 3 + 2]);
                ParrotSheet[i - 1].difficulty = data[i][Constants.TemplateSize * 3 + 3];
            }
            ParrotSheet[i - 1].isCompleted = false;
        }
    }

    public void ParrotCollect(int index)
    {
        if(index >= parrots_collected_data.have_corrected.Count)
            return ;

        if(parrots_collected_data.have_corrected[index] == true)
            return ;

        parrots_collected_data.have_corrected[index] = true;
        Debug.Log($"{this.ParrotSheet[index].name} 앵무새를 처음으로 완성했습니다!");
        DataManager.Instance.saveJson<ParrotsCorrectedVariable>("parrots-corrected.json", parrots_collected_data);
    }

    public void NewCustomParrotAdd(ParrotInfo data)
    {
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

        custom_parrot_data.parrot_data.Add(data);

        DataManager.Instance.saveJson<ParrotsVariable>("custom-parrots.json", custom_parrot_data);

        ParrotSheetUpdate();
    }

    public List<int> GetParrotBodyDataIntoInt(int index)
    {
        List<int> tmp = new List<int>();

        if(getParrotCount() <= index)
            return tmp;

        for(int i = 0; i < Constants.TemplateSize; i++)
        {
            tmp.Add((int)ParrotSheet[index].bodyTemplates[i].x);
            tmp.Add((int)ParrotSheet[index].bodyTemplates[i].y);
            tmp.Add((int)ParrotSheet[index].bodyTemplates[i].z);
        }

        return tmp;
    }

    public string getParrotName(int index)
    {
        if(index >= getParrotCount())
            return null;
        return ParrotSheet[index].name;
    }

    public int getParrotCount()
    {
        return ParrotSheet.GetLength(0);
    }
}


[System.Serializable]
public class ParrotInfo
{
    public List<string> data = new List<string>();
}

[System.Serializable]
public class ParrotsVariable
{
    public List<ParrotInfo> parrot_data = new List<ParrotInfo>();

    public List<List<string>> GetListData()
    {
        List<List<string>> result = new List<List<string>>();

        foreach (ParrotInfo info in parrot_data)
        {
            result.Add(new List<string>(info.data));
        }

        return result;
    }
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