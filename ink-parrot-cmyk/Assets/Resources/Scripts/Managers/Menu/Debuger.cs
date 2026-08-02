using UnityEngine;

public class Debuger : MonoBehaviour
{
    public void AnswerProcessing()
    {
        SettingManager.Instance.setting.debuger.isAnswerProcessing = !SettingManager.Instance.setting.debuger.isAnswerProcessing;
    }

    public void TutorialProcessing()
    {
        SettingManager.Instance.setting.debuger.isTutorial = !SettingManager.Instance.setting.debuger.isTutorial;
    }
}
