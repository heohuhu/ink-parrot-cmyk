using UnityEngine;
using UnityEngine.UI;

public class CustomUIManager : MonoBehaviour
{
    public static CustomUIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public GameObject returnPanel;

    public void ReturnButtonClicked()
    {
        returnPanel.SetActive(true);
    }

    public void ReturnMenuUnload()
    {
        returnPanel.SetActive(false);
    }


}
