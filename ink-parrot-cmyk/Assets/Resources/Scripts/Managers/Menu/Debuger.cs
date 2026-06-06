using UnityEngine;

public class Debuger : MonoBehaviour
{
    public void AnswerProcessing()
    {
        SettingManager.Instance.setting.debuger.isAnswerProcessing = !SettingManager.Instance.setting.debuger.isAnswerProcessing;
    }
}
