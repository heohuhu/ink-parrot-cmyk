using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class GameUiManager : MonoBehaviour
{
    public static GameUiManager Instance;
    public GameObject PauseMenu;
    public GameObject ReturnMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void PauseMenuOpen()
    {
        PauseMenu.SetActive(true);
        PauseMenuManager.Instance.OpenMenu();
    }

    public void PauseMenuClose()
    {
        PauseMenu.SetActive(false);
    }

    public void ReturnMenuOpen()
    {
        ReturnMenu.SetActive(true);
    }

    public void ReturnMenuClose()
    {
        ReturnMenu.SetActive(false);
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
    public GameObject GameEndScore, RankingPanel;
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
        }
    }

    public void GameEndNext()
    {
        if (isEndNext)
        {
            GameMainScene.SetActive(false);
            GameEndScene.SetActive(true);
            GameManager.Instance.GameEndNext();
            GameEndScore.GetComponent<TextMeshProUGUI>().text = "스코어: " + ScoreManager.Instance.score.ToString() + "점";
        }
    }
}
