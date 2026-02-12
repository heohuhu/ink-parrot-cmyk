using UnityEngine;

public interface InputInterface
{
    public void OnTouch();
}

public class InputController: MonoBehaviour
{
    public static InputController Instance;

    void Awake()
    {
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
    void Update()
    {
    #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }
    #else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleInput(touch.position);
            }
        }
    #endif
    }

    void HandleInput(Vector3 screenPosition)
    {
        Vector3 screenPos = screenPosition;
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit != null)
        {
            hit.GetComponent<InputInterface>()?.OnTouch();
        }
    }


}