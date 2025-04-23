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

        // 히트 기반 회복 스킬
        AllSkills.Add(new Skill("히트 회복", "히트가 3회 누적되면 HP를 3% 회복합니다.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));
        AllSkills.Add(new Skill("히트 회복+", "히트가 2회 누적되면 HP를 5% 회복합니다.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));

        // 조건부 HP 회복 (HP 30% 이하)
        AllSkills.Add(new Skill("빈사 회복", "HP가 처음으로 30% 미만이 되면 3턴 동안 매 턴마다 10% 회복됩니다.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));
        AllSkills.Add(new Skill("빈사 회복+", "HP가 처음으로 30% 미만이 되면 3턴 동안 매 턴마다 15% 회복됩니다.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));

        // HP 회복 증가
        AllSkills.Add(new Skill("HP 회복 증가", "HP 회복량이 +2% 증가합니다.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));
        AllSkills.Add(new Skill("HP 회복 증가+", "HP 회복량이 +3.5% 증가합니다.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));

        // 대미지 감소
        AllSkills.Add(new Skill("피해 감소", "받는 피해가 25% 감소합니다.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/shield")));
        AllSkills.Add(new Skill("피해 감소+", "받는 피해가 40% 감소합니다.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/shield")));

        // 일반 공격 시 보호막 획득
        AllSkills.Add(new Skill("일반 보호막", "일반 공격 시 확률로 보호막 획득. (보호막: HP의 5%)", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/shield")));

        // 분노 보호막
        AllSkills.Add(new Skill("분노 보호막", "분노 스킬 사용 시 보호막 획득. (보호막: HP의 10%)", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/shield")));

        // 수리검 투척
        AllSkills.Add(new Skill("수리검", "2턴마다 수리검 1개를 발사. 수리검은 기본 공격의 45% 대미지를 줍니다.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/dagger")));
        AllSkills.Add(new Skill("수리검+", "2턴마다 수리검 2개 발사. 수리검은 기본 공격의 45% 대미지를 줍니다.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/dagger")));

        // 수리검 + 회복
        AllSkills.Add(new Skill("회복 수리검", "2턴마다 수리검 1개 발사. 수리검은 확률적으로 3% HP 회복 가능.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/dagger")));
        AllSkills.Add(new Skill("회복 수리검+", "2턴마다 수리검 1개 발사. 수리검은 확률적으로 3% HP 회복 가능. 확률 증가.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/dagger")));

        // 수리검 + 번개
        AllSkills.Add(new Skill("번개 수리검", "2턴마다 수리검 1개 발사. 모든 수리검은 일정 확률로 번개 발동 가능. 번개 대미지: 기본 공격의 30%", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/bolt")));
        AllSkills.Add(new Skill("번개 수리검+", "2턴마다 수리검 1개 발사. 번개 확률 증가. 번개 대미지: 기본 공격의 30%", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/bolt")));

        // 수리검 + 독소
        AllSkills.Add(new Skill("독소 수리검", "2턴마다 수리검 1개 발사. 일정 확률로 적에게 중독 효과. 중독 대미지: 기본 공격의 20%", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/poison")));
        AllSkills.Add(new Skill("독소 수리검+", "2턴마다 수리검 1개 발사. 확률 증가. 중독 대미지: 기본 공격의 20%", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/poison")));

        // 치명타 관련 스킬
        AllSkills.Add(new Skill("치명타 확률 상승", "기본 공격의 치명타 확률↑.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/crit")));
        AllSkills.Add(new Skill("치명타 확률 상승+", "기본 공격의 치명타 확률이 ↑↑.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/crit")));

        // 연타 관련 스킬
        AllSkills.Add(new Skill("연타 확률 상승", "기본 공격이 연타로 발동될 확률↑.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/multi")));
        AllSkills.Add(new Skill("연타 확률 상승+", "기본 공격이 연타로 발동될 확률↑↑.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/multi")));

        //특수 스킬
        AllSkills.Add(new Skill("폭군", "공격력 ↑↑↑↑, HP 최대치 ↓↓.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("방어 끝판왕", "턴마다 대미지 감소 ↑.", 30, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("용맹 끝판왕", "턴마다 공격력 상승.", 30, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("슈퍼 공격력", "공격력 ↑↑↑↑↑.", 35, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("슈퍼 생명력", "HP ↑↑↑↑↑, HP 최대치까지 회복.", 35, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("천사의 힘", "전투 시작 시, HP ↑↑↑↑↑↑↑↑↑↑. 천사의 보호를 획득하며 6턴 지속.", 40, SkillType.Passive, SkillGrade.Immortal, angelIcon));
        AllSkills.Add(new Skill("악마의 힘", "HP 처음으로 50% 미만일 경우 공격력 ↑↑↑↑↑↑↑↑↑↑. 악마의 보호를 획득하며 전투 종료까지 지속.", 40, SkillType.Passive, SkillGrade.Immortal, demonIcon));


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
