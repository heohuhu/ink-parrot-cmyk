using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CustomUIManager : MonoBehaviour
{
    public static CustomUIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public GameObject returnPanel, InputPanel;
    public TMP_InputField inputField;
    public void ReturnButtonClicked()
    {
        returnPanel.SetActive(true);
    }

    public void ReturnMenuUnload()
    {
        returnPanel.SetActive(false);
    }

    public void InputPanelOpen()
    {
        inputField.text = "";
        InputPanel.SetActive(true);
    }

    public void InputPanelClose()
    {
        InputPanel.SetActive(false);
    }

    public GameObject [] TemplateButton = new GameObject [Constants.TemplateSize];

    public void TemplateButtonOutlineSetting()
    {
        float x = 5f, y = 5f;
        for(int i = 0; i < Constants.TemplateSize; i++)
        {
            Outline tmp = TemplateButton[i].GetComponent<Outline>();
            tmp.effectDistance = new Vector2(x, y);
            tmp.enabled = false;
        }
    }

    public void TemplateButtonOutlineEnable(int index)
    {
        TemplateButtonOutlineSetting();
        Outline tmp = TemplateButton[index].GetComponent<Outline>();
        tmp.enabled = true;
    }
}
