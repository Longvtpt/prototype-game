using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogSystem
{
    public static void LogByColor(string text, string color)
    {
        Debug.Log($"<color={color}>" + text + "</color>");
    }

    public static void LogWarning(string text)
    {
        LogByColor(text, "yellow");
    }

    public static void LogError(string text)
    {
        LogByColor(text, "red");
    }

    public static void LogSuccess(string text)
    {
        LogByColor(text, "green");
    }
}
