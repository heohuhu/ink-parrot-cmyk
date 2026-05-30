using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
public class CollectionManager : MonoBehaviour
{
    static public CollectionManager Instance;
    private const int UnitPerPage = 3;


    public GIFShower collectionGIF;
    private int ParrotsSize;
    private int [] page = new int[2];
    private int current_page; // 0이면 사전 설정된 앵무새 목록, 1이면 커스텀 앵무새 목록
    public GameObject [] collection_showcase = new GameObject[UnitPerPage];
    public GameObject [] page_select = new GameObject[2];
    public TextMeshProUGUI PageText;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Reset();
    }

    void Update()
    {
        PageText.text = (page[current_page] + 1).ToString() + "/" + getTotalPage(current_page).ToString();
    }

    public void Reset()
    {
        ParrotsSize = ParrotDataManager.Instance.getParrotCount();
        page[0] = page[1] = 0;
        current_page = 0;
        isPageUpdating = false;
    }

    public void CollectionOn()
    {
        for(int i = 0; i < UnitPerPage; i++){
            CanvasGroup cg = collection_showcase[i].GetComponent<CanvasGroup>();
            if (cg == null) cg = collection_showcase[i].AddComponent<CanvasGroup>();
            cg.alpha = 0f;
        }
        collectionGIF.Setting();
        PageChanging = StartCoroutine(PageUpdate(0));

    }

    Coroutine PageChanging;
    private void StartPageChange()
    {
        if(isPageUpdating == true)
            return;
        PageChanging = StartCoroutine(PageUpdate(1));
    }

    private bool isPageUpdating = false;
    
    IEnumerator PageUpdate(int option)
    {
        isPageUpdating = true;
        if(option == 1){
            for(int i = 0; i < UnitPerPage; i++)
            {
                StartFadeOut(i, 200);
            }

            yield return new WaitForSeconds(0.3f);
        }

        AudioManager.Instance.PlayUI("컬렉션페이지변경");

        collectionGIF.ActivatingPlay();

        while(true){
            if(collectionGIF.IsPlaying())
                yield return null;
            else
                break;
        }
        
        for(int i = 0; i < UnitPerPage; i++)
        {
            TextChange(collection_showcase[i], i);
            ImageChange(collection_showcase[i], i);
            StartFadeIn(i, 200);
        }

        isPageUpdating = false;
    }

    public void PageUP()
    {
        if(current_page == 0)
        {
            page[current_page]++;
            if(page[current_page] >= Mathf.Ceil((float)Constants.BasicParrotsSize / UnitPerPage))
                page[current_page]--;
            else {
                collectionGIF.SetReversing(false);
                StartPageChange();
            }
        }else if(current_page == 1)
        {
            page[current_page]++;
            if((page[current_page] * UnitPerPage + Constants.BasicParrotsSize) / UnitPerPage >= Mathf.Ceil((float)ParrotsSize / UnitPerPage))
                page[current_page]--;
            else {
                collectionGIF.SetReversing(false);
                StartPageChange();
            }
        }
    }

    private int getTotalPage(int page_type)
    {
        if(page_type == 0)
        {
            return (int)Mathf.Ceil((float)Constants.BasicParrotsSize / 3);
        }
        else
        {
            return (int)Mathf.Ceil((float)(ParrotsSize - Constants.BasicParrotsSize) / 3);
        }
    }

    public void PageDown()
    {
        page[current_page]--;
        if(page[current_page] < 0)
            page[current_page] = 0;
        else {
                collectionGIF.SetReversing(true);
                StartPageChange();
            }
    }

    public void TextChange(GameObject target, int target_index)
    {
        
        TextMeshProUGUI tmpro = target.GetComponentInChildren<TextMeshProUGUI>();
        
        string text;
        int index = 0;
        
        if(current_page == 0) //사전 등록 앵무새
            index = page[0] * UnitPerPage + target_index;
        else if(current_page == 1) //커스텀 앵무새
            index = Constants.BasicParrotsSize + page[1] * UnitPerPage + target_index;
        
        text = ParrotDataManager.Instance.getParrotName(index);

        if(text == null)
            target.SetActive(false);
        else {
            tmpro.text = text;
            target.SetActive(true);
        }
    }

    public void ImageChange(GameObject target, int target_index) //이건 앵무새 이미지 에셋 잘 적용한 뒤에 하는걸로
    {
        
    }

    public void pageSelect(int n)
    {
        current_page = n;
        CollectionOn();

    }

    Coroutine [] fadeRoutine = new Coroutine[UnitPerPage];

    public void StartFadeIn(int index, int time)
    {
        if (fadeRoutine[index] != null)
            StopCoroutine(fadeRoutine[index]);

        fadeRoutine[index] = StartCoroutine(FadeIn(collection_showcase[index], time));
    }

    public void StartFadeOut(int index, int time)
    {
        if (fadeRoutine[index] != null)
            StopCoroutine(fadeRoutine[index]);

        fadeRoutine[index] = StartCoroutine(FadeOut(collection_showcase[index], time));
    }
    private IEnumerator FadeIn(GameObject target, int time)
    {
        Debug.Log("페이드 인 실행");
        float duration = time / 1000f;
        float t = 0f;

        CanvasGroup cg = target.GetComponent<CanvasGroup>();
        if (cg == null) cg = target.AddComponent<CanvasGroup>();

        float startAlpha = cg.alpha;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, 1f, t / duration);
            yield return null;
        }

        cg.alpha = 1f;
    }
    private IEnumerator FadeOut(GameObject target, int time)
    {
        float duration = time / 1000f;
        float t = 0f;

        CanvasGroup cg = target.GetComponent<CanvasGroup>();
        if (cg == null) cg = target.AddComponent<CanvasGroup>();

        float startAlpha = cg.alpha;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, 0f, t / duration);
            yield return null;
        }

        cg.alpha = 0f;
    }
}
