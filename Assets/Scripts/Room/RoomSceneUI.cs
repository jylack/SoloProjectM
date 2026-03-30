using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RoomSceneUI : MonoBehaviour
{
    [SerializeField] private RoomFlowController roomFlowController;
    [SerializeField] private Text dayText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text attackText;
    [SerializeField] private Text encounterTypeText;
    [SerializeField] private Text encounterRewardText;
    [SerializeField] private Button enterButton;
    [SerializeField] private Button rerollButton;

    private void OnEnable()
    {
        EnsureRuntimeUI();

        if (enterButton != null)
        {
            enterButton.onClick.AddListener(OnEnterClicked);
        }

        if (rerollButton != null)
        {
            rerollButton.onClick.AddListener(OnRerollClicked);
            rerollButton.interactable = false;
        }

        if (RunManager.Instance != null)
        {
            RunManager.Instance.StateChanged += Refresh;
        }

        Refresh();
    }

    private void EnsureRuntimeUI()
    {
        if (roomFlowController == null)
        {
            roomFlowController = FindObjectOfType<RoomFlowController>();
        }

        if (dayText != null && enterButton != null)
        {
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var canvasObject = new GameObject("RunFlowCanvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        var panelObject = new GameObject("RunHudPanel");
        panelObject.transform.SetParent(canvas.transform, false);
        var panel = panelObject.AddComponent<VerticalLayoutGroup>();
        panel.childControlHeight = true;
        panel.childControlWidth = true;
        panel.childForceExpandHeight = false;
        panel.childForceExpandWidth = true;

        dayText = CreateText("DayText", panelObject.transform);
        hpText = CreateText("HpText", panelObject.transform);
        goldText = CreateText("GoldText", panelObject.transform);
        attackText = CreateText("AttackText", panelObject.transform);
        encounterTypeText = CreateText("EncounterTypeText", panelObject.transform);
        encounterRewardText = CreateText("EncounterRewardText", panelObject.transform);

        enterButton = CreateButton("EnterButton", "Enter Encounter", panelObject.transform);
        rerollButton = CreateButton("RerollButton", "Reroll Disabled", panelObject.transform);
    }

    private Text CreateText(string objectName, Transform parent)
    {
        var textObject = new GameObject(objectName);
        textObject.transform.SetParent(parent, false);
        var text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.color = Color.white;
        text.fontSize = 24;
        return text;
    }

    private Button CreateButton(string objectName, string label, Transform parent)
    {
        var buttonObject = new GameObject(objectName);
        buttonObject.transform.SetParent(parent, false);
        var image = buttonObject.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        var button = buttonObject.AddComponent<Button>();

        var labelText = CreateText(objectName + "_Label", buttonObject.transform);
        labelText.alignment = TextAnchor.MiddleCenter;
        labelText.text = label;

        var rect = buttonObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(320f, 50f);

        return button;
    }

    private void OnDisable()
    {
        if (enterButton != null)
        {
            enterButton.onClick.RemoveListener(OnEnterClicked);
        }

        if (rerollButton != null)
        {
            rerollButton.onClick.RemoveListener(OnRerollClicked);
        }

        if (RunManager.Instance != null)
        {
            RunManager.Instance.StateChanged -= Refresh;
        }
    }

    private void OnEnterClicked()
    {
        if (roomFlowController != null)
        {
            roomFlowController.EnterEncounter();
        }
    }

    private void OnRerollClicked()
    {
        if (roomFlowController != null)
        {
            roomFlowController.RegenerateNextEncounter();
        }
    }

    private void Refresh()
    {
        var runManager = RunManager.Instance;
        if (runManager == null || runManager.RunState == null)
        {
            return;
        }

        var stats = runManager.RunState.stats;
        var encounter = roomFlowController != null ? roomFlowController.NextEncounter : runManager.CurrentEncounter;

        if (dayText != null)
        {
            dayText.text = "Day: " + runManager.RunState.currentDay;
        }

        if (hpText != null)
        {
            hpText.text = "HP: " + stats.currentHp + " / " + stats.maxHp;
        }

        if (goldText != null)
        {
            goldText.text = "Gold: " + runManager.RunState.currentGold;
        }

        if (attackText != null)
        {
            attackText.text = "Attack: " + stats.attack;
        }

        if (encounterTypeText != null)
        {
            encounterTypeText.text = encounter != null ? "Encounter: " + encounter.encounterType : "Encounter: None";
        }

        if (encounterRewardText != null)
        {
            encounterRewardText.text = BuildRewardPreview(encounter);
        }

        if (enterButton != null)
        {
            enterButton.interactable = encounter != null;
        }
    }

    private string BuildRewardPreview(EncounterData encounter)
    {
        if (encounter == null || encounter.rewards == null || encounter.rewards.Count == 0)
        {
            return "Reward: None";
        }

        var builder = new StringBuilder();
        builder.Append("Reward: ");

        for (int i = 0; i < encounter.rewards.Count; i++)
        {
            if (i > 0)
            {
                builder.Append(" | ");
            }

            builder.Append(RewardSelectUI.BuildRewardLabel(encounter.rewards[i]));
        }

        return builder.ToString();
    }
}
