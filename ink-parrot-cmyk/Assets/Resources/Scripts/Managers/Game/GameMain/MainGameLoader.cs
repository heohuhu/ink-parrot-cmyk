using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MainGameLoader : MonoBehaviour
{
    static public MainGameLoader Instacne;

    [SerializeField] List<GameObject> OffTargets;
    [SerializeField] List<GameObject> OnTargets;

    void Awake()
    {
        Instacne = this;
    }

    void Start()
    {
        for(int i = 0; i < OffTargets.Count; i++)
            OffTargets[i].SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartLoader(){
        StartCoroutine(StartLoad());
    }

    IEnumerator StartLoad()
    {
        for(int i = 0; i < OnTargets.Count; i++)
            OnTargets[i].SetActive(true);
        
        yield return null;

        GameUiManager.Instance.Setting();
        AnswerSheet.Instance.Setting();
        yield return null;

        GameManager.Instance.StartLoad();
        yield return null;

    }
}
