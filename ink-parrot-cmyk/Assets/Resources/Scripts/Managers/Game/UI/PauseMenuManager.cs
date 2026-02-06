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
    }

    public void OpenTutorial()
    {
        TutorialPanel.SetActive(true);
        GameManager.Instance.StopTime();
        MenuPanel.SetActive(false);
    }

    public void CloseTutorial()
    {
        TutorialPanel.SetActive(false);
        ResumeUILoad();
    }

    public void ResumeUILoad()
    {
        ResumePanel.SetActive(true);
        TutorialPanel.SetActive(false);
        MenuPanel.SetActive(false);
        GameManager.Instance.StopTime();
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
