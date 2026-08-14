using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomParrotShower : MonoBehaviour
{
    public int[,] ColorData = new int [Constants.TemplateSize, 3];
    static public CustomParrotShower Instance;
    
    public GameObject [] BodyTemplates = new GameObject[Constants.TemplateSize + 1];
    private int[] TemplatesSiblingIndex = new int[Constants.TemplateSize + 1];
    
    void Awake()
    {
        Instance = this;
    }

    public void Reset()
    {
        for(int i = 0; i < Constants.TemplateSize; i++)
        {
            ColorData[i, 0] = ColorData[i, 1] = ColorData[i, 2] = 3;
            Outline outline = BodyTemplates[i].GetComponent<Outline>();
            outline.effectDistance = new Vector2(1.5f, 1.5f);
            outline.effectColor = new Color(1f, 0.92f, 0.016f, 1f);
            outline.enabled = false;

            TemplatesSiblingIndex[i] = BodyTemplates[i].GetComponent<RectTransform>().GetSiblingIndex();
        }

        TemplatesSiblingIndex[Constants.TemplateSize] = BodyTemplates[Constants.TemplateSize].GetComponent<RectTransform>().GetSiblingIndex();
        ShowSampleImage();

        TemplateUnselected();
    }

    public void ShowSampleImage()
    {
        for(int template = 0; template < Constants.TemplateSize; template++)
        {
            Color C = Constants.Instance.GetColor(Constants.ColorType.Cyan, this.ColorData[template, (int)Constants.ColorType.Cyan]);
            Color M = Constants.Instance.GetColor(Constants.ColorType.Magenta, this.ColorData[template, (int)Constants.ColorType.Magenta]);
            Color Y = Constants.Instance.GetColor(Constants.ColorType.Yellow, this.ColorData[template, (int)Constants.ColorType.Yellow]);

            // 색깔 조합
            Color result = Utility.CombineColor(C, M, Y);

            Image spr = this.BodyTemplates[template].GetComponent<Image>();
            spr.color = result;
        }
    }

    public void ShowSampleImage(int template)
    {
        Color C = Constants.Instance.GetColor(Constants.ColorType.Cyan, this.ColorData[template, (int)Constants.ColorType.Cyan]);
        Color M = Constants.Instance.GetColor(Constants.ColorType.Magenta, this.ColorData[template, (int)Constants.ColorType.Magenta]);
        Color Y = Constants.Instance.GetColor(Constants.ColorType.Yellow, this.ColorData[template, (int)Constants.ColorType.Yellow]);

        // 색깔 조합
        Color result = Utility.CombineColor(C, M, Y);

        Image spr = this.BodyTemplates[template].GetComponent<Image>();
        spr.color = result;
    }

    public Color GetColor(int CMYK, int LightType)
    {
        Color tmp = SettingManager.Instance.GetColor((Constants.ColorType)CMYK);
        float t = Mathf.Clamp01(Constants.Instance.GetLightTypeData(LightType) / 100f);
        tmp = Color.Lerp(Color.white, tmp, t);

        return tmp;
    }

    public List<string> getParrotData()
    {
        List<string> parrotsData = new List<string>();

        for(int i = 0; i < Constants.TemplateSize; i++)
        {
            parrotsData.Add(this.ColorData[i, (int)Constants.ColorType.Cyan].ToString());
            parrotsData.Add(this.ColorData[i, (int)Constants.ColorType.Magenta].ToString());
            parrotsData.Add(this.ColorData[i, (int)Constants.ColorType.Yellow].ToString());
        }

        return parrotsData;
    }

    //isLast 가 true면 가장 뒤로, isLast가 false면 원래 위치로
    public void ChangeTemplateSiblingLast(int index, bool isLast)
    {
        if(isLast)
            BodyTemplates[index].GetComponent<RectTransform>().SetAsLastSibling();
        else
            BodyTemplates[index].GetComponent<RectTransform>().SetSiblingIndex(TemplatesSiblingIndex[index]);
    }

    public void TemplateSelected(int template)
    {
        Outline outline = BodyTemplates[template].GetComponent<Outline>();
        outline.enabled = true;
        ChangeTemplateSiblingLast(template, true);
        ChangeTemplateSiblingLast(Constants.TemplateSize, true);
    }

    public void TemplateUnselected()
    {
        for(int template = 0; template < Constants.TemplateSize; template++){
            Outline outline = BodyTemplates[template].GetComponent<Outline>();
            outline.enabled = false;
            ChangeTemplateSiblingLast(template, false);
        }
        ChangeTemplateSiblingLast(Constants.TemplateSize, false);
    }
}
