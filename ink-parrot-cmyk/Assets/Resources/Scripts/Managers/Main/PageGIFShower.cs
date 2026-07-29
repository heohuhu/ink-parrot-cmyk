using UnityEngine;
using UnityEngine.UI;

public class PageGIFShower : MonoBehaviour
{
    [SerializeField] private Sprite[] GifImages;
    [SerializeField] private float TimeBetweenFrame = 0.1f;
    [SerializeField] private int BaseFrameIndex = 0; // GIF 플레이 전 상태 인덱스 지정
    [SerializeField] private int StartingFrameIndex = 0; // GIF 플레이 처음 상태 인덱스 지정
    [SerializeField] private int EndingFrameIndex = 0; // GIF 종료 상태 인덱스 지정 (-1: 마지막 프레임)
    [SerializeField] private bool AlwaysPlay = false;
    [SerializeField] private bool PlayReverse = false;
    private RectTransform targetRect;

    private float WidthScale = 1f;
    private float HeightScale = 1f;
    
    private float RemainDelay;
    private int CurrentIndex;
    private int GifSize;
    private bool isPlaying = false;
    private bool isPause = false;
    enum PlayType
    {
      forward, backward  
    };

    public SpriteRenderer TargetSprite;
    public Image TargetImage;

    public void Setting()
    {
        isPlaying = false;
        GifSize = GifImages.GetLength(0);
        CurrentIndex = BaseFrameIndex;
        RemainDelay = TimeBetweenFrame;
        ChangeImage(CurrentIndex);

        if (TargetImage != null)
        {
            targetRect = TargetImage.rectTransform;

            Sprite baseSprite = GifImages[BaseFrameIndex];

            WidthScale = targetRect.rect.width / baseSprite.rect.width;
            HeightScale = targetRect.rect.height / baseSprite.rect.height;
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
        if(IsPlaying())
            return ;
        Activating();
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
        if (TargetSprite != null)
            TargetSprite.material = new Material(TargetSprite.material);
        if (TargetImage != null && TargetImage.material != null)
            TargetImage.material = new Material(TargetImage.material);
            
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
        if(GifImages[index] == null) //오류 방지 혹시모르잖아
            return ;

        if(TargetSprite != null)
        {
            TargetSprite.sprite = GifImages[index];
        }
        else
        {
            Sprite sprite = GifImages[index];

            TargetImage.sprite = sprite;

            if (targetRect != null)
            {
                float width = sprite.rect.width * WidthScale;
                float height = sprite.rect.height * HeightScale;

                targetRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                targetRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            }
        }
    }
}
