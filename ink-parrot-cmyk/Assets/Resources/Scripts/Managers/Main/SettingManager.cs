using UnityEngine;

//여러 설정 값을 저장하고 관리합니다.
[System.Serializable]
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
        setting = new SettingVariable();
    }

    public void SettingSave()
    {
        DataManager.Instance.saveJson<SettingVariable>("setting.json", setting);
    }


    public Color GetColor(Constants.ColorType color)
    {
        Color original = Constants.Instance.GetColor(color);
        // RGB -> HSV
        Color.RGBToHSV(original, out float h, out float s, out float v);

        // 쨍함 감소 (채도 감소)
        float reductionRatio = 1f - (setting.graphicOption.ColorStrength / 100f);
        s *= reductionRatio;

        // HSV -> RGB
        Color result = Color.HSVToRGB(h, s, v);
        result.a = original.a; // 알파값 유지

        return original;
    }
}

[System.Serializable]
public class SettingVariable
{
    public Settings.Sound sound = new Settings.Sound();
    public Settings.GraphicOption graphicOption = new Settings.GraphicOption();
}

namespace Settings{
    [System.Serializable]
    public class Sound
    {
        public int Master = 100, SFX = 100, BGM = 100, UI = 100;

        public void Setting()
        {
            Master = 100;
            SFX = 100;
            BGM = 100;
            UI = 100;
        }

        public Sound()
        {
            Setting();
        }
    }

    [System.Serializable]
    public class GraphicOption
    {
        //색약 모드 관련
        public int ColorWeakness = 0;
        //쨍함 감소 관련
        public int ColorStrength = 0;

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