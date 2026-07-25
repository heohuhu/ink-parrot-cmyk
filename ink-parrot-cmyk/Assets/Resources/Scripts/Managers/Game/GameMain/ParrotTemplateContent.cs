using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrotTemplateContent : MonoBehaviour
{
    public GameObject [] BodyTemplates = new GameObject[Constants.TemplateSize];
    public int[,] ColorData = new int [Constants.TemplateSize, 3];
    public void SetUp(List<int> bodyTemplate)
    {
        for(int i = 0; i < Constants.TemplateSize; i++)
        {
            ColorData[i, (int)Constants.ColorType.Cyan] = bodyTemplate[i * 3 + (int)Constants.ColorType.Cyan];
            ColorData[i, (int)Constants.ColorType.Magenta] = bodyTemplate[i * 3 + (int)Constants.ColorType.Magenta];
            ColorData[i, (int)Constants.ColorType.Yellow] = bodyTemplate[i * 3 + (int)Constants.ColorType.Yellow];
        }

        ShowSampleImage();
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

}