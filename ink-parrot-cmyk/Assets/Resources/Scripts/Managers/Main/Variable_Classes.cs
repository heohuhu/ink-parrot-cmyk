using UnityEngine;
using System;
public class Constants: MonoBehaviour
{
    static public Constants Instance;

    void Awake()
    {
        Instance = this;
    }

    public const int TemplateSize = 7;
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
}