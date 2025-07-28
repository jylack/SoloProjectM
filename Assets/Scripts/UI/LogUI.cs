using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    [SerializeField] private RectTransform contentRoot; // 로그가 쌓일 Content
    [SerializeField] private ScrollRect scrollRect;     // ScrollView의 ScrollRect
    
    private LogPool logPool;

    private void Awake()
    {
        logPool = GetComponent<LogPool>();
    }

    public void AddDayLog(int day, string message)
    {
        var entry = logPool.Get();
        entry.transform.SetParent(contentRoot, false);
        entry.InitializeDay(day, message);
        ScrollToBottom();
    }

    public void AddLog(string message)
    {
        var entry = logPool.Get();
        entry.transform.SetParent(contentRoot, false);
        entry.InitializeLog(message);
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
