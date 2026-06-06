using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class Timer : MonoBehaviour
{
    public static Timer Instance { get; set; }
    public int BasicTime;
    private void Awake()
    {
        Instance = this;
    }
    GameTimer timer;
    public TextMeshProUGUI timeshower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = new GameTimer(BasicTime);
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.IsFinished())
        {
            timeshower.text = "TIME\n" + timer.GetRestTime();
            GameManager.Instance.GameEnd();
        }
        else
        {
            timer.Tick();
            timeshower.text = "TIME\n" + timer.GetRestTime();

            if(timer.GetRestTimeSeconds() <= 10f)
            {
                timeshower.color = Color.softRed;
            }
            else
            {
                timeshower.color = Color.black;
            }
        }
    }

    public void Pause()
    {
        timer.Pause();
    }

    public void Resume()
    {
        timer.Resume();
    }
    
    public void Reset()
    {
        timer.Reset();
    }

    public void TimeModify(float seconds)
    {
        timer.ModifyTime(seconds);
    }
}
public class GameTimer
{
    private float totalTime;     
    private float elapsedTime;   
    private bool isRunning;
    private bool isPaused;

    private float maxTotalTime;   // 추가: 최대 시간

    public GameTimer(float seconds)
    {
        totalTime = Mathf.Max(0f, seconds);
        maxTotalTime = totalTime;   // 최대값 저장
        elapsedTime = 0f;
        isRunning = false;
        isPaused = false;
    }

    /// <summary>
    /// 매 프레임 Update()에서 호출
    /// </summary>
    public void Tick()
    {
        if (!isRunning || isPaused)
            return;

        elapsedTime += Time.unscaledDeltaTime;
        elapsedTime = Mathf.Min(elapsedTime, totalTime);
    }
    public void ModifyTime(float seconds)
    {
        float remaining = totalTime - elapsedTime;
        remaining += seconds;

        remaining = Mathf.Clamp(remaining, 0f, maxTotalTime);

        elapsedTime = totalTime - remaining;
    }
    public void Start()
    {
        isRunning = true;
        isPaused = false;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void Reset()
    {
        elapsedTime = 0f;
    }

    public bool IsFinished()
    {
        return elapsedTime >= totalTime;
    }

    /// <summary>
    /// 남은 시간을 "MM:SS" 형식 문자열로 반환
    /// </summary>
    public string GetRestTime()
    {
        float remaining = Mathf.Max(0f, totalTime - elapsedTime);

        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);

        return $"{minutes:00}:{seconds:00}";
    }

    /// <summary>
    /// 남은 시간 (초)
    /// </summary>
    public float GetRestTimeSeconds()
    {
        return Mathf.Max(0f, totalTime - elapsedTime);
    }
}
