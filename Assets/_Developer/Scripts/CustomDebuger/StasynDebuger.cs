using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StasynDebuger
{
    public static void Message(LogType type, string tag, string message)
    {
        string logMessage = $"[{tag}] {message}";
        switch (type)
        {
            case LogType.Log:
                Debug.Log(logMessage);
                break;
            case LogType.Error:
                Debug.LogError(logMessage);
                break;
            case LogType.Warning:
                Debug.LogWarning(logMessage);
                break;
            default:
                return;
        }
    }

    public static void Message(string tag, string message)
    {
        string logMessage = $"[{tag}] {message}";
        Debug.Log(logMessage);
    }
}
