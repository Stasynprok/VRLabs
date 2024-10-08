using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TesterEvents : XRBaseControllerInteractor
{
   
    public void DebugString(string word)
    {
        Debug.LogError(word);
    }
}
