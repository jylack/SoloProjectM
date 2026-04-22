using TMPro;
using UnityEngine;

public class StateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LvlText;
    [SerializeField] private TextMeshProUGUI HpText;
    [SerializeField] private TextMeshProUGUI AtkText;
    [SerializeField] private TextMeshProUGUI DefText;

    private void OnEnable()
    {
        if (RunManager.Instance != null)
        {
            RunManager.Instance.StateChanged += Refresh;
        }

        Refresh();
    }

    private void OnDisable()
    {
        if (RunManager.Instance != null)
        {
            RunManager.Instance.StateChanged -= Refresh;
        }
    }

    private void Refresh()
    {
        var runManager = RunManager.Instance;
        var profile = GameManager.Instance != null ? GameManager.Instance.PlayerProfile : null;

        if (runManager != null && runManager.RunState != null && runManager.RunState.stats != null)
        {
            var stats = runManager.RunState.stats;
            SetText(LvlText, "LV." + Mathf.Max(1, profile != null && profile.stats != null ? profile.stats.level : 1));
            SetText(HpText, stats.currentHp + "/" + stats.maxHp);
            SetText(AtkText, stats.attack.ToString());
            SetText(DefText, stats.defense.ToString());
            return;
        }

        if (profile != null && profile.stats != null)
        {
            SetText(LvlText, "LV." + Mathf.Max(1, profile.stats.level));
            SetText(HpText, profile.stats.maxHp + "/" + profile.stats.maxHp);
            SetText(AtkText, Mathf.Max(1, profile.stats.attack).ToString());
            SetText(DefText, Mathf.Max(0, profile.stats.defense).ToString());
            return;
        }

        SetText(LvlText, "LV.-");
        SetText(HpText, "-/ -");
        SetText(AtkText, "-");
        SetText(DefText, "-");
    }

    private static void SetText(TMP_Text text, string value)
    {
        if (text != null)
        {
            text.text = value;
        }
    }
}
