using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;
    public GameObject [] backgrounds;
    private int current_background_index = 0;

    private void Awake()
    {
        // 싱글톤 처리
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBackgroundStop(bool isStop)
    {
        GIFShower gif = backgrounds[current_background_index].GetComponent<GIFShower>();

        if(gif == null)
            return;

        if(isStop)
            gif.Stop();
        else
            gif.Pause();
    }


}
