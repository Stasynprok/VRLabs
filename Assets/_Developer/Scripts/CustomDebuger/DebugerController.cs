using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugerController
{
    public List<DebugInfo> DebugInfos = new List<DebugInfo>();
    public Action<DebugInfo> NewLogMessage;
    private static DebugerController instance;
    public static DebugerController GetInstance()
    {
        if (instance == null)
        {
            instance = new DebugerController();
        }

        return instance;
    }

    public DebugerController()
    {
        Initialize();
    }
    ~DebugerController()
    {
        Unnitialize();
    }

    private void Initialize()
    {
        Application.logMessageReceived += OnNewDebugMessage;
    }
    private void Unnitialize()
    {
        Application.logMessageReceived -= OnNewDebugMessage;
    }

    private void AddNewDebugMessage(string logMessage, LogType logType)
    {
        DebugInfo debugInfo = new DebugInfo(logMessage, logType);
        NewLogMessage.Invoke(debugInfo);
        DebugInfos.Add(debugInfo);
    }

    private void OnNewDebugMessage(string logString, string stackTrace, LogType type)
    {
        AddNewDebugMessage(logString, type);
    }
}

public class DebugInfo
{
    private string _time;
    public string LogMessage;
    public LogType LogType;

    public DebugInfo(string logMessage, LogType logType)
    {
        _time = DateTime.Now.ToString("hh:mm:ss");
        LogMessage = logMessage;
        LogType = logType;
    }

    public MessageInfo GetLogMessage()
    {
        string logMessage = $"[{_time}] {LogType} {LogMessage}";
        MessageInfo message = new MessageInfo(LogType, logMessage);
        return message;
    }
}

public class MessageInfo
{
    public LogType LogType;
    public string LogMessage;

    public MessageInfo(LogType type, string logMessage)
    {
        LogType = type;
        LogMessage = logMessage;
    }
}
