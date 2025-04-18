using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class SkillDB
{
    public static List<Skill> AllSkills = new List<Skill>
    {
        CreateSkill("일반 공격 번개", "일반 공격 시 일정 확률로 번개 2개 발동 가능.", SkillType.Passive, SkillGrade.Normal),
        CreateSkill("일반 공격 번개+", "일반 공격 시 일정 확률로 번개 2개 발동 가능. (확률 ↑↑)", SkillType.Passive, SkillGrade.Normal),
        CreateSkill("연타 마스터리", "연타 확률 ↑↑, 연타 시 공격력 ↑, 중첩 가능하며 전투 종료될 때까지 지속.", SkillType.Passive, SkillGrade.Legendary),
        CreateSkill("치명타 마스터리", "치명타 확률 ↑↑, 피해 ↑, 중첩 가능.", SkillType.Passive, SkillGrade.Legendary),
        CreateSkill("분노 마스터리", "분노 스킬 대미지 +100%, 1회 시전할 때마다 분노 스킬 -25%.", SkillType.Passive, SkillGrade.Legendary),
        CreateSkill("수리검 마스터리", "수리검 대미지 +60%. 대미지 ↓.", SkillType.Passive, SkillGrade.Legendary),
        CreateSkill("폭군", "공격력 ↑↑↑↑, HP 최대치 ↓↓.", SkillType.Passive, SkillGrade.Legendary),
        CreateSkill("방어 끝판왕", "턴마다 대미지 감소 ↑.", SkillType.Passive, SkillGrade.Mythic),
        CreateSkill("용맹 끝판왕", "턴마다 공격력 상승.", SkillType.Passive, SkillGrade.Mythic),
        CreateSkill("슈퍼 공격력", "공격력 ↑↑↑↑↑.", SkillType.Passive, SkillGrade.Mythic),
        CreateSkill("슈퍼 생명력", "HP ↑↑↑↑↑, HP 최대치까지 회복.", SkillType.Passive, SkillGrade.Mythic),
        CreateSkill("천사의 힘[C]", "전투 시작 시, HP ↑↑↑↑↑↑↑↑↑↑. 천사의 보호를 획득하며 6턴 지속.", SkillType.Passive, SkillGrade.Immortal),
        CreateSkill("악마의 힘[C]", "HP 처음으로 50% 미만일 경우 공격력 ↑↑↑↑↑↑↑↑↑↑. 악마의 보호를 획득하며 전투 종료까지 지속.", SkillType.Passive, SkillGrade.Immortal)
    };

    private static Skill CreateSkill(string name, string desc, SkillType type, SkillGrade grade)
    {
        int power = CalculatePowerFromArrows(desc);
        return new Skill(name, desc, power, type, grade);
    }

    private static int CalculatePowerFromArrows(string text)
    {
        int total = 0;
        var matches = Regex.Matches(text, "[↑↓]+");

        foreach (Match match in matches)
        {
            int factor = match.Value.Contains("↑") ? 10 : -10;
            total += factor * match.Value.Length;
        }

        return total;
    }

    public static List<Skill> GetRandomSkills(int count)
    {
        var shuffled = new List<Skill>(AllSkills);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[rnd]) = (shuffled[rnd], shuffled[i]);
        }
        return shuffled.GetRange(0, count);
    }
}
