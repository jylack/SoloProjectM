using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogEntry : MonoBehaviour
{
    [SerializeField] TMP_Text dayText;
    [SerializeField] TMP_Text messageText;
    [SerializeField] LayoutElement layoutElement;

    private const float padding = 16f;  // 위아래 여유 공간

    /// <summary>
    /// 날짜 로그 초기화
    /// </summary>
    public void InitializeDay(int day, string msg)
    {
        dayText.gameObject.SetActive(true);
        dayText.text = $"{day}일차";
        SetupMessage(msg);
    }

    /// <summary>
    /// 일반 로그 초기화
    /// </summary>
    public void InitializeLog(string msg)
    {
        dayText.gameObject.SetActive(false);
        SetupMessage(msg);
    }

    
    private void SetupMessage(string message)
    {
        // 1) 텍스트 세팅
        messageText.text = message;

        // 2) 레이아웃 값이 갱신되도록 강제
        Canvas.ForceUpdateCanvases();

        // 3) 텍스트의 preferredHeight 에 패딩을 더해서 말풍선 높이 지정
        //layoutElement.minHeight = messageText.preferredHeight + padding;
    }

}
