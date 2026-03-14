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
}
