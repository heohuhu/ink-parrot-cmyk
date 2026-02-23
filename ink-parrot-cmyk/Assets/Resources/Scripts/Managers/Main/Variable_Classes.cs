using UnityEngine;
using System;
using System.Collections.Generic;
public class Constants: MonoBehaviour
{
    static public Constants Instance;

    void Awake()
    {
        Instance = this;
    }

    public const int TemplateSize = 7;
    public const int BasicParrotsSize = 24; // 기본 앵무새 수
    public enum ColorType { Magenta, Yellow, Cyan }
    public enum TemplateType { Head1, Head2, Head3, Body1, Body2, Wing1, Wing2 };

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
}