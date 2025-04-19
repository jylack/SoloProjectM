using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillHudUI : MonoBehaviour
{
    [SerializeField] private Transform iconParent; // �������� ��ġ�� �θ�
    [SerializeField] private GameObject iconPrefab; // ������ ������

    private List<GameObject> spawnedIcons = new List<GameObject>();

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        // ���� ������ ����
        foreach (var icon in spawnedIcons)
        {
            Destroy(icon);
        }
        spawnedIcons.Clear();

        // �÷��̾� ��ų ����Ʈ �ҷ�����
        var skills = GameManager.instance.PlayerCurrentSkills;
        foreach (var skill in skills)
        {
            GameObject icon = Instantiate(iconPrefab, iconParent);
            Image iconImage = icon.GetComponent<Image>();
            if (iconImage != null && skill.Icon != null)
                iconImage.sprite = skill.Icon;

            // ���� ����
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
