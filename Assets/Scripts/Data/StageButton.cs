using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageButton : MonoBehaviour
{
    [SerializeField] private TMP_Text stageNameText;
    [SerializeField] private TMP_Text daysText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedDecoration;
    [SerializeField] private Color selectedTextColor = Color.white;
    [SerializeField] private SceneName gameScene = SceneName.InGameScene;

    private StageDefinition _def;
    private Color _stageNameBaseColor;
    private Color _daysBaseColor;
    private bool _colorsCached;
    private bool _isSubscribed;



    public void Initialize(StageDefinition def)
    {
        _def = def;

        if (_def == null)
        {
            Debug.LogWarning("StageButton: Initialized with a null StageDefinition.");
            return;
        }

        CacheBaseColors();

        if (stageNameText != null)
            stageNameText.text = _def.stageName;

        if (daysText != null)
            daysText.text = $"{Mathf.Max(1, _def.maxDays)}일";

        if (backgroundImage != null && def.backgroundSprite != null)
            backgroundImage.sprite = def.backgroundSprite;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);

        SubscribeStageEvents();
        RefreshVisual(IsCurrentStage(), GetCurrentStageDay());
    }

    private void OnClick()
    {
        if (_def == null)
            return;

        // 1) 선택한 스테이지로 설정
        if (StageManager.Instance != null)
        {
            StageManager.Instance.ForceStage(_def);
        }
        else
        {
            Debug.LogWarning("StageButton: StageManager instance is missing.");
        }

        // 2) GameManager를 통해 InGameScene 로드
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SceneLoad(gameScene);
        }
        else
        {
            Debug.LogWarning("StageButton: GameManager instance is missing.");
        }
    }

    private void OnEnable()
    {
        if (_def == null)
            return;

        SubscribeStageEvents();
        RefreshVisual(IsCurrentStage(), GetCurrentStageDay());
    }

    private void OnDisable()
    {
        UnsubscribeStageEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeStageEvents();
    }

    private void OnStageDayChanged(int stageId, int day)
    {
        if (_def == null)
            return;

        bool isSelected = stageId == _def.stageId;
        RefreshVisual(isSelected, day);
    }

    private void RefreshVisual(bool isSelected, int day)
    {
        CacheBaseColors();

        if (selectedDecoration != null)
            selectedDecoration.SetActive(isSelected);

        if (stageNameText != null)
            stageNameText.color = isSelected ? selectedTextColor : _stageNameBaseColor;

        if (daysText != null)
        {
            daysText.color = isSelected ? selectedTextColor : _daysBaseColor;

            int maxDays = Mathf.Max(1, _def.maxDays);

            if (isSelected)
            {
                int clampedDay = Mathf.Clamp(day, 1, maxDays);
                daysText.text = $"{clampedDay}/{maxDays}일";
            }
            else
            {
                daysText.text = $"{maxDays}일";
            }
        }
    }

    private void SubscribeStageEvents()
    {
        if (_isSubscribed || StageManager.Instance == null)
            return;

        StageManager.Instance.OnStageDayChanged.AddListener(OnStageDayChanged);
        _isSubscribed = true;
    }

    private void UnsubscribeStageEvents()
    {
        if (!_isSubscribed || StageManager.Instance == null)
            return;

        StageManager.Instance.OnStageDayChanged.RemoveListener(OnStageDayChanged);
        _isSubscribed = false;
    }

    private void CacheBaseColors()
    {
        if (_colorsCached)
            return;

        if (stageNameText != null)
            _stageNameBaseColor = stageNameText.color;

        if (daysText != null)
            _daysBaseColor = daysText.color;

        _colorsCached = true;
    }

    private bool IsCurrentStage()
    {
        return StageManager.Instance != null && StageManager.Instance.CurrentStage == _def.stageId;
    }

    private int GetCurrentStageDay()
    {
        if (StageManager.Instance == null)
            return 1;

        return StageManager.Instance.CurrentStage == _def.stageId
            ? StageManager.Instance.CurrentDay
            : 1;
    }
}
