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

    //0이면 왼쪽 정렬, 1이면 오른쪽 정렬
    public void SetTextAreaAlignment(bool isRight){
        ButtonArea.GetComponent<RectTransform>().localScale = new Vector3(isRight ? -1 : 1, 1, 1);
        dialogueArea.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(isRight ? -1 : 1, 1, 1);
    }
}