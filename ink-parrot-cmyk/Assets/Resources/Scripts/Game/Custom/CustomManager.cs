using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomManager : MonoBehaviour
{
    public static CustomManager Instance;

    void Awake()
    {
        Instance = this;
    }

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
        parrotName = "null";

        CustomParrotShower.Instance.Reset();
        
        SetButtonColor(Constants.ColorType.Cyan, 3);
        SetButtonColor(Constants.ColorType.Magenta, 3);
        SetButtonColor(Constants.ColorType.Yellow, 3);
    }

    public void SetButtonColor(Constants.ColorType CMYK, int LightType)
    {
        Image spr = MYC[(int)CMYK].GetComponent<Image>();
        spr.color = Constants.Instance.GetColor(CMYK, LightType);
    }

    public void ReturnButtonClicked()
    {
        SceneController.Instance.LoadSceneAdditiveAsActive("StartMenu");
        SceneController.Instance.UnloadScene("CustomPage");
    }

    public void TemplateSelected(int template)
    {
        selectedTemplate = template;

        SetButtonColor(Constants.ColorType.Cyan, CustomParrotShower.Instance.ColorData[template, (int)Constants.ColorType.Cyan]);
        SetButtonColor(Constants.ColorType.Magenta, CustomParrotShower.Instance.ColorData[template, (int)Constants.ColorType.Magenta]);
        SetButtonColor(Constants.ColorType.Yellow, CustomParrotShower.Instance.ColorData[template, (int)Constants.ColorType.Yellow]);
    }

    public void SaveParrot()
    {
        ParrotInfo result = new ParrotInfo();
        result.data.Add("0");
        result.data.Add(CustomUIManager.Instance.inputField.text);
        result.data.AddRange(CustomParrotShower.Instance.getParrotData());

        ParrotDataManager.Instance.NewCustomParrotAdd(result);
        Utility.PrintRecursive(result, 0);
        CustomUIManager.Instance.InputPanelClose();
    }

    public void ColorTouched(int color)
    {
        if(selectedTemplate == -1)
            return ;
        if(--CustomParrotShower.Instance.ColorData[selectedTemplate, color] < 0){
            CustomParrotShower.Instance.ColorData[selectedTemplate, color] = 3;
        }
        CustomParrotShower.Instance.ShowSampleImage(selectedTemplate);
        SetButtonColor((Constants.ColorType)color, CustomParrotShower.Instance.ColorData[selectedTemplate, color]);
    }

    public void SaveParrotStart()
    {
        CustomUIManager.Instance.InputPanelOpen();
    }

    public void SaveParrotComplete()
    {
        Reset();
        CustomUIManager.Instance.InputPanelClose();
    }

    public void SaveParrotEnd()
    {
        CustomUIManager.Instance.InputPanelClose();
    }
}
