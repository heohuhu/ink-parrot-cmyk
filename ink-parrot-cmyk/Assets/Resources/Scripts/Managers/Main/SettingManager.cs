using UnityEngine;

//여러 설정 값을 저장하고 관리합니다.
public class SettingManager : MonoBehaviour
{
    static public SettingManager Instance;
    
    public SettingVariable setting = new SettingVariable();

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
        setting.sound.Setting();
        setting.graphicOption.Setting();
    }

    void SettingSave()
    {
        DataManager.Instance.saveJson<SettingVariable>("setting.json", setting);
    }
}

public class SettingVariable
{
    public Settings.Sound sound = new Settings.Sound();
    public Settings.GraphicOption graphicOption = new Settings.GraphicOption();
}

namespace Settings{
    public class Sound
    {
        public int Master = 100, SFX = 100, BGM = 100, Voice = 100, UI = 100;

        public void Setting()
        {
            Master = 100;
            SFX = 100;
            BGM = 100;
            Voice = 100;
            UI = 100;
        }

        public Sound()
        {
            Setting();
        }
    }

    public class GraphicOption
    {
        //색약 모드 관련
        public int ColorWeakness = 0;
        //쨍함 감소 관련
        public int ColorStrength = 100;

        public void Setting()
        {
            ColorWeakness = 0;
            ColorStrength = 100;
        }

        public GraphicOption()
        {
            Setting();
        }
    }
}