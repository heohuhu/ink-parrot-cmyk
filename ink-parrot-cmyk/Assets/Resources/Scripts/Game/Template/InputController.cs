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
        }
    #else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

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

    bool CheckUIBlocking(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = screenPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            Debug.Log("입력을 막은 UI 목록:");

            foreach (var result in results)
            {
                Debug.Log(" - " + result.gameObject.name);
            }

            return true;
        }

        return false;
    }

}