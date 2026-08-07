using UnityEngine;

public class Debuger : MonoBehaviour
{
    public void AnswerProcessing()
    {
        SettingManager.Instance.setting.debuger.isAnswerProcessing = !SettingManager.Instance.setting.debuger.isAnswerProcessing;
    }

    public void TutorialProcessing()
    {
        SettingManager.Instance.setting.isTutorial = !SettingManager.Instance.setting.isTutorial;
    }

    public void CollectionTutorialProcessing()
    {
        SettingManager.Instance.setting.isCustomTutorial = !SettingManager.Instance.setting.isCustomTutorial;
    }
}
