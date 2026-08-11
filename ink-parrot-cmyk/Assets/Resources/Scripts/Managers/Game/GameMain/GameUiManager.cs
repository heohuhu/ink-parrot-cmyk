using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Timeline;
public class GameUiManager : MonoBehaviour
{
    public static GameUiManager Instance;
    public GameObject PauseMenu;
    public GameObject ReturnMenu;
    public GameObject RealReturnMenu;
    public GameObject ParrotsRenderImage;
    private bool isWindowOpened = false;
    private float rotationSpeed = 115f;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(NewParrotPanel.activeInHierarchy){
            NewParrotEffect[0].GetComponent<RectTransform>().Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
            NewParrotEffect[1].GetComponent<RectTransform>().Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    public void DisableEveryThing()
    {
        HighlightManager.Instance.RegisterFromParent(GameMainScene);
        HighlightManager.Instance.Activate();
        HighlightManager.Instance.Setting();
        HighlightManager.Instance.EnableFromParent(GameMainScene);
        HighlightManager.Instance.AddException(ParrotsRenderImage);
    }
    public void PauseMenuOpen()
    {
        if(isWindowOpened)
            return;
        isWindowOpened = true;
        PauseMenu.SetActive(true);
        PauseMenuManager.Instance.OpenMenu();
    }

    public void PauseMenuClose()
    {
        PauseMenu.SetActive(false);
        isWindowOpened = false;
    }

    public void ReturnMenuOpen()
    {
        if(isWindowOpened)
            return;
        isWindowOpened = true;
        ReturnMenu.SetActive(true);
    }

    public void ReturnMenuClose()
    {
        RealReturnMenu.SetActive(false);
        ReturnMenu.SetActive(false);
        isWindowOpened = false;
    }

    public void ReturnButtonClicked()
    {
        if(ScoreManager.Instance.score > 0)
        {
            ReturnMenu.SetActive(false);
            RealReturnMenu.SetActive(true);
        }else
            GameManager.Instance.ReturnStartMenu();
    }


    public GameObject SelectedPanel, GamePanel;
    public void SelectColor(int ColorType)
    {
        GamePanel.SetActive(false);
        SelectedPanel.SetActive(true);
    }

    public void UnSelectColor()
    {
        extractButton.SetActive(false);
        refillButton.SetActive(false);
        lightManagingSlider.SetActive(false);
        SelectedPanel.SetActive(false);
        GamePanel.SetActive(true);
    }

    public GameObject extractButton, lightManagingSlider, refillButton;

    public void SelectTemplate(int value)
    {
        if(value == 0){
            refillButton.SetActive(true);
            extractButton.SetActive(false);
        }else{
            extractButton.SetActive(true);
            refillButton.SetActive(false);
        }
    }

    public void SetLightManagingSlider(int value)
    {
        if(value == 0){
            lightManagingSlider.SetActive(false);
            return ;
        }
        lightManagingSlider.SetActive(true);
        lightManagingSlider.GetComponent<Slider>().SetValueWithoutNotify(value);
    }

    public GameObject GameMainScene, GameEndScene;
    public GameObject GameOverPanel;
    
    public GameObject GameEndScore, RankingPanel, NewParrotPanel;
    public GameObject[] RankingText = new GameObject[3];
    private bool isEndNext = false;
    public IEnumerator GameEndProcess()
    {
        isEndNext = false;
        GameOverPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        isEndNext = true;
    }

    public IEnumerator RankingPanelOn()
    {
        isEndNext = false;
        RankingPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        isEndNext = true;
    }

    public void RankingShow(List<Ranking_Type> rankings)
    {
        for(int i = 0; i < 3; i++)
        {
            if(rankings[i].score == -1)
            {
                RankingText[i].GetComponent<TextMeshProUGUI>().text = (i + 1).ToString() + "위: -점(--/--)";
            }
            else
            {
                RankingText[i].GetComponent<TextMeshProUGUI>().text = (i + 1).ToString() + "위: " + rankings[i].score + "점(" + rankings[i].date+")";
            }
        }
    }

    public void RankingPanelClose()
    {
        if (isEndNext)
        {
            RankingPanel.SetActive(false);
            GameManager.Instance.StartCompletedParrotsShow();
        }
    }

    public void GameEndNext()
    {
        if (isEndNext)
        {
            GameMainScene.SetActive(false);
            GameEndScene.SetActive(true);
            GameManager.Instance.GameEndNext(0);
            GameEndScore.GetComponent<TextMeshProUGUI>().text = "스코어: " + ScoreManager.Instance.score.ToString() + "점";
        }
    }

    public GameObject [] NewParrotEffect = new GameObject [2];

    public void NewParrotPanelOpen()
    {
        NewParrotPanel.SetActive(true);
    }

    public void NewParrotPanelClose()
    {
        NewParrotPanel.SetActive(false);
        GameManager.Instance.GameEndNext(1);
    }

    public GameObject [] TemplateButton = new GameObject [Constants.TemplateSize];

    public void TemplateButtonOutlineSetting()
    {
        float x = 5f, y = 5f;
        for(int i = 0; i < Constants.TemplateSize; i++)
        {
            Outline tmp = TemplateButton[i].GetComponent<Outline>();
            tmp.effectDistance = new Vector2(x, y);
            tmp.enabled = false;
        }
    }

    public void TemplateButtonOutlineEnable(int index)
    {
        TemplateButtonOutlineSetting();
        Outline tmp = TemplateButton[index].GetComponent<Outline>();
        tmp.enabled = true;
    }
}
