using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TutorialUIManager : MonoBehaviour
{
    static public TutorialUIManager Instance;
    [SerializeField]
    public GameObject TutorialCanvas;
    [SerializeField]
    public TextMeshProUGUI dialogueArea;

    void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        TutorialCanvas.SetActive(true);

    }

    public void PrintDialogue(string text)
    {
        dialogueArea.text = text;
    }
}