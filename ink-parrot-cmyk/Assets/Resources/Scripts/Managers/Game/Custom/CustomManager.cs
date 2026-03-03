using UnityEngine;
using System.Collections.Generic;

public class CustomManager : MonoBehaviour
{
    public int selectedTemplate;
    private string parrotName;
    public GameObject ParrotSample;
    public GameObject [] MYC = new GameObject[3];
    void Start()
    {
        Reset();
    }

    void Reset()
    {
        selectedTemplate = -1;
        parrotName = "이름을 입력해주세요";
    }

    public void ReturnButtonClicked()
    {
        SceneController.Instance.LoadSceneAdditiveAsActive("StartMenu");
        SceneController.Instance.UnloadScene("CustomPage");
    }

    public void TemplateSelected(int template)
    {
        selectedTemplate = template;
    }

    public void SaveParrot()
    {
        List<string> result = new List<string>();

    }

    public void ColorTouched(int color)
    {
        if(selectedTemplate == -1)
            return ;

        if(--CustomParrotShower.Instance.ColorData[selectedTemplate, color] < 0){
            CustomParrotShower.Instance.ColorData[selectedTemplate, color] = 2;
            CustomParrotShower.Instance.ShowSampleImage();
        }
    }
}
