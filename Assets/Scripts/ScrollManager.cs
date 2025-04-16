
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Transform contentTr;
    [SerializeField] private Slider tabSlider;
    [SerializeField] private RectTransform[] BtnRect;
    [SerializeField] private RectTransform[] BtnImageRect;

    private TouchControls controls;
    private Vector2 startPos, endPos;
    private bool isTouching;
    private bool isDragging;

    private const int SIZE = 4;
    private float[] pos = new float[SIZE];
    private float distance;
    private float targetPos;
    private int targetIndex;

    private void Awake()
    {
        controls = new TouchControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Touch.PrimaryContact.started += OnTouchStart;
        controls.Touch.PrimaryContact.canceled += OnTouchEnd;
    }

    private void OnDisable()
    {
        controls.Touch.PrimaryContact.started -= OnTouchStart;
        controls.Touch.PrimaryContact.canceled -= OnTouchEnd;
        controls.Disable();
    }

    private void Start()
    {
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;
    }

    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
        isTouching = true;
        isDragging = true;
        startPos = controls.Touch.PrimaryPosition.ReadValue<Vector2>();
        targetPos = GetNearestPos(scrollbar.value);
    }

    private void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        isDragging = false;
        endPos = controls.Touch.PrimaryPosition.ReadValue<Vector2>();

        Vector2 delta = endPos - startPos;
        float cur = targetPos;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            // 수평 드래그 판단
            if (delta.x > 18 && cur - distance >= 0) --targetIndex;
            else if (delta.x < -18 && cur + distance <= 1.01f) ++targetIndex;

            targetPos = pos[targetIndex];
        }

        for (int i = 0; i < SIZE; i++)
        {
            if (contentTr.GetChild(i).TryGetComponent(out Scrollbar sb) && cur != pos[i] && targetPos == pos[i])
                sb.value = 1;
        }

        isTouching = false;
    }

    private float GetNearestPos(float val)
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (val < pos[i] + distance * 0.5f && val > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        }
        return val;
    }

    private void Update()
    {
        tabSlider.value = scrollbar.value;

        if (!isDragging)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);

            for (int i = 0; i < SIZE; i++)
            {
                BtnRect[i].sizeDelta = new Vector2(i == targetIndex ? 360 : 180, BtnRect[i].sizeDelta.y);
            }
        }

        for (int i = 0; i < SIZE; i++)
        {
            Vector3 BtnTargetPos = BtnRect[i].anchoredPosition3D;
            Vector3 BtnTargetScale = Vector3.one;
            bool textActive = false;

            if (i == targetIndex)
            {
                BtnTargetPos.y = -23f;
                BtnTargetScale = new Vector3(1.2f, 1.2f, 1f);
                textActive = true;
            }

            BtnImageRect[i].anchoredPosition3D = Vector3.Lerp(BtnImageRect[i].anchoredPosition3D, BtnTargetPos, 0.25f);
            BtnImageRect[i].localScale = Vector3.Lerp(BtnImageRect[i].localScale, BtnTargetScale, 0.25f);
            BtnImageRect[i].transform.GetChild(0).gameObject.SetActive(textActive);
        }
    }

    public void TabClick(int n)
    {
        targetIndex = n;
        targetPos = pos[n];
    }
}

