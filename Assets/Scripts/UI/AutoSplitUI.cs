using UnityEngine;

public class AutoSplitUI : MonoBehaviour
{
    [SerializeField] RectTransform panelA;  // Portrait ���, Landscape ����
    [SerializeField] RectTransform panelB;  // Portrait �ϴ�, Landscape ����

    bool lastIsLandscape = false;

    void Start()
    {
        UpdateLayout();
    }

    void Update()
    {
        bool isLandscape = Screen.width > Screen.height;
        if (isLandscape != lastIsLandscape)
            UpdateLayout();
    }

    void UpdateLayout()
    {
        bool isLandscape = Screen.width > Screen.height;

        if (isLandscape)
        {
            // ���� ��� �� ��/�� ����
            SetAnchors(panelA, new Vector2(0f, 0f), new Vector2(0.5f, 1f));  // ����
            SetAnchors(panelB, new Vector2(0.5f, 0f), new Vector2(1f, 1f));  // ����
        }
        else
        {
            // ���� ��� �� ��/�� ����
            SetAnchors(panelA, new Vector2(0f, 0.5f), new Vector2(1f, 1f));  // ���
            SetAnchors(panelB, new Vector2(0f, 0f), new Vector2(1f, 0.5f)); // �ϴ�
        }

        lastIsLandscape = isLandscape;
    }

    void SetAnchors(RectTransform rt, Vector2 min, Vector2 max)
    {
        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
