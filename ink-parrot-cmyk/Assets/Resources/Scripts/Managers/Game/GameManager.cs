using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
        for(int i = 0; i < 3; i++)
            this.parrots[i] = parrots_objects[i].GetComponent<ParrotTemplate>();
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
    ParrotTemplate [] parrots = new ParrotTemplate[3];
    public GameObject [] parrots_objects = new GameObject[3];
    int selectedColor = -1;
    int selectedTemplate = -1;
    float squeezing = 100f;
    public void SelectColor(int ColorType)
    {
        Debug.Log("색상 선택됨");
        selectedColor = ColorType;
        StartCoroutine(parrots[ColorType].ObjectSelected());
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
    public void InputDetected(int color)
    {
        if(selectedColor == -1)
            SelectColor(color);
    }
}
