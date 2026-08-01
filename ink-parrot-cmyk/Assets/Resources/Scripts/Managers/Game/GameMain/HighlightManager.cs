using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HighlightManager : MonoBehaviour
{
    public static HighlightManager Instance;

    public bool isActivated = false;

    [SerializeField, Range(0f, 1f)]
    private float darkBrightness = 0.4f;

    [SerializeField]
    public Dictionary<GameObject, UIColorData> UIs = new Dictionary<GameObject, UIColorData>();

    void Awake()
    {
        Instance = this;
    }

    void Update(){
        if(isActivated == true)
        {
            RefreshAllColors();
        }
    }

    public void Activate()
    {
        isActivated = true;
    }

    public void Deactivate()
    {
        isActivated = false;
    }

    public void UpdateObjectColor(GameObject target)
    {
        UIColorData data = GetUIColorData(target);

        if (data == null)
            return;

        data.isUpdateTarget = true;
        data.GraphicUpdate(target.GetComponent<Graphic>());
    }

    public void UpdateFromParent(GameObject parent)
    {
        if(parent == null)
            return;
        
        UpdateDFS(parent.transform);
    }

    public void EnableFromParent(GameObject superParent)
    {
        if (superParent == null)
            return;

        EnableDFS(superParent.transform);
    }

    public void EnableOne(GameObject targetObject)
    {
        UIColorData data = GetUIColorData(targetObject);

        if (data == null)
            return;

        data.isEnabled = true;
        RefreshColor(data);
    }

    public void DisableFromParent(GameObject superParent)
    {
        if (superParent == null)
            return;

        DisableDFS(superParent.transform);
    }

    public void DisableOne(GameObject targetObject)
    {
        UIColorData data = GetUIColorData(targetObject);

        if (data == null)
            return;

        data.isEnabled = false;
        RefreshColor(data);
    }

    public void AddException(GameObject targetObject)
    {
        UIColorData data = GetUIColorData(targetObject);

        if (data == null)
            return;

        data.isException = true;
        RefreshColor(data);
    }

    public void DeleteException(GameObject targetObject)
    {
        UIColorData data = GetUIColorData(targetObject);

        if (data == null)
            return;

        data.isException = false;
        RefreshColor(data);
    }

    public void RegisterFromParent(GameObject superParent)
    {
        if (superParent == null)
            return;

        AddDFS(superParent.transform);
    }

    public void RegisterOne(GameObject targetObject)
    {
        if (targetObject == null)
            return;

        Graphic graphic = targetObject.GetComponent<Graphic>();

        if (graphic != null && !UIs.ContainsKey(targetObject))
        {
            UIs.Add(targetObject,
                new UIColorData(targetObject, graphic));
        }
    }

    public void UnregisterFromParent(GameObject superParent)
    {
        if (superParent == null)
            return;

        RemoveDFS(superParent.transform);
    }

    public void UnregisterOne(GameObject targetObject)
    {
        if (targetObject == null)
            return;

        UIs.Remove(targetObject);
    }

    public void Setting()
    {
        foreach (UIColorData data in UIs.Values)
        {
            data.isEnabled = false;
            data.isException = false;
        }

        RefreshAllColors();
    }

    private void AddDFS(Transform current)
    {
        //if (!current.gameObject.activeInHierarchy)
        //    return;

        Graphic graphic = current.GetComponent<Graphic>();

        if (graphic != null && !UIs.ContainsKey(current.gameObject))
        {
            UIs.Add(current.gameObject,
                new UIColorData(current.gameObject, graphic));
        }

        foreach (Transform child in current)
        {
            AddDFS(child);
        }
    }

    private void RemoveDFS(Transform current)
    {
        UIs.Remove(current.gameObject);

        foreach (Transform child in current)
        {
            RemoveDFS(child);
        }
    }

    private void UpdateDFS(Transform current)
    {
        UpdateObjectColor(current.gameObject);

        foreach (Transform child in current)
        {
            UpdateDFS(child);
        }
    }

    private void EnableDFS(Transform current)
    {
        UIColorData data = GetUIColorData(current.gameObject);

        if (data != null)
        {
            data.isEnabled = true;
            data.isUpdateTarget = true;
        }

        foreach (Transform child in current)
        {
            EnableDFS(child);
        }
    }

    private void DisableDFS(Transform current)
    {
        UIColorData data = GetUIColorData(current.gameObject);

        if (data != null)
        {
            data.isEnabled = false;
            data.isUpdateTarget = true;
        }

        foreach (Transform child in current)
        {
            DisableDFS(child);
        }
    }

    private UIColorData GetUIColorData(GameObject target)
    {
        if (target == null)
            return null;

        UIs.TryGetValue(target, out UIColorData data);
        return data;
    }

    // 색상이 어두워지는 대상이 아니면 원본 색상으로, 대상이면 어두워지도록
    private void RefreshColor(UIColorData data)
    {
        if (data.graphic == null)
            return;

        Color color = data.originalColor;

        if (!data.isEnabled && !data.isException)
        {
            color.r *= darkBrightness;
            color.g *= darkBrightness;
            color.b *= darkBrightness;
        }

        data.graphic.color = color;
    }

    private void RefreshAllColors()
    {
        foreach (UIColorData data in UIs.Values)
        {
            if(data.isUpdateTarget == true)
                RefreshColor(data);
        }
    }
}

public class UIColorData
{
    public GameObject gameObject;
    public Graphic graphic;
    public Color originalColor;

    public bool isEnabled;
    public bool isException;
    public bool isUpdateTarget;

    public UIColorData(GameObject obj, Graphic target)
    {
        gameObject = obj;
        graphic = target;
        originalColor = target.color;

        isEnabled = false;
        isException = false;
        isUpdateTarget = false;
    }

    public void GraphicUpdate(Graphic target)
    {
        graphic = target;
        originalColor = target.color;
    }
}