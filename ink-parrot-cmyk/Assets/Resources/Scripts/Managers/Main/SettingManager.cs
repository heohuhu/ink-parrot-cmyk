using UnityEngine;

//여러 설정 값을 저장하고 관리합니다.
public class SettingManager : MonoBehaviour
{
    static public SettingManager Instance;
    
    public SettingVariable setting;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if(!DataManager.Instance.tryLoadJson<SettingVariable>("setting.json", out setting)){
            Setting();
            Debug.Log("세이브된 설정 데이터가 없어 새로이 생성합니다.");
        }
        else
        {
            Debug.Log("세이브된 설정 데이터가 있어 불러옵니다.");
        }
    }

    void Setting()
    {
        setting.sound = new Settings.Sound();
        setting.graphicOption = new Settings.GraphicOption();
    }

    void SettingSave()
    {
        DataManager.Instance.saveJson<SettingVariable>("setting.json", setting);
    }
}

public class SettingVariable
{
    public Settings.Sound sound;
    public Settings.GraphicOption graphicOption;
}