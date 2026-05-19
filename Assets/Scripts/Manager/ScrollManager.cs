
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Transform contentTr;
    [SerializeField] private RectTransform[] BtnRect;
    [SerializeField] private RectTransform[] BtnImageRect;

    private TouchControls controls;
    private Vector2 startPos, endPos;
    private bool isDragging;

    private const int SIZE = 5;
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
        targetIndex = 2; // 초기 타겟 인덱스 설정
        targetPos = scrollbar.value; // 초기 타겟 위치 설정

        distance = 1f / (SIZE - 1);
        
        for (int i = 0; i < SIZE; i++)
        {
            pos[i] = distance * i;
        }
        scrollbar.value = 0.5f; // 초기값 설정
    }

    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
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
        if (!isDragging)
        {
            Debug.Log($"[ScrollManager] Smoothly moving scrollbar from {scrollbar.value} to {targetPos}");  
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
        }

        for (int i = 0; i < SIZE; i++)
        {
            Vector3 BtnTargetPos = BtnRect[i].anchoredPosition3D;
            Vector3 BtnTargetScale = Vector3.one;
            bool textActive = false;

            if (i == targetIndex)
            {
                BtnTargetPos.y = 100f;
                BtnTargetScale = new Vector3(1.2f, 1.2f, 1f);
                textActive = true;
                BtnRect[i].localScale = BtnTargetScale;
                BtnImageRect[i].anchoredPosition3D =
                Vector3.Lerp(Vector3.zero,
                            new Vector3(0, BtnTargetPos.y, 0),
                            0.25f);
            }
            else
            {
                BtnTargetPos.y = 0f;
                BtnImageRect[i].anchoredPosition3D = Vector3.zero;
                BtnRect[i].localScale = Vector3.one;
            }


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

