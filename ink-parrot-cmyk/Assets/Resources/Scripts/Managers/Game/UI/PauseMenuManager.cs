using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenuManager : MonoBehaviour
{
    static public PauseMenuManager Instance;
    public GameObject TutorialPanel;
    public GameObject MenuPanel;
    public GameObject ResumePanel;
    public GameObject ResumeCountdownPanel;
    void Awake()
    {
        Instance = this;
    }

    public void OpenMenu()
    {
        TutorialPanel.SetActive(false);
        ResumePanel.SetActive(false);
        MenuPanel.SetActive(true);
        ResumeCountdownPanel.SetActive(false);
        GameManager.Instance.StopTime();
    }

    public GameObject [] tutorials = new GameObject [2];
    public void ShowTutorial()
    {
        tutorials[0].SetActive(true);
        tutorials[1].SetActive(false);
        TutorialPanel.SetActive(true);
    }

    public void UnShowTutorial()
    {
        TutorialPanel.SetActive(false);
    }

    public void TutorialSwitch(int target)
    {
        tutorials[target].SetActive(true);
        tutorials[target == 0 ? 1 : 0].SetActive(false);
    }

    public void ResumeUILoad()
    {
        ResumePanel.SetActive(true);
        TutorialPanel.SetActive(false);
        MenuPanel.SetActive(false);
    }
    
    public void ResumeUIUnload() 
    {
        ResumePanel.SetActive(false);
        ResumeCountdownPanel.SetActive(true);
        ResumeManager.Instance.TimerStart();
    }

    public void Resume()
    {
        ResumeCountdownPanel.SetActive(false);
        GameManager.Instance.ResumeTime();
        GameUiManager.Instance.PauseMenuClose();
    }
}
