using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionUI : MonoBehaviour
{
    [SerializeField] private SkillSlotUI[] skillSlots;
    [SerializeField] private GameObject rootPanel;

    

    private List<Skill> currentSkills = new List<Skill>();

    public static SkillSelectionUI Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        rootPanel.SetActive(false);


        Open(SkillDB.GetRandomSkills(3));
    }

    public void Open(List<Skill> skillOptions)
    {
        currentSkills = skillOptions;
        rootPanel.SetActive(true);

        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (i < currentSkills.Count)
                skillSlots[i].SetUp(currentSkills[i], OnSkillSelected);
            else
                skillSlots[i].gameObject.SetActive(false);
        }
    }

    private void OnSkillSelected(Skill selected)
    {
        GameManager.instance.player.Skills.Add(selected);
        //LogUI.Instance.AddLog($"{selected.Name} ½ºÅ³À» È¹µæÇß½À´Ï´Ù!");
        rootPanel.SetActive(false);
        GameManager.instance.OnInGameEnd();
    }
}
