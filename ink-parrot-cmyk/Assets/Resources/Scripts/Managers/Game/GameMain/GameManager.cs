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
        Ranking = DataManager.Instance.loadJson<List<Ranking_Type>>("ranking.json");

        if(Ranking == null || Ranking.Count == 0)
        {
            Ranking = new List<Ranking_Type>();
            Ranking.Add(new Ranking_Type(-1, "null"));
            Ranking.Add(new Ranking_Type(-1, "null"));
            Ranking.Add(new Ranking_Type(-1, "null"));
        }

        if (SettingManager.Instance.setting.isTutorial)
        {
            isTutorial = true;
            TutorialManager.Instance.TutorialStart(0);
        }else {
            Timer.Instance.TimerStart();
            AnswerSheet.Instance.GiveProblem(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool isTimeStopped = false;
    public bool isTutorial = false;
    public static float GetdeltaTime => Instance.isTimeStopped == true ? 0f : Time.deltaTime;
    public void StopTime()
    {
        isTimeStopped = true;
        Timer.Instance.Pause();
        BackgroundManager.Instance.SetBackgroundStop(true);
        AudioManager.Instance.PauseALLBGM();
    }

    public void ResumeTime()
    {
        Timer.Instance.Resume();
        isTimeStopped = false;
        BackgroundManager.Instance.SetBackgroundStop(false);
        AudioManager.Instance.ResumeALLBGM();
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
        if(isTimeStopped)
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
            Timer.Instance.Pause();
            parrots[i].ChangeMaterial(1);
            yield return parrots[i].ObjectAssembled();
        }

        AnswerSheet.Instance.AnswerSubmit();
        yield return new WaitForSeconds(0.5f);
        
        for(int i = 0; i < 3; i++)
        {
            yield return parrots[i].ObjectPositionInit();
        }
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
            Timer.Instance.Resume();
            AnswerSheet.Instance.GiveProblem();
        }
        processing = 0;
        isAssembling = false;
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
                return;
            }else
                targetParent_Object.SetActive(true);
            Ranking_Update(ScoreManager.Instance.score);
        }
    }

    public void StartCompletedParrotsShow()
    {
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
    private List<Ranking_Type> Ranking = new List<Ranking_Type>();

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