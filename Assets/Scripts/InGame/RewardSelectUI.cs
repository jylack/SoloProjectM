using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject rootPanel;
    [SerializeField] private Text titleText;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private Text[] optionTexts;

    private Action<RewardData> _onSelected;
    private List<RewardData> _currentRewards;

    private void Awake()
    {
        EnsureRuntimeUI();
        Hide();
    }

    public void Show(List<RewardData> rewards, Action<RewardData> onSelected)
    {
        _currentRewards = rewards;
        _onSelected = onSelected;

        if (titleText != null)
        {
            titleText.text = "Choose Reward";
        }

        if (rootPanel != null)
        {
            rootPanel.SetActive(true);
        }

        for (int i = 0; i < optionButtons.Length; i++)
        {
            var button = optionButtons[i];
            var text = i < optionTexts.Length ? optionTexts[i] : null;
            if (button == null)
            {
                continue;
            }

            int index = i;
            button.onClick.RemoveAllListeners();

            bool hasReward = rewards != null && index < rewards.Count;
            button.gameObject.SetActive(hasReward);
            button.interactable = hasReward;

            if (text != null)
            {
                text.text = hasReward ? BuildRewardLabel(rewards[index]) : string.Empty;
            }

            if (hasReward)
            {
                button.onClick.AddListener(delegate { Select(index); });
            }
        }
    }

    public void Hide()
    {
        if (rootPanel != null)
        {
            rootPanel.SetActive(false);
        }
    }

    public static string BuildRewardLabel(RewardData reward)
    {
        if (reward == null)
        {
            return "None";
        }

        switch (reward.rewardType)
        {
            case RewardType.Heal:
                return "Heal +" + reward.amount;
            case RewardType.Gold:
                return "Gold +" + reward.amount;
            case RewardType.Stat:
                return reward.statType + " +" + reward.amount;
            case RewardType.Skill:
                return "Skill " + reward.skillId;
            case RewardType.Item:
                return "Item " + reward.itemId;
            case RewardType.Exp:
                return "Exp +" + reward.amount;
            default:
                return reward.rewardType.ToString();
        }
    }

    private void Select(int index)
    {
        if (_currentRewards == null || index < 0 || index >= _currentRewards.Count)
        {
            return;
        }

        var selected = _currentRewards[index];
        Hide();
        _onSelected?.Invoke(selected);
    }

    private void EnsureRuntimeUI()
    {
        if (optionButtons != null && optionButtons.Length > 0 && rootPanel != null)
        {
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var canvasObject = new GameObject("InGameCanvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        rootPanel = new GameObject("RewardSelectPanel");
        rootPanel.transform.SetParent(canvas.transform, false);
        var layout = rootPanel.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = true;
        layout.childControlWidth = true;

        titleText = CreateText("RewardTitle", rootPanel.transform, "Choose Reward");
        optionButtons = new Button[3];
        optionTexts = new Text[3];

        for (int i = 0; i < 3; i++)
        {
            var buttonObject = new GameObject("RewardOption" + i);
            buttonObject.transform.SetParent(rootPanel.transform, false);
            buttonObject.AddComponent<Image>();
            var button = buttonObject.AddComponent<Button>();
            var label = CreateText("Label", buttonObject.transform, string.Empty);
            label.alignment = TextAnchor.MiddleCenter;
            optionButtons[i] = button;
            optionTexts[i] = label;
        }
    }

    private Text CreateText(string objectName, Transform parent, string value)
    {
        var textObject = new GameObject(objectName);
        textObject.transform.SetParent(parent, false);
        var text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.color = Color.white;
        text.fontSize = 24;
        text.text = value;
        return text;
    }
}
