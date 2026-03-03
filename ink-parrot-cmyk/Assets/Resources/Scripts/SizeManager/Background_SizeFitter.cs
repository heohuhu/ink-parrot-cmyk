using UnityEngine;

[ExecuteAlways]
public class BackgroundFitToCamera : MonoBehaviour
{
    void Start()
    {
        FitToCamera();
    }

    void FitToCamera()
    {
        Camera cam = Camera.main;

        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        transform.localScale = new Vector3(
            width / spriteWidth,
            height / spriteHeight,
            1f
        );

        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0f);
    }
}