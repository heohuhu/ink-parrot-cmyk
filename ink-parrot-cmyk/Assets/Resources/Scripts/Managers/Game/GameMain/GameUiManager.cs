using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class GameUiManager : MonoBehaviour
{
    public static GameUiManager Instance;
    public GameObject PauseMenu;
    public GameObject ReturnMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void PauseMenuOpen()
    {
        PauseMenu.SetActive(true);
        PauseMenuManager.Instance.OpenMenu();
    }

    public void PauseMenuClose()
    {
        PauseMenu.SetActive(false);
    }

    public void ReturnMenuOpen()
    {
        ReturnMenu.SetActive(true);
    }

    public void ReturnMenuClose()
    {
        ReturnMenu.SetActive(false);
    }


    public GameObject SelectedPanel, GamePanel;
    public void SelectColor(int ColorType)
    {
        GamePanel.SetActive(false);
        SelectedPanel.SetActive(true);
    }

    public void UnSelectColor()
    {
        extractButton.SetActive(false);
        lightManagingSlider.SetActive(false);
        SelectedPanel.SetActive(false);
        GamePanel.SetActive(true);
    }

    public GameObject extractButton, lightManagingSlider, refillButton;

    public void SelectTemplate(int value)
    {
        if(value == 0){
            refillButton.SetActive(true);
            extractButton.SetActive(false);
        }else{
            extractButton.SetActive(true);
            refillButton.SetActive(false);
        }
    }

    public void SetLightManagingSlider(int value)
    {
        if(value == 0){
            lightManagingSlider.SetActive(false);
            return ;
        }
        lightManagingSlider.SetActive(true);
        lightManagingSlider.GetComponent<Slider>().SetValueWithoutNotify(value);
    }
}
