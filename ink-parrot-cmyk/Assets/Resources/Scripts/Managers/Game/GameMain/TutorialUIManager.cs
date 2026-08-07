using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using System;

public class TutorialUIManager : MonoBehaviour
{
    static public TutorialUIManager Instance;
    [SerializeField]
    public GameObject TutorialCanvas;
    [SerializeField]
    public GameObject dialogueArea;
    [SerializeField]
    public GameObject ButtonArea;

    void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        TutorialCanvas.SetActive(true);

    }

    public IEnumerator PrintDialogue(string text)
    {
        if(text == "" || text == null)
        {
            TutorialCanvas.SetActive(false);
            return null;
        }
        TutorialCanvas.SetActive(true);
        TextMeshProUGUI tmpro = dialogueArea.GetComponentInChildren<TextMeshProUGUI>();
        tmpro.text = text;
        return null;
    }

    public void SetDialogueButton(bool isEnabled)
    {
        ButtonArea.GetComponent<Image>().raycastTarget = isEnabled;
        ButtonArea.GetComponent<Button>().interactable = isEnabled;
    }
}