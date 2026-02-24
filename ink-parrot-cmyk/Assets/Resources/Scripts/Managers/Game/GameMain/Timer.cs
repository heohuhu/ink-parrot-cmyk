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
            GameManager.Instance.GameEnd();
        }
        else
        {
            timer.Tick();
            timeshower.text = "TIME\n" + timer.GetRestTime();
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
}
public class GameTimer
{
    private float totalTime;     // 전체 시간 (초)
    private float elapsedTime;   // 경과 시간 (초)
    private bool isRunning;
    private bool isPaused;

    public GameTimer(float seconds)
    {
        totalTime = Mathf.Max(0f, seconds);
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
