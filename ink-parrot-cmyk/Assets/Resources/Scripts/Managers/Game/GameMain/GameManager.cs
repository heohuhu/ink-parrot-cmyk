using NUnit.Framework;
//using Unity.Android.Gradle.Manifest;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
        for(int i = 0; i < 3; i++)
            this.parrots[i] = parrots_objects[i].GetComponent<ParrotTemplate>();
        this.isGameEnd = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AnswerSheet.Instance.GiveProblem();
        Ranking = DataManager.Instance.loadJson<List<Ranking_Type>>("ranking.json");

        if(Ranking == null || Ranking.Count == 0)
        {
            Ranking = new List<Ranking_Type>();
            Ranking.Add(new Ranking_Type(-1, "null"));
            Ranking.Add(new Ranking_Type(-1, "null"));
            Ranking.Add(new Ranking_Type(-1, "null"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool isTimeStopped = false;
    public static float GetdeltaTime => Instance.isTimeStopped == true ? 0f : Time.deltaTime;
    public void StopTime()
    {
        isTimeStopped = true;
        Timer.Instance.Pause();
    }

    public void ResumeTime()
    {
        Timer.Instance.Resume();
        isTimeStopped = false;
    }

    public void ReturnStartMenu()
    {
        SceneController.Instance.LoadSceneAdditiveAsActive("StartMenu");
        SceneController.Instance.UnloadScene("Game");
    }

    //-1 : 없는 상태, 0 : Cyan, 1 : Magenta, 2 : Yellow
    ParrotTemplate [] parrots = new ParrotTemplate[3];
    public GameObject [] parrots_objects = new GameObject[3];
    public int selectedColor = -1;
    int selectedTemplate = -1;
    float squeezing = 100f;
    public int processing = 0;
    public void SelectColor(int ColorType)
    {
        if(processing == 1)
            return;
        processing = 1;
        selectedTemplate = -1;
        selectedColor = ColorType;
        StartCoroutine(parrots[ColorType].ObjectSelected());
        GameUiManager.Instance.SelectColor(ColorType);
    }

    public void unSelectColor()
    {
        if(processing == 1)
            return;
        processing = 1;
        StartCoroutine(parrots[selectedColor].ObjectUnSelected());
        GameUiManager.Instance.UnSelectColor();
        selectedColor = -1;
        selectedTemplate = -1;
    }

    public void SelectTemplate(int Template)
    {
        selectedTemplate = Template;
        squeezing = (this.parrots[selectedColor].BodyTemplatesInk[selectedTemplate] == 0 ? 0f : 100f);
        GameUiManager.Instance.SelectTemplate(parrots[selectedColor].BodyTemplatesInk[selectedTemplate]);
        GameUiManager.Instance.SetLightManagingSlider(this.parrots[selectedColor].BodyTemplatesInk[selectedTemplate]);
    }
    public void SqueezeColor()
    {
        if(selectedTemplate == -1)
            return;
        if(squeezing > 0f)
            squeezing -= 0.1f;
        else if(squeezing <= 0f)
        {
            SqueezedColor();
        }
    }
    public void SqueezedColor()
    {
        this.parrots[selectedColor].TemplateExtracted(selectedTemplate);
        GameUiManager.Instance.SetLightManagingSlider(this.parrots[selectedColor].BodyTemplatesInk[selectedTemplate]);
        GameUiManager.Instance.SelectTemplate(parrots[selectedColor].BodyTemplatesInk[selectedTemplate]);
    }

    public void LightManaging(float num)
    {
        if(selectedTemplate == -1)
            return;
        
        this.parrots[selectedColor].BodyTemplatesInk[selectedTemplate] = (int)num;
        this.parrots[selectedColor].DrawColor(selectedTemplate);
    }

    public void InputDetected(int color)
    {
        if(isTimeStopped)
            return ;
        if(selectedColor == -1)
            SelectColor(color);
    }

    public void RefillButtonClicked()
    {
        if(selectedColor != -1 && selectedTemplate != -1){
            this.parrots[selectedColor].Resetting(selectedTemplate);
            GameUiManager.Instance.SetLightManagingSlider(this.parrots[selectedColor].BodyTemplatesInk[selectedTemplate]);
            GameUiManager.Instance.SelectTemplate(parrots[selectedColor].BodyTemplatesInk[selectedTemplate]);
        }
    }

    public int GetCurrentScore()
    {
        int [] C = new int[Constants.TemplateSize], M = new int[Constants.TemplateSize], Y = new int[Constants.TemplateSize];

        for(int i = 0; i < Constants.TemplateSize; i++)
        {
            C[i] = parrots[(int)Constants.ColorType.Cyan].BodyTemplatesInk[i];
            M[i] = parrots[(int)Constants.ColorType.Magenta].BodyTemplatesInk[i];
            Y[i] = parrots[(int)Constants.ColorType.Yellow].BodyTemplatesInk[i];
        }

        return AnswerSheet.Instance.CompareAnswer(C, M, Y);
    }
    public void AnswerSubmit()
    {
        if(AnswerSheet.Instance.Answer.answerType == 0){
            AnswerSheet.Instance.AnswerSubmit();
        }else if(AnswerSheet.Instance.Answer.answerType == 1)
        {
            AnswerSheet.Instance.AnswerSubmit();
        }

        parrots[0].Init();
        parrots[1].Init();
        parrots[2].Init();
    }


    public bool isGameEnd = false;
    //시간 초과 발생
    public void GameEnd()
    {
        if(this.isGameEnd)
            return ;
        this.isGameEnd = true;
        StartCoroutine(GameUiManager.Instance.GameEndProcess());
    }

    //진짜 게임 엔드 처리
    public void GameEndNext()
    {
        if(!this.isGameEnd)
            return;

        List<List<int>> parrotData = AnswerSheet.Instance.GetAllCorrectedParrotData();
        AnswerParrots = new List<GameObject>();

        for(int i = 0; i < parrotData.Count; i++)
        {
            GameObject newObject = Instantiate(GameEndParrotTemplate, targetParent);
            ParrotTemplateContent code = newObject.GetComponent<ParrotTemplateContent>();
            code.SetUp(parrotData[i]);
            newObject.SetActive(true);
        }

        Ranking_Update(ScoreManager.Instance.score);
    }

    public void ReStartGame()
    {
        SceneController.Instance.ReloadScene("Game");
    }

    public GameObject GameEndParrotTemplate;
    public Transform targetParent;
    public List<GameObject> AnswerParrots;
    private List<Ranking_Type> Ranking;

    public void Ranking_Update(int score)
    {
        bool isRankingUpdated = false;
        for(int i = 0; i < Ranking.Count; i++)
        {
            if(score > Ranking[i].score)
            {
                Ranking.Insert(i, new Ranking_Type(score, Utility.GetCurrentDateMMDD()));

                if(Ranking.Count > 3)
                    Ranking.RemoveAt(3);

                isRankingUpdated = true;
                break;
            }
        }

        if (isRankingUpdated)
        {
            StartCoroutine(GameUiManager.Instance.RankingPanelOn());
            GameUiManager.Instance.RankingShow(this.Ranking);
            DataManager.Instance.saveJson<List<Ranking_Type>>("ranking.json", Ranking);
        }
    }
}

public class Ranking_Type
{
    public int score;
    public string date;

    public Ranking_Type(int score, string date)
    {
        this.score = score;
        this.date = date;
    }
}