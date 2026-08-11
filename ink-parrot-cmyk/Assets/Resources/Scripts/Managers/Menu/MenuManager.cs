using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    static public MenuManager Instance;
    public GameObject menu_ui;
    public GameObject tutorial_ui;
    public GameObject [] tutorials = new GameObject[2];
    public GameObject setting_ui;
    public GameObject [] settings = new GameObject[2];
    public GameObject collection_ui;
    public GameObject notification_panel;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        menu_ui.SetActive(true);
        tutorial_ui.SetActive(false);
        setting_ui.SetActive(false);
        collection_ui.SetActive(false);
    }

    public void GameStart()
    {
        SceneController.Instance.LoadSceneAdditiveAsActive("Game");
        SceneController.Instance.UnloadScene("StartMenu");

        menu_ui.SetActive(true);
    }

    public void CustomStart()
    {
        if (SettingManager.Instance.setting.isTutorial)
        {
            PrintNotification("먼저 메인 게임 튜토리얼을 완료해주세요.");
            return;
        }
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGMPlaylist(
        new List<string>()
        {
            "커스텀백그라운드"
        });
        SceneController.Instance.LoadSceneAdditiveAsActive("CustomPage");
        SceneController.Instance.UnloadScene("StartMenu");
    }

    public void ShowTutorial()
    {
        tutorials[0].SetActive(true);
        tutorials[1].SetActive(false);
        tutorial_ui.SetActive(true);
    }

    public void UnShowTutorial()
    {
        tutorial_ui.SetActive(false);
    }

    public void TutorialSwitch(int target)
    {
        tutorials[target].SetActive(true);
        tutorials[target == 0 ? 1 : 0].SetActive(false);
    }

    public void ShowCollection()
    {
        collection_ui.SetActive(true);
        menu_ui.SetActive(false);
        CollectionManager.Instance.Reset();
        CollectionManager.Instance.CollectionOn();
    }

    public void unShowCollection()
    {
        menu_ui.SetActive(true);
        collection_ui.SetActive(false);
    }

    public void ShowSetting()
    {
        setting_ui.SetActive(true);
    }

    public void unShowSetting()
    {
        setting_ui.SetActive(false);
        SettingManager.Instance.SettingSave();
        Debug.Log($"Master {SettingManager.Instance.setting.sound.Master}\nBGM {SettingManager.Instance.setting.sound.BGM}\nSFX {SettingManager.Instance.setting.sound.SFX}\nUI {SettingManager.Instance.setting.sound.UI}\nColorStrength {SettingManager.Instance.setting.graphicOption.ColorStrength}");
    }

    public void ShowSwitching(int index)
    {
        
    }

    public void GameReseting()
    {
        DataManager.Instance.ResetGameData();
        SceneController.Instance.GameRestart();
    }

    public void PrintNotification(string text)
    {
        notification_panel.SetActive(true);
        notification_panel.GetComponentInChildren<TextMeshProUGUI>().text = text;
        notification_panel.GetComponent<FadeShower>().Play();
    }

    public void PrintNotification(string text, float time)
    {
        notification_panel.SetActive(true);
        notification_panel.GetComponentInChildren<TextMeshProUGUI>().text = text;
        notification_panel.GetComponent<FadeShower>().Play(time);
    }
}
