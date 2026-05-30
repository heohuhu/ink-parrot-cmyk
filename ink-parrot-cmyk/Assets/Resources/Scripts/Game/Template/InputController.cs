using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
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
            if (CheckUIBlocking(Input.mousePosition))
                return;
            HandleInput(Input.mousePosition);
            AudioManager.Instance.PlayUI("터치");
        }
    #else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (CheckUIBlocking(touch.position))
                    return;

                HandleInput(touch.position);
                AudioManager.Instance.PlayUI("터치");
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

    bool CheckUIBlocking(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = screenPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<InputInterface>() != null)
                continue;
            return true;
        }

        return false;
    }

}