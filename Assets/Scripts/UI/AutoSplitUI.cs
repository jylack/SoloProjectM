using UnityEngine;

public class AutoSplitUI : MonoBehaviour
{
    [SerializeField] RectTransform panelA;  // Portrait 상단, Landscape 좌측
    [SerializeField] RectTransform panelB;  // Portrait 하단, Landscape 우측

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
            // 가로 모드 → 좌/우 절반
            SetAnchors(panelA, new Vector2(0f, 0f), new Vector2(0.5f, 1f));  // 좌측
            SetAnchors(panelB, new Vector2(0.5f, 0f), new Vector2(1f, 1f));  // 우측
        }
        else
        {
            // 세로 모드 → 상/하 절반
            SetAnchors(panelA, new Vector2(0f, 0.5f), new Vector2(1f, 1f));  // 상단
            SetAnchors(panelB, new Vector2(0f, 0f), new Vector2(1f, 0.5f)); // 하단
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
