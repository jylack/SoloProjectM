using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    [SerializeField] private RectTransform contentRoot; // �αװ� ���� Content
    [SerializeField] private GameObject logTextPrefab;  // ������ (TMP_Text)
    [SerializeField] private ScrollRect scrollRect;     // ScrollView�� ScrollRect

    //�������� ���� ��¥ �α�
    public void AddDayLog(int day, string message)
    {
        GameObject logItem = Instantiate(logTextPrefab, contentRoot);

        logItem.transform.GetChild(0).gameObject.SetActive(true); // �̹��� ��Ȱ��ȭ

        TMP_Text dayText = logItem.transform.Find("Day")?.GetComponent<TMP_Text>();
        if (dayText != null)
            dayText.text = " " + day + "����";

        TMP_Text messageText = logItem.transform.Find("LogImg/MessageText")?.GetComponent<TMP_Text>();
        if (messageText != null)
            messageText.text = message;


        StartCoroutine(ScrollToBottomNextFrame());
    }

    //�Ϲ� �α�
    public void AddLog(string message)
    {
        GameObject logItem = Instantiate(logTextPrefab, contentRoot);
        var dayText = logItem.transform.GetChild(0).gameObject;
        var logImage = logItem.transform.GetChild(1).gameObject;

        dayText.gameObject.SetActive(false); // ��¥ ��Ȱ��ȭ

        TMP_Text messageText = logItem.transform.Find("LogImg/MessageText")?.GetComponent<TMP_Text>();

        var line = messageText.textInfo.lineCount;

        Debug.Log(logImage.GetComponent<RectTransform>().sizeDelta.y);


        var size = logImage.GetComponent<RectTransform>().sizeDelta;
        var sizeY = size.y + ((line) * 50); // 50�� �� ����
        Debug.Log(sizeY);
        size = new Vector2(size.x, sizeY); // �̹��� ũ�� ����
        logImage.GetComponent<RectTransform>().sizeDelta = size;

        Debug.Log(logImage.GetComponent<RectTransform>().sizeDelta.y);

        if (messageText != null)
            messageText.text = message;

        StartCoroutine(ScrollToBottomNextFrame());
    }

    private IEnumerator ScrollToBottomNextFrame()
    {
        yield return null; // ���̾ƿ� ������Ʈ ��ٸ�
        scrollRect.verticalNormalizedPosition = 0f; // �� �Ʒ��� ��ũ��
    }
}
