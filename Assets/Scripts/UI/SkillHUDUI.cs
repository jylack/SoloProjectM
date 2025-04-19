using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillHudUI : MonoBehaviour
{
    [SerializeField] private Transform iconParent; // 아이콘을 배치할 부모
    [SerializeField] private GameObject iconPrefab; // 아이콘 프리팹

    private List<GameObject> spawnedIcons = new List<GameObject>();

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        // 기존 아이콘 제거
        foreach (var icon in spawnedIcons)
        {
            Destroy(icon);
        }
        spawnedIcons.Clear();

        // 플레이어 스킬 리스트 불러오기
        var skills = GameManager.instance.PlayerCurrentSkills;
        foreach (var skill in skills)
        {
            GameObject icon = Instantiate(iconPrefab, iconParent);
            Image iconImage = icon.GetComponent<Image>();
            if (iconImage != null && skill.Icon != null)
                iconImage.sprite = skill.Icon;

            // 툴팁 연결
            var btn = icon.GetComponent<Button>();
            if (btn != null)
            {
                string name = skill.Name;
                string desc = skill.Description;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    //SkillTooltipUI.Instance.Show(name, desc);
                });
            }

            spawnedIcons.Add(icon);
        }
    }
}
