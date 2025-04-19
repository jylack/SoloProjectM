using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTooltipUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Image iconImage;

    public static SkillTooltipUI Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Hide();
    }

    public void Show(string name, string description)
    {
        nameText.text = name;
        descText.text = description;
        root.SetActive(true);
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}
