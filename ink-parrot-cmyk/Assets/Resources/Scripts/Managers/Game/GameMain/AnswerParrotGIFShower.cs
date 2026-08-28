using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class AnswerParrotFlyerGIFShower : MonoBehaviour
{
    private List<List<Sprite>> GifImages = new List<List<Sprite>>();
    [SerializeField] private float TimeBetweenFrame = 0.1f;
    [SerializeField] private int BaseFrameIndex = 0; // GIF 플레이 전 상태 인덱스 지정
    [SerializeField] private int StartingFrameIndex = 0; // GIF 플레이 처음 상태 인덱스 지정
    [SerializeField] private int EndingFrameIndex = 0; // GIF 종료 상태 인덱스 지정 (-1: 마지막 프레임)
    [SerializeField] private bool AlwaysPlay = false;
    [SerializeField] private bool PlayReverse = false;
    [SerializeField] private List<Sprite> Head1 = new List<Sprite>();
    [SerializeField] private List<Sprite> Head2 = new List<Sprite>();
    [SerializeField] private List<Sprite> Head3 = new List<Sprite>();
    [SerializeField] private List<Sprite> Body1 = new List<Sprite>();
    [SerializeField] private List<Sprite> Body2 = new List<Sprite>();
    [SerializeField] private List<Sprite> Wing1 = new List<Sprite>();
    [SerializeField] private List<Sprite> Wing2 = new List<Sprite>();

    private float RemainDelay;
    private int CurrentIndex;
    private int GifSize;
    private bool isPlaying = false;
    private bool isPause = false;
    enum PlayType
    {
      forward, backward  
    };

    public SpriteRenderer[] TargetSprites;
    public Image[] TargetImage;

    public void Setting()
    {
        ArrayAssembling();
        isPlaying = false;
        GifSize = GifImages[0].Count;
        CurrentIndex = BaseFrameIndex;
        RemainDelay = TimeBetweenFrame;
        ChangeImage(CurrentIndex);
    }

    private void ArrayAssembling()
    {
        if(GifImages.Count != 0)
            return ;

        GifImages.Add(Head1);
        GifImages.Add(Head2);
        GifImages.Add(Head3);
        GifImages.Add(Body1);
        GifImages.Add(Body2);
        GifImages.Add(Wing1);
        GifImages.Add(Wing2);
    }

    public void SetColor(int template, Color color)
    {
        if(TargetSprites.GetLength(0) > 0)
        {
            TargetSprites[template].color = color;
        }
        else
        {
            TargetImage[template].color = color;
        }
    }

    public bool IsReversing()
    {
        return PlayReverse;
    }

    public void SetReversing(bool isReverse)
    {
        PlayReverse = isReverse;
    }
    public bool IsPlaying()
    {
        return isPlaying;
    }

    public void ActivatingPlay()
    {
        Activating();
    }

    //option이 true 면 아예 중단하고 처음부터 실행, false면 이미 실행 중일 때 입력 무시
    public void ActivatingPlay(bool option)
    {
        if(option == false && IsPlaying())
            return ;
        Activating();
    }

    public IEnumerator ActivatingPlayOnce()
    {
        ActivatingPlay();

        while(isPlaying == true)
        {
            yield return null;
        }
    }

    private void Activating()
    {
        isPlaying = true;

        if(PlayReverse)
            CurrentIndex = (EndingFrameIndex == -1 ? GifSize - 1 : EndingFrameIndex);
        else
            CurrentIndex = StartingFrameIndex;

        RemainDelay = TimeBetweenFrame;
        ChangeImage(CurrentIndex);
    }

    public void Stop()
    {
        isPause = true;
    }

    public void Pause()
    {
        isPause = false;
    }

    void Awake()
    {
        if(TargetSprites.GetLength(0) > 0)
            for(int i = 0; i < TargetSprites.GetLength(0); i++)
                TargetSprites[i].material = new Material(TargetSprites[i].material);
        if(TargetImage.GetLength(0) > 0)
            for(int i = 0; i < TargetImage.GetLength(0); i++)
                if(TargetImage[i].material != null)
                    TargetImage[i].material = new Material(TargetImage[i].material);
            
        Setting();
    }

    void Update()
    {
        if(isPause)
            return;
        
        if(AlwaysPlay || isPlaying){
            RemainDelay -= Time.deltaTime;

            if(RemainDelay <= 0)
            {
                if(PlayReverse == false){
                    if(CurrentIndex == (EndingFrameIndex == -1 ? GifSize - 1 : EndingFrameIndex) && AlwaysPlay == false){ //종료 조건 달성
                        isPlaying = false;
                        CurrentIndex = BaseFrameIndex;
                        ChangeImage(CurrentIndex);
                        return;
                    }
                }
                else
                {
                    if(CurrentIndex == (StartingFrameIndex == -1 ? GifSize - 1 : StartingFrameIndex) && AlwaysPlay == false){ //종료 조건 달성
                        isPlaying = false;
                        CurrentIndex = BaseFrameIndex;
                        ChangeImage(CurrentIndex);
                        return;
                    }
                }

                if(PlayReverse == false)
                    CurrentIndex++;
                else
                    CurrentIndex--;

                if(CurrentIndex >= GifSize)
                    CurrentIndex = 0;
                if(CurrentIndex < 0)
                    CurrentIndex = GifSize - 1;
                
                ChangeImage(CurrentIndex);

                RemainDelay = TimeBetweenFrame;
            }
        }
    }

    void ChangeImage(int index)
    {
        for(int i = 0; i < Constants.TemplateSize; i++)
        {
            ChangeImage(i, index);
        }
    }
    void ChangeImage(int template, int index)
    {
        if(GifImages[template][index] == null) //오류 방지 혹시모르잖아
            return ;

        if(TargetSprites.GetLength(0) > 0)
        {
            TargetSprites[template].sprite = GifImages[template][index];
        }
        else
        {
            TargetImage[template].sprite = GifImages[template][index];
        }
    }
}
