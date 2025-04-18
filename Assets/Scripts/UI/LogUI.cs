using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    [SerializeField] private RectTransform contentRoot; // 로그가 쌓일 Content
    [SerializeField] private GameObject logTextPrefab;  // 프리팹 (TMP_Text)
    [SerializeField] private ScrollRect scrollRect;     // ScrollView의 ScrollRect

    //스테이지 시작 날짜 로그
    public void AddDayLog(int day, string message)
    {
        GameObject logItem = Instantiate(logTextPrefab, contentRoot);

        logItem.transform.GetChild(0).gameObject.SetActive(true); // 이미지 비활성화

        TMP_Text dayText = logItem.transform.Find("Day")?.GetComponent<TMP_Text>();
        if (dayText != null)
            dayText.text = " " + day + "일차";

        TMP_Text messageText = logItem.transform.Find("LogImg/MessageText")?.GetComponent<TMP_Text>();
        if (messageText != null)
            messageText.text = message;


        StartCoroutine(ScrollToBottomNextFrame());
    }

    //일반 로그
    public void AddLog(string message)
    {
        GameObject logItem = Instantiate(logTextPrefab, contentRoot);
        var dayText = logItem.transform.GetChild(0).gameObject;
        var logImage = logItem.transform.GetChild(1).gameObject;

        dayText.gameObject.SetActive(false); // 날짜 비활성화

        TMP_Text messageText = logItem.transform.Find("LogImg/MessageText")?.GetComponent<TMP_Text>();

        var line = messageText.textInfo.lineCount;

        Debug.Log(logImage.GetComponent<RectTransform>().sizeDelta.y);


        var size = logImage.GetComponent<RectTransform>().sizeDelta;
        var sizeY = size.y + ((line) * 50); // 50은 줄 간격
        Debug.Log(sizeY);
        size = new Vector2(size.x, sizeY); // 이미지 크기 조정
        logImage.GetComponent<RectTransform>().sizeDelta = size;

        Debug.Log(logImage.GetComponent<RectTransform>().sizeDelta.y);

        if (messageText != null)
            messageText.text = message;

        StartCoroutine(ScrollToBottomNextFrame());
    }

    private IEnumerator ScrollToBottomNextFrame()
    {
        yield return null; // 레이아웃 업데이트 기다림
        scrollRect.verticalNormalizedPosition = 0f; // 맨 아래로 스크롤
    }
}
