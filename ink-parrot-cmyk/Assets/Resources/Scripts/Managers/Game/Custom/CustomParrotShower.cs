using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomParrotShower : MonoBehaviour
{
    public int[,] ColorData = new int [Constants.TemplateSize, 3];
    static public CustomParrotShower Instance;
    private GameObject [] BodyTemplates = new GameObject[Constants.TemplateSize];
    
    void Awake()
    {
        Instance = this;
    }

    public void Reset()
    {
        for(int i = 0; i < Constants.TemplateSize; i++)
        {
            ColorData[i, 0] = ColorData[i, 1] = ColorData[i, 2] = 3;
        }

        ShowSampleImage();
    }

    public void ShowSampleImage()
    {
        for(int template = 0; template < Constants.TemplateSize; template++)
        {
            Color C = GetColor((int)Constants.ColorType.Cyan, this.ColorData[template, (int)Constants.ColorType.Cyan]);
            Color M = GetColor((int)Constants.ColorType.Magenta, this.ColorData[template, (int)Constants.ColorType.Magenta]);
            Color Y = GetColor((int)Constants.ColorType.Yellow, this.ColorData[template, (int)Constants.ColorType.Yellow]);

            Color result = new Color(
                C.r * M.r * Y.r,
                C.g * M.g * Y.g,
                C.b * M.b * Y.b,
                1f
            );

            Image spr = this.BodyTemplates[template].GetComponent<Image>();

            spr.color = result;
        }
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
}
