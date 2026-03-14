using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundFitToCamera : MonoBehaviour
{
    public enum VerticalAlign
    {
        Top,
        Center,
        Bottom
    }

    public enum SizeFitter
    {
        Min,
        Max
    }

    public VerticalAlign verticalAlign = VerticalAlign.Center;
    public SizeFitter sizeFitter = SizeFitter.Max;

    void Start()
    {
        FitToCamera();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
            FitToCamera();
    }
#endif

    void FitToCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr.sprite == null) return;

        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        // 원본 비율 유지
        float scale;
        if(sizeFitter == SizeFitter.Min)
            scale = Mathf.Min(camWidth / spriteWidth, camHeight / spriteHeight);
        else
            scale = Mathf.Max(camWidth / spriteWidth, camHeight / spriteHeight);

        transform.localScale = new Vector3(scale, scale, 1f);

        float scaledHeight = spriteHeight * scale;

        float camTop = cam.transform.position.y + camHeight / 2f;
        float camBottom = cam.transform.position.y - camHeight / 2f;

        float posY = cam.transform.position.y;

        switch (verticalAlign)
        {
            case VerticalAlign.Top:
                posY = camTop - scaledHeight / 2f;
                break;

            case VerticalAlign.Center:
                posY = cam.transform.position.y;
                break;

            case VerticalAlign.Bottom:
                posY = camBottom + scaledHeight / 2f;
                break;
        }

        transform.position = new Vector3(
            cam.transform.position.x,
            posY,
            transform.position.z
        );
    }
}