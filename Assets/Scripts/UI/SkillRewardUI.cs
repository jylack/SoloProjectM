using System.Collections.Generic;
using UnityEngine;

public class SkillRewardUI : MonoBehaviour
{
    [SerializeField] private SkillSlotUI[] rewardSlots;
    [SerializeField] private GameObject rootPanel;

    private List<Skill> rewards = new List<Skill>();

    public static SkillRewardUI Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        rootPanel.SetActive(false);
    }

    public void SetUp(RewardCondition reward)
    {

    }


public void Open(List<Skill> rewardList)
    {
        rewards = rewardList;
        rootPanel.SetActive(true);

        for (int i = 0; i < rewardSlots.Length; i++)
        {
            if (i < rewards.Count)
            {
                var skill = rewards[i];
                rewardSlots[i].Set(skill);
                int index = i;
                rewardSlots[i].GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
                rewardSlots[i].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnSelect(skill));
            }
            else
            {
                rewardSlots[i].Clear();
                rewardSlots[i].GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
            }
        }
    }

    private void OnSelect(Skill selected)
    {
        GameManager.instance.AddSkill(selected);
//        LogUI.Instance.AddLog($"스킬 보상 선택: {selected.Name}");
        rootPanel.SetActive(false);
    }
}
