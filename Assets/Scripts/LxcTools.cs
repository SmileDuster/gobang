using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class LxcTools
{
    public static void DisableTextButton(Button button, float alpha, bool enabled)
    {
        button.enabled = enabled;
        DisableText(button.GetComponent<Text>(), alpha);
    }
    
    public static void EnableTextButton(Button button)
    {
        button.enabled = true;
        EnableText(button.GetComponent<Text>());
    }

    public static void DisableText(Text text, float alpha)
    {
        var color = text.color;
        color.a = alpha;
        text.color = color;
    }
    
    public static void EnableText(Text text)
    {
        var color = text.color;
        color.a = 1;
        text.color = color;
    }

    public static void EnableImage(Image image)
    {
        var color = image.color;
        color.a = 1;
        image.color = color;
    }
    
}
