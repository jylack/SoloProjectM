using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    UnitStats stats = new UnitStats("User", 1000, 10, 0, 5, 1);
    UnitStats applyStats = new UnitStats("", 0, 0, 0, 0, 0);

    public BuffSystem Buffs = new BuffSystem();

    public Dictionary<string, Skill> SkillDict = new();
    public List<Skill> Skills => new List<Skill>(SkillDict.Values);
    public List<Skill> CurrentSkills => new List<Skill>(SkillDict.Values);

    private void Start()
    {
        //������ ����� ������ ���ݹ�ȯ
        GameManager.instance.SetPlayerBaseStats(GetStats());
    }

    public UnitStats GetStats()
    {
        return new UnitStats(
            stats.Name,
            stats.MaxHp + applyStats.MaxHp,
            stats.Attack + applyStats.Attack,
            stats.Defense + applyStats.Defense,
            stats.AttackCount + applyStats.AttackCount,
            stats.Speed + applyStats.Speed
            );
    }

    public UnitStats GetOriginState()
    {
        return stats;
    }

    public UnitStats GetApplyStats()
    {
        return applyStats;
    }

    public void AddSkill(Skill newSkill)
    {
        if (SkillDict.ContainsKey(newSkill.Name))
        {
            SkillDict[newSkill.Name].ApplyStack(newSkill);
            //LogUI.Instance.AddLog($"��ų ��ȭ: {newSkill.Name} (���� {PlayerSkillDict[newSkill.Name].Stack})");
        }
        else
        {
            SkillDict[newSkill.Name] = newSkill.Clone();
            //LogUI.Instance.AddLog($"��ų ȹ��: {newSkill.Name}");
        }
    }

    //��� ��� �޾Ƽ� ���� ������Ʈ ����.
    //� �����Ҷ� Ȯ�������� ũ��Ƽ�ð����� �߸� ����ؼ� ������ �Ѱ���.
    //������ �Ѱ��ְ� �ִϸ��̼��� �Ѱ���.
    //��밡���� ��ų ����Ʈ ���


}
