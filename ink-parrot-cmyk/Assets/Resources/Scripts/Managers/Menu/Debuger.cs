using UnityEngine;

public class Debuger : MonoBehaviour
{
    public void AnswerProcessing()
    {
        SettingManager.Instance.setting.debuger.isAnswerProcessing = !SettingManager.Instance.setting.debuger.isAnswerProcessing;
        MenuManager.Instance.PrintNotification(SettingManager.Instance.setting.debuger.isAnswerProcessing ? "모든 제출 정답 처리 유효화" : "모든 제출 정답 처리 무효화");
    }

    public void TutorialProcessing()
    {
        SettingManager.Instance.setting.isTutorial = !SettingManager.Instance.setting.isTutorial;
        MenuManager.Instance.PrintNotification(SettingManager.Instance.setting.isTutorial ? "메인 게임 튜토리얼 진행" : "메인 게임 튜토리얼 패스");
    }

    public void CollectionTutorialProcessing()
    {
        SettingManager.Instance.setting.isCustomTutorial = !SettingManager.Instance.setting.isCustomTutorial;
        MenuManager.Instance.PrintNotification(SettingManager.Instance.setting.isCustomTutorial ? "커스텀 튜토리얼 진행" : "커스텀 튜토리얼 패스");

    }

    public void PlayTimeAdjust(int updown)
    {
        Constants.Instance.PlayTime += updown;

        MenuManager.Instance.PrintNotification($"현재 플레이 타임은\n{Constants.Instance.PlayTime}초 입니다.");
    }
}
