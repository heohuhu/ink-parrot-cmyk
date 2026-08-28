using NUnit.Framework;
//using Unity.Android.Gradle.Manifest;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

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
        Ranking = DataManager.Instance.loadJson<Ranking_Type_List>("ranking.json");

        if(Ranking == null || Ranking.ranking.Count == 0)
        {
            Ranking = new Ranking_Type_List();
            Ranking.ranking.Add(new Ranking_Type(-1, "null"));
            Ranking.ranking.Add(new Ranking_Type(-1, "null"));
            Ranking.ranking.Add(new Ranking_Type(-1, "null"));
        }

        if (SettingManager.Instance.setting.isTutorial)
        {
            isTutorial = true;
            TutorialManager.Instance.TutorialStart(0);
        }else {
            Timer.Instance.TimerStart();
            AnswerSheet.Instance.GiveProblem(1);
        }

        GameUiManager.Instance.Setting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool isTimeStopped = false;
    public bool isGameTimeStopped = false; // Time.timeScale 처럼 동작할 예정
    public bool isTutorial = false;
    public static float GetdeltaTime => Instance.isGameTimeStopped == true ? 0f : Time.deltaTime;
    public void StopTime()
    {
        isTimeStopped = true;
        Timer.Instance.Pause();
    }

    public void StopGameTime()
    {
        isGameTimeStopped = true;
        SceneController.Instance.isGameTimeStopped = isGameTimeStopped;
        Timer.Instance.Pause();
        BackgroundManager.Instance.SetBackgroundStop(true);
        AudioManager.Instance.PauseALLBGM();
    }

    public void ResumeTime()
    {
        isTimeStopped = false;
        
        if(isTimeStopped == false && isGameTimeStopped == false)
            Timer.Instance.Resume();
    }

    public void ResumeGameTime()
    {
        isGameTimeStopped = false;
        SceneController.Instance.isGameTimeStopped = isGameTimeStopped;

        if(isTimeStopped == false && isGameTimeStopped == false)
            Timer.Instance.Resume();

        BackgroundManager.Instance.SetBackgroundStop(false);
        AudioManager.Instance.ResumeALLBGM();
    }

    public IEnumerator WaitForGameTime(float seconds)
    {
        float elapsedTime = 0f;

        while (elapsedTime < seconds)
        {
            if (!isGameTimeStopped)
            {
                elapsedTime += Time.deltaTime;
            }

            yield return null;
        }
    }

    public void ReturnStartMenu()
    {
        if(gameendCoroutine != null)
            StopCoroutine(gameendCoroutine);
        AudioManager.Instance.ResumeALLBGM();
        SceneController.Instance.LoadSceneAdditiveAsActive("StartMenu");
        SceneController.Instance.UnloadScene("Game");
    }

    //-1 : 없는 상태, 0 : Cyan, 1 : Magenta, 2 : Yellow
    ParrotTemplate [] parrots = new ParrotTemplate[3];
    public GameObject [] parrots_objects = new GameObject[3];
    public GameObject answerParrotFlyerGIFObject;
    public int selectedColor = -1;
    int selectedTemplate = -1;
    float squeezing = 100f;
    public int processing = 0;
    public void SelectColor(int ColorType)
    {
        if (isTutorial)
        {
            if(TutorialManager.Instance.current_question.StartsWith("Color"))
            {
                int target = int.Parse(TutorialManager.Instance.current_question.Split("-")[1]);

                if(ColorType != target)
                    return ;
                TutorialManager.Instance.current_question = "none";
                TutorialManager.Instance.is_event_fulfilled = true;
                TutorialManager.Instance.NextDialogue();
            }else
                return ;
        }
        if(processing == 1)
            return;
        AnswerSheet.Instance.isTouched = true;
        processing = 1;
        selectedTemplate = -1;
        selectedColor = ColorType;
        StartCoroutine(parrots[ColorType].ObjectSelected());
        GameUiManager.Instance.SelectColor(ColorType);
        GameUiManager.Instance.TemplateButtonOutlineSetting();
    }

    public void unSelectColor()
    {
        if (isTutorial)
        {
            if(TutorialManager.Instance.current_question == "tmp")
            {
            }else
                return ;
        }
        if(processing == 1)
            return;
        processing = 1;
        parrots[selectedColor].TemplateUnselected();
        StartCoroutine(parrots[selectedColor].ObjectUnSelected());
        GameUiManager.Instance.UnSelectColor();
        selectedColor = -1;
        selectedTemplate = -1;
    }

    public void SelectTemplate(int Template)
    {
        if (isTutorial)
        {
            if(TutorialManager.Instance.current_question.StartsWith("Template"))
            {
                int target = int.Parse(TutorialManager.Instance.current_question.Split("-")[1]);
                if(Template != target)
                    return;

                TutorialManager.Instance.current_question = "none";
                TutorialManager.Instance.is_event_fulfilled = true;
                TutorialManager.Instance.NextDialogue();
            }else if(TutorialManager.Instance.current_question.StartsWith("Answer"))
            {
                int target = int.Parse(TutorialManager.Instance.current_question.Split("-")[1]);

                if(Template != target)
                    return;
            }else
                return ;
        }
        selectedTemplate = Template;
        squeezing = (this.parrots[selectedColor].BodyTemplatesInk[selectedTemplate] == 0 ? 0f : 100f);
        parrots[selectedColor].TemplateUnselected();
        parrots[selectedColor].TemplateSelected(selectedTemplate);
        
        GameUiManager.Instance.TemplateButtonOutlineEnable(selectedTemplate);
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
        if(selectedTemplate == -1)
            return;

        if (isTutorial)
        {
            if(TutorialManager.Instance.current_question.StartsWith("색상조작"))
            {
                string[] temp = TutorialManager.Instance.current_question.Split("-");

                temp[2] = "1";

                TutorialManager.Instance.current_question = string.Join("-", temp);

                if(TutorialManager.Instance.current_question == "색상조작-1-1"){
                    TutorialManager.Instance.current_question = "none";
                    TutorialManager.Instance.is_event_fulfilled = true;
                    TutorialManager.Instance.NextDialogue();
                }
            } else if(TutorialManager.Instance.current_question.StartsWith("Template")) {}
            else if(TutorialManager.Instance.current_question.StartsWith("Answer"))
            {
                int target = int.Parse(TutorialManager.Instance.current_question.Split("-")[1]);
                if(selectedTemplate != target)
                    return;

                int target_value = int.Parse(TutorialManager.Instance.current_question.Split("-")[2]);
                
                if(0 == target_value){
                    TutorialManager.Instance.current_question = "none";
                    TutorialManager.Instance.is_event_fulfilled = true;
                    TutorialManager.Instance.NextDialogue();
                }
            }else
                return;
        }

        this.parrots[selectedColor].TemplateExtracted(selectedTemplate);
        GameUiManager.Instance.SetLightManagingSlider(this.parrots[selectedColor].BodyTemplatesInk[selectedTemplate]);
        GameUiManager.Instance.SelectTemplate(parrots[selectedColor].BodyTemplatesInk[selectedTemplate]);
    }

    public void LightManaging(float num)
    {
        if(selectedTemplate == -1)
            return;

        if (isTutorial)
        {
            if(TutorialManager.Instance.current_question.StartsWith("색상조작"))
            {
                string[] temp = TutorialManager.Instance.current_question.Split("-");

                temp[1] = "1";

                TutorialManager.Instance.current_question = string.Join("-", temp);

                if(TutorialManager.Instance.current_question == "색상조작-1-1"){
                    TutorialManager.Instance.current_question = "none";
                    TutorialManager.Instance.is_event_fulfilled = true;
                    TutorialManager.Instance.NextDialogue();
                }
            }else if(TutorialManager.Instance.current_question.StartsWith("Answer"))
            {
                int target = int.Parse(TutorialManager.Instance.current_question.Split("-")[1]);
                if(selectedTemplate != target)
                    return;

                int target_value = int.Parse(TutorialManager.Instance.current_question.Split("-")[2]);
                
                if((int)num == target_value){
                    TutorialManager.Instance.current_question = "none";
                    TutorialManager.Instance.is_event_fulfilled = true;
                    TutorialManager.Instance.NextDialogue();
                }
            }else
                return;
        }
        
        this.parrots[selectedColor].BodyTemplatesInk[selectedTemplate] = (int)num;
        this.parrots[selectedColor].DrawColor(selectedTemplate);
    }

    public void InputDetected(int color)
    {
        if(isTimeStopped || isGameTimeStopped)
            return ;
        if(selectedColor == -1)
            SelectColor(color);
    }

    public void RefillButtonClicked()
    {
        if(selectedColor == -1 || selectedTemplate == -1)
            return;

        if (isTutorial)
        {
            if(TutorialManager.Instance.current_question.StartsWith("색상조작"))
            {
                string[] temp = TutorialManager.Instance.current_question.Split("-");

                temp[2] = "1";

                TutorialManager.Instance.current_question = string.Join("-", temp);

                if(TutorialManager.Instance.current_question == "색상조작-1-1"){
                    TutorialManager.Instance.current_question = "none";
                    TutorialManager.Instance.is_event_fulfilled = true;
                    TutorialManager.Instance.NextDialogue();
                }
            }
            else if(TutorialManager.Instance.current_question.StartsWith("Answer"))
            {
                int target = int.Parse(TutorialManager.Instance.current_question.Split("-")[1]);
                if(selectedTemplate != target)
                    return;

                int target_value = int.Parse(TutorialManager.Instance.current_question.Split("-")[2]);
                
                if(3 == target_value){
                    TutorialManager.Instance.current_question = "none";
                    TutorialManager.Instance.is_event_fulfilled = true;
                    TutorialManager.Instance.NextDialogue();
                }
            }else
                return ;
        }

        this.parrots[selectedColor].Resetting(selectedTemplate);
        GameUiManager.Instance.SetLightManagingSlider(this.parrots[selectedColor].BodyTemplatesInk[selectedTemplate]);
        GameUiManager.Instance.SelectTemplate(parrots[selectedColor].BodyTemplatesInk[selectedTemplate]);
        
    }

    public int GetCurrentScore()
    {
        if (SettingManager.Instance.setting.debuger.isAnswerProcessing)
        {
            return Constants.TemplateSize;
        }
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
        if (isTutorial)
        {
            if(TutorialManager.Instance.current_question == "tmp")
            {
                
            }else
                return ;
        }
        if(AnswerSheet.Instance.Answer.answerType == 0){
            if(processing == 1)
                return;
            processing = 1;
            StartCoroutine(AnswerSubmitProcess());
        }else if(AnswerSheet.Instance.Answer.answerType == 1)
        {
            if(processing == 1)
                return;
            AnswerSheet.Instance.AnswerSubmit();
            parrots[0].Init();
            parrots[1].Init();
            parrots[2].Init();
        }
    }

    public bool isAssembling = false;
    private IEnumerator AnswerSubmitProcess()
    {
        isAssembling = true;
        
        for(int i = 0; i < 3; i++)
        {
            StopTime();
            parrots[i].ChangeMaterial(1);
            yield return parrots[i].ObjectAssembled();
        }

        AnswerSheet.Instance.AnswerSubmit();
        yield return WaitForGameTime(0.5f);

        for(int i = 0; i < 3; i++)
            parrots[i].hideParrot();

        //GIF Init & Play
        {
            AnswerParrotFlyerGIFShower gIFShower = answerParrotFlyerGIFObject.GetComponent<AnswerParrotFlyerGIFShower>();
            gIFShower.Setting();

            for(int i = 0; i < Constants.TemplateSize; i++)
            {
                Color C = parrots[(int)Constants.ColorType.Cyan].getTemplateColor(i);
                Color M = parrots[(int)Constants.ColorType.Magenta].getTemplateColor(i);
                Color Y = parrots[(int)Constants.ColorType.Yellow].getTemplateColor(i);

                Color result = Utility.CombineColor(C, M, Y);

                gIFShower.SetColor(i, result);
            }

            answerParrotFlyerGIFObject.SetActive(true);
            yield return gIFShower.ActivatingPlayOnce();
            answerParrotFlyerGIFObject.SetActive(false);
        }
        
        for(int i = 0; i < 3; i++)
        {
            yield return parrots[i].ObjectPositionInit();
        }

        yield return WaitForGameTime(0.8f);

        parrots[0].Init();
        parrots[1].Init();
        parrots[2].Init();
        parrots[0].ChangeMaterial(0);
        parrots[1].ChangeMaterial(0);
        parrots[2].ChangeMaterial(0);

        if (isTutorial)
        {
            TutorialManager.Instance.NextDialogue();
        }else{
            ResumeTime();
            AnswerSheet.Instance.GiveProblem();
        }
        processing = 0;
        isAssembling = false;
    }

    public bool isGameEnd = false;
    public Coroutine isGameEndProcessing = null;
    //시간 초과 발생
    public void GameEnd()
    {
        if(this.isGameEnd)
            return ;
        this.isGameEnd = true;
        isGameEndProcessing = StartCoroutine(GameUiManager.Instance.GameEndProcess());
    }

    //진짜 게임 엔드 처리
    Coroutine gameendCoroutine;
    public void GameEndNext(int index)
    {
        if(index == 0){
            if(!this.isGameEnd)
                return;

            AudioManager.Instance.PauseALLBGM();
            AudioManager.Instance.PlaySFX("게임종료뿅");

            if (AnswerSheet.Instance.isNewParrotCollected)
            {
                GameUiManager.Instance.NewParrotPanelOpen();
            }else
                index = 1;
        }
        
        if(index == 1)
        {
            if(ScoreManager.Instance.score == 0)
            {
                targetParent_Object.SetActive(false);
                GameUiManager.Instance.GameEndPanel.SetActive(true);
                return;
            }else
                targetParent_Object.SetActive(true);
            Ranking_Update(ScoreManager.Instance.score);
        }
    }

    public void StartCompletedParrotsShow()
    {
        GameUiManager.Instance.RealGameEndPanelOn();
        gameendCoroutine = StartCoroutine(CompletedParrotsShow());
    }

    private IEnumerator CompletedParrotsShow()
    {
        List<List<int>> parrotData = AnswerSheet.Instance.GetAllCorrectedParrotData();
        AnswerParrots = new List<GameObject>();

        for(int i = 0; i < parrotData.Count; i++)
        {
            GameObject newObject = Instantiate(GameEndParrotTemplate, targetParent);
            ParrotTemplateContent code = newObject.GetComponent<ParrotTemplateContent>();
            code.SetUp(parrotData[i]);
            newObject.SetActive(true);
            AudioManager.Instance.PlaySFX("휙");
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ReStartGame()
    {
        if(gameendCoroutine != null)
            StopCoroutine(gameendCoroutine);
        AudioManager.Instance.PlayBGMPlaylist(
        new List<string>()
        {
            "기본브금1",
            "기본브금2",
            "기본브금3"
        });
        SceneController.Instance.ReloadScene("Game");
    }

    public GameObject GameEndParrotTemplate;
    public Transform targetParent;
    public GameObject targetParent_Object;
    public List<GameObject> AnswerParrots;
    private Ranking_Type_List Ranking = new Ranking_Type_List();

    public void Ranking_Update(int score)
    {
        bool isRankingUpdated = false;
        for(int i = 0; i < Ranking.ranking.Count; i++)
        {
            if(score >= Ranking.ranking[i].score)
            {
                Ranking.ranking.Insert(i, new Ranking_Type(score, Utility.GetCurrentDateMMDD()));

                if(Ranking.ranking.Count > 3)
                    Ranking.ranking.RemoveAt(3);

                if(i == 0)
                    isRankingUpdated = true;
                break;
            }
        }

        if (isRankingUpdated)
        {
            GameUiManager.Instance.RankingHighlightPanelOpen();
            GameUiManager.Instance.RankingShow(this.Ranking.ranking);
            DataManager.Instance.saveJson<Ranking_Type_List>("ranking.json", Ranking);
        }
        else
        {
            StartCompletedParrotsShow();
        }
    }

    public void AnswerParrotAnswerShower()
    {
        GameUiManager.Instance.NewParrotPanelOpen();
    }
}

[System.Serializable]
public class Ranking_Type_List
{
    public List<Ranking_Type> ranking;

    public Ranking_Type_List()
    {
        ranking = new List<Ranking_Type>();
    }
}
[System.Serializable]
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