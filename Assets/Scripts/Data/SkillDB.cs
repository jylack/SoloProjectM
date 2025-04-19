using System.Collections.Generic;
using UnityEngine;

public static class SkillDB
{
    public static List<Skill> AllSkills = new List<Skill>();

    public static void InitDB()
    {
        AllSkills.Clear();

        Sprite boltIcon = Resources.Load<Sprite>("SkillIcons/bolt");
        Sprite masteryIcon = Resources.Load<Sprite>("SkillIcons/mastery");
        Sprite mythIcon = Resources.Load<Sprite>("SkillIcons/myth");
        Sprite angelIcon = Resources.Load<Sprite>("SkillIcons/angel");
        Sprite demonIcon = Resources.Load<Sprite>("SkillIcons/demon");

        // 기존 스킬들 아이콘 포함 버전으로 재등록
        AllSkills.Add(new Skill("일반 공격 번개", "일반 공격 시 일정 확률로 번개 2개 발동 가능.", 15, SkillType.Passive, SkillGrade.Normal, boltIcon));
        AllSkills.Add(new Skill("일반 공격 번개+", "일반 공격 시 일정 확률로 번개 2개 발동 가능. (확률 ↑↑)", 20, SkillType.Passive, SkillGrade.Normal, boltIcon));
        AllSkills.Add(new Skill("연타 마스터리", "연타 확률 ↑↑, 연타 시 공격력 ↑, 중첩 가능하며 전투 종료될 때까지 지속.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("치명타 마스터리", "치명타 확률 ↑↑, 피해 ↑, 중첩 가능.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("분노 마스터리", "분노 스킬 대미지 +100%, 1회 시전할 때마다 분노 스킬 -25%.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("수리검 마스터리", "수리검 대미지 +60%. 대미지 ↓.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("폭군", "공격력 ↑↑↑↑, HP 최대치 ↓↓.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("방어 끝판왕", "턴마다 대미지 감소 ↑.", 30, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("용맹 끝판왕", "턴마다 공격력 상승.", 30, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("슈퍼 공격력", "공격력 ↑↑↑↑↑.", 35, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("슈퍼 생명력", "HP ↑↑↑↑↑, HP 최대치까지 회복.", 35, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("천사의 힘[C]", "전투 시작 시, HP ↑↑↑↑↑↑↑↑↑↑. 천사의 보호를 획득하며 6턴 지속.", 40, SkillType.Passive, SkillGrade.Immortal, angelIcon));
        AllSkills.Add(new Skill("악마의 힘[C]", "HP 처음으로 50% 미만일 경우 공격력 ↑↑↑↑↑↑↑↑↑↑. 악마의 보호를 획득하며 전투 종료까지 지속.", 40, SkillType.Passive, SkillGrade.Immortal, demonIcon));
    }

    public static List<Skill> GetRandomSkills(int count)
    {
        if (AllSkills.Count == 0) InitDB();

        List<Skill> temp = new List<Skill>(AllSkills);
        List<Skill> result = new List<Skill>();

        for (int i = 0; i < count && temp.Count > 0; i++)
        {
            int rand = Random.Range(0, temp.Count);
            result.Add(temp[rand].Clone());
            temp.RemoveAt(rand);
        }

        return result;
    }
}
