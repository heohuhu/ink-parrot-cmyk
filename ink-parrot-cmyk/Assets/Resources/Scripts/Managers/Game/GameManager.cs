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

    //-1 : 없는 상태, 0 : Cyan, 1 : Magenta, 2 : Yellow
    enum Color { Cyan, Magenta, Yellow }
    ParrotTemplate [] parrots = new ParrotTemplate[3];
    int selectedColor = -1;
    int selectedTemplate = -1;
    float squeezing = 100f;
    public void SelectColor(int ColorType)
    {
        selectedColor = ColorType;
    }
    public void SelectTemplate(int Template)
    {
        selectedTemplate = Template;
        squeezing = (this.parrots[selectedColor].BodyTemplatesInk[selectedTemplate] == 0 ? 0f : 100f);
    }
    public void SqueezeColor()
    {
        if(squeezing > 0f)
            squeezing -= 0.1f;
    }
}
