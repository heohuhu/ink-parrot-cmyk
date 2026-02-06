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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
