using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject tutorial_ui;
    public GameObject setting_ui;
    public GameObject collection_ui;
    void Start()
    {
        
    }

    public void GameStart()
    {
        SceneController.Instance.LoadSceneAdditiveAsActive("Game");
        SceneController.Instance.UnloadScene("StartMenu");
    }

    public void CustomStart()
    {
        SceneController.Instance.LoadSceneAdditiveAsActive("CustomPage");
        SceneController.Instance.UnloadScene("StartMenu");
    }

    public void ShowTutorial()
    {
        tutorial_ui.SetActive(true);
    }

    public void UnShowTutorial()
    {
        tutorial_ui.SetActive(false);
    }

    public void ShowCollection()
    {
        collection_ui.SetActive(true);
    }

    public void unShowCollection()
    {
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
}
