using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtil
{
    public static Color GetGradientColor(Color color1, Color color2, float percent)
    {
        return Color.Lerp(color1, color2, percent);
    }

    public static Color GetRandomGradientColor(Color color1, Color color2)
    {
        int priority = Random.Range(0, 10);

        int randomNumber = Random.Range(0, 100);

        if (priority < 1)
        {
            randomNumber = Random.Range(20, 100);
        }
        else
        {
            randomNumber = Random.Range(0, 20);
        }

        return Color.Lerp(color1, color2, randomNumber / 100f);
    }

    public static int GetRandomNumber()
    {
        int priority = Random.Range(0, 10);

        int randomNumber = Random.Range(0, 100);

        if (priority < 1)
        {
            randomNumber = Random.Range(20, 100);
        }
        else
        {
            randomNumber = Random.Range(0, 20);
        }

        return randomNumber;
    }
}
