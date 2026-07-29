using UnityEngine;
using UnityEngine.UI;
public class SettingUIManager : MonoBehaviour
{
    public Slider Master, SFX, BGM, UI;

    void Start()
    {
        Master.SetValueWithoutNotify((float)SettingManager.Instance.setting.sound.Master);
        SFX.SetValueWithoutNotify((float)SettingManager.Instance.setting.sound.SFX);
        BGM.SetValueWithoutNotify((float)SettingManager.Instance.setting.sound.BGM);
        UI.SetValueWithoutNotify((float)SettingManager.Instance.setting.sound.UI);
    }

    public void SettingChange_Master(float value)
    {
        SettingManager.Instance.setting.sound.Master = Mathf.RoundToInt(value);
    }

    public void SettingChange_SFX(float value)
    {
        SettingManager.Instance.setting.sound.SFX = Mathf.RoundToInt(value);
    }

    public void SettingChange_BGM(float value)
    {
        SettingManager.Instance.setting.sound.BGM = Mathf.RoundToInt(value);
    }

    public void SettingChange_UI(float value)
    {
        SettingManager.Instance.setting.sound.UI = Mathf.RoundToInt(value);
    }
}
