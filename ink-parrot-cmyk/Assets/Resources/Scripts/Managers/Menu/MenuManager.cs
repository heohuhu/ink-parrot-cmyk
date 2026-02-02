using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        
    }

    public void GameStart()
    {
        SceneController.Instance.LoadSceneAdditiveAsActive("Game");
        SceneController.Instance.UnloadScene("StartMenu");
    }
}
