using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ResumeManager : MonoBehaviour
{
    public GameObject Resume_Countdown_Panel;
    public TextMeshProUGUI Resume_Countdown;
    static public ResumeManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void TimerStart()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        Resume_Countdown.text = "3";
        yield return new WaitForSeconds(1f);
        Resume_Countdown.text = "2";
        yield return new WaitForSeconds(1f);
        Resume_Countdown.text = "1";
        yield return new WaitForSeconds(1f);
        PauseMenuManager.Instance.Resume();
    }
}
