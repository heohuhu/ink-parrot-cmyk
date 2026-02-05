using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopTime()
    {
        Timer.Instance.Pause();
    }

    public void ResumeTime()
    {
        Timer.Instance.Resume();
    }

    public void ReturnStartMenu()
    {
        SceneController.Instance.LoadSceneAdditiveAsActive("StartMenu");
        SceneController.Instance.UnloadScene("Game");
    }
}
