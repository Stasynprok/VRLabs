using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotepadController : MonoBehaviour
{
    private DebugerController debugerController;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private ScrollRect _scrollView;

    private void Awake()
    {
        debugerController = DebugerController.GetInstance();
        debugerController.NewLogMessage += OnNewMessage;
    }

    private void OnNewMessage(DebugInfo debugInfo)
    {
        string newStringDebug = $"{debugInfo.GetLogMessage().LogMessage}\r\n";

        StartCoroutine(ScrollToBottom());
        _text.text += newStringDebug;
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        _scrollView.verticalNormalizedPosition = 0;
    }

    private void ReloadDebugTextOnEnable()
    {

    }
}
