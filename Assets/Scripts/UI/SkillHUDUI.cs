using System.Collections.Generic;
using UnityEngine;

public class SkillHudUI : MonoBehaviour
{
    [SerializeField] private SkillSlotUI[] slots;
    //옵션창열었을때 보여줄 패널
    [SerializeField] private GameObject rootPanel;
    //스테이지 내에서 일상적으로 보여질 패널
    [SerializeField] private GameObject stagePanel;

    public static SkillHudUI Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowSkills(List<Skill> skillList)
    {
        if(rootPanel.activeSelf == true)
        {
            rootPanel.SetActive(false);
            return;
        }
        

        rootPanel.SetActive(true);
        
        Debug.Log(skillList.Count);

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < skillList.Count)
                slots[i].Set(skillList[i]);
            else
                slots[i].Clear();
        }
    }

    public void Hide()
    {
        rootPanel.SetActive(false);
    }
}
