using TMPro;
using UnityEngine;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI stackText;

    public void Set(Skill skill)
    {
        nameText.text = skill.Name;
        powerText.text = skill.Power + "%";
        stackText.text = skill.Stack > 1 ? $"x{skill.Stack}" : "";
    }

    public void SetUp(Skill skill, System.Action<Skill> onClick)
    {
        Set(skill);
        var button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick(skill));
    }

    public void Clear()
    {
        nameText.text = "";
        powerText.text = "";
        stackText.text = "";
    }
}
