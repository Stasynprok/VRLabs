using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDebugg : MonoBehaviour
{
    public bool Log = false;
    public bool Warning = false;
    public bool Error = false;

    private void OnValidate()
    {
        if (Log)
        {
            StasynDebuger.Message(LogType.Log, "Stasyn", "Stasyn Log");
            Debug.Log("Just log");
            Log = false;
        }
        
        if (Warning)
        {
            StasynDebuger.Message(LogType.Warning, "Stasyn", "Stasyn Warning");
            Debug.LogWarning("Just Warning");
            Warning = false;
        }
        
        if (Error)
        {
            StasynDebuger.Message(LogType.Error, "Stasyn", "Stasyn Error");
            Debug.LogError("Just Error");
            Error = false;
        }
    }
}
