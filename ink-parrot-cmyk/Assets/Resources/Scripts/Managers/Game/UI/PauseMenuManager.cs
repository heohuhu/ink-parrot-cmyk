using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenuManager : MonoBehaviour
{
    static public PauseMenuManager Instance;
    public GameObject TutorialPanel;
    public GameObject ResumePanel;
    public TextMeshProUGUI ResumeCount;
    void Awake()
    {
        Instance = this;
        TutorialPanel.SetActive(false);
        ResumePanel.SetActive(false);
    }
    public void OpenTutorial()
    {
        
    }

    public void CloseTutorial()
    {
        
    }

    public void ResumeUILoad()
    {
        ResumePanel.SetActive(true);
    }
    
    public void ResumeUIUnload() 
    {
        ResumePanel.SetActive(false);
        Resume();
        GameUiManager.Instance.PauseMenuClose();
    }

    void Resume()
    {
        GameManager.Instance.ResumeTime();
    }
}
