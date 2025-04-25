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
        //장착된 장비스텟 합쳐진 스텟반환
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
            //LogUI.Instance.AddLog($"스킬 강화: {newSkill.Name} (스택 {PlayerSkillDict[newSkill.Name].Stack})");
        }
        else
        {
            SkillDict[newSkill.Name] = newSkill.Clone();
            //LogUI.Instance.AddLog($"스킬 획득: {newSkill.Name}");
        }
    }

    //장비 등등 받아서 스텟 업데이트 해줌.
    //어떤 공격할때 확률적으로 크리티컬같은거 뜨면 계산해서 데미지 넘겨줌.
    //스텟을 넘겨주고 애니메이션을 넘겨줌.
    //사용가능한 스킬 리스트 사용


}
