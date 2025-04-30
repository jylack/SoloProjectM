
using System.Collections.Generic;
using UnityEngine;

public enum SkillName
{
    ComboAttack,
    Dagger,
    Thunder
}

public class SkillDB
{
    public Skill[] Skills { get; private set; } // 스킬 배열

    List<AnimationClip> animationClips = new List<AnimationClip>();

    public SkillDB()
    {
        Skills = new Skill[]
        {
            new Skill(null, "연격", "2연타 강화시 연타수가 늘어남 ", 10, 2, 1,GetAnim(SkillName.ComboAttack)),
            new Skill(null, "단검 던지기", "2턴마다 단검을 던짐 강화시 연타수 늘어남", 8, 2, 1, GetAnim(SkillName.Dagger)),
            new Skill(null, "번개 강타", "3턴마다 번개를 내려침 강화시 연타수 늘어남", 12, 3, 1, GetAnim(SkillName.Thunder))
        };
    }

    public AnimationClip GetAnim(SkillName name)
    {
        return animationClips[(int)name];
    }

}

