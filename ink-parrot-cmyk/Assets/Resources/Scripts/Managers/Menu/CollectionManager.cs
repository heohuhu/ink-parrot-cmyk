using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CollectionManager : MonoBehaviour
{
    static public CollectionManager Instance;
    public GIFShower collectionGIF;
    private int ParrotsSize;
    private int [] page = new int[2];
    private int current_page; // 0이면 사전 설정된 앵무새 목록, 1이면 커스텀 앵무새 목록
    public GameObject [] collection_showcase = new GameObject[3];
    public GameObject [] page_select = new GameObject[2];
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Reset();
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
        for(int i = 0; i < 3; i++){
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
            for(int i = 0; i < 3; i++)
            {
                StartFadeOut(i, 200);
            }

            yield return new WaitForSeconds(0.3f);
        }

        collectionGIF.ActivatingPlay();

        while(true){
            if(collectionGIF.IsPlaying())
                yield return null;
            else
                break;
        }
        
        for(int i = 0; i < 3; i++)
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
            if(page[current_page] >= Mathf.Ceil((float)Constants.BasicParrotsSize / 3))
                page[current_page]--;
            else {
                collectionGIF.SetReversing(false);
                StartPageChange();
            }
        }else if(current_page == 1)
        {
            page[current_page]++;
            if((page[current_page] * 3 + Constants.BasicParrotsSize) / 3 >= Mathf.Ceil((float)ParrotsSize / 3))
                page[current_page]--;
            else {
                collectionGIF.SetReversing(false);
                StartPageChange();
            }
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
            index = page[0] * 3 + target_index;
        else if(current_page == 1) //커스텀 앵무새
            index = Constants.BasicParrotsSize + page[1] * 3 + target_index;
        
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

    Coroutine [] fadeRoutine = new Coroutine[3];

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
