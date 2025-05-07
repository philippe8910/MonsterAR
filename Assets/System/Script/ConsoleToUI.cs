using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConsoleToUI : MonoBehaviour
{
    public Text consoleText; // UI Text 元件
    public ScrollRect scrollRect; // ScrollRect 用於自動滾動
    public int maxMessages = 15; // 記錄的最大訊息數

    private Queue<string> messageQueue = new Queue<string>(); // 訊息隊列

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string message = $"{type}: {logString}";

        if (type == LogType.Exception) // 捕獲錯誤訊息
        {
            message += $"\n{stackTrace}";
        }

        messageQueue.Enqueue(message);

        if (messageQueue.Count > maxMessages)
        {
            messageQueue.Dequeue(); // 移除最舊的訊息
        }

        UpdateConsoleText();
        ScrollToBottom(); // 自動捲動到底部
    }

    void UpdateConsoleText()
    {
        if (consoleText != null)
        {
            consoleText.text = string.Join("\n", messageQueue);
        }
    }

    void ScrollToBottom()
    {
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases(); // 確保 ScrollRect 更新
            scrollRect.verticalNormalizedPosition = 0f; // 滾動到底部
        }
    }

    public void ClearConsole()
    {
        messageQueue.Clear();
        UpdateConsoleText();
        ScrollToBottom();
    }
}