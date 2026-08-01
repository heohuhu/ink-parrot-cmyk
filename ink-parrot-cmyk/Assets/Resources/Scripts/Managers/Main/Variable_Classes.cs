using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
public class Constants: MonoBehaviour
{
    static public Constants Instance;

    void Awake()
    {
        Instance = this;
    }

    public const int TemplateSize = 7;
    [SerializeField]
    public int PlayTime = 20;
    public const int BasicParrotsSize = 24; // 기본 앵무새 수
    public enum ColorType { Magenta, Yellow, Cyan };
    public enum TemplateType { Head1, Head2, Head3, Body1, Body2, Wing1, Wing2 };

    // 이 함수를 SettingManager의 함수랑 혼동하지 않도록 주의
    public Color GetColor(ColorType color)
    {
        switch (color)
        {
            case ColorType.Cyan:
                return new Color(0f, 1f, 1f, 1f);
            case ColorType.Magenta:
                return new Color(1f, 0f, 1f, 1f);
            case ColorType.Yellow:
                return new Color(1f, 1f, 0f, 1f);
            default:
                return new Color(1f, 0f, 1f, 1f);
        }
    }

    //컬러 타입별 밝기
    public int GetLightTypeData(int colortype)
    {
        int N = 0;

        switch (colortype)
        {
            case 0:
            N = 0;
            break;

            case 1:
            N = 33;
            break;

            case 2:
            N = 66;
            break;

            case 3:
            N = 100;
            break;
        }

        return N;
    }

    public Color GetColor(Constants.ColorType CMYK, int LightType)
    {
        Color tmp = SettingManager.Instance.GetColor(CMYK);
        float t = Mathf.Clamp01(this.GetLightTypeData(LightType) / 100f);
        tmp = Color.Lerp(Color.white, tmp, t);

        return tmp;
    }

    public int Difficulty_to_Score_Per_Part(string difficulty)
    {
        if(difficulty == "easy")
            return 30;
        else if(difficulty == "normal")
            return 60;
        else if(difficulty == "hard")
            return 80;
        else
            return 0;
    }
    public int Difficulty_to_Score_Complete(string difficulty)
    {
        if(difficulty == "easy")
            return 90;
        else if(difficulty == "normal")
            return 80;
        else if(difficulty == "hard")
            return 140;
        else
            return 0;
    }
}

public static class Utility
{
    private static System.Random _random = new System.Random();

    public static int GetRandomInt(int min, int max)
    {
        return _random.Next(min, max);
    }

    public static List<List<string>> ConvertToList(string[,] array2D)
    {
        int rows = array2D.GetLength(0);
        int cols = array2D.GetLength(1);

        List<List<string>> result = new List<List<string>>(rows);

        for (int i = 0; i < rows; i++)
        {
            List<string> row = new List<string>(cols);

            for (int j = 0; j < cols; j++)
            {
                row.Add(array2D[i, j]);
            }

            result.Add(row);
        }

        return result;
    }

    //3개 색상 조합
    public static Color CombineColor(Color C, Color M, Color Y)
    {
        return new Color(
                C.r * M.r * Y.r,
                C.g * M.g * Y.g,
                C.b * M.b * Y.b,
                1f
            );
    }

    public static void PrintRecursive(object data, int depth)
    {
        if (data == null)
        {
            Debug.Log("null");
            return;
        }

        // 문자열은 IEnumerable이지만 배열처럼 처리하면 안됨
        if (data is string)
        {
            Debug.Log(new string(' ', depth * 2) + data);
            return;
        }

        // List / Array 등 IEnumerable 처리
        if (data is IEnumerable<object> enumerable)
        {
            int index = 0;
            foreach (var item in enumerable)
            {
                Debug.Log(new string(' ', depth * 2) + $"[{index}]");
                PrintRecursive(item, depth + 1);
                index++;
            }
        }
        else
        {
            Debug.Log(new string(' ', depth * 2) + data.ToString());
        }
    }

    public static string GetCurrentDateMMDD()
    {
            return DateTime.Now.ToString("MM/dd");
    }
}