using UnityEngine;

public enum SkillType
{
    Passive,
    Active,
    Special
}

public enum SkillGrade
{
    Normal,
    Rare,
    Epic,
    Legendary,
    Mythic,
    Immortal
}

[System.Serializable]
public class Skill
{
    public string Name;
    public string Description;
    public int Power;
    public SkillType Type;
    public SkillGrade Grade;
    public int Stack;

    public Sprite Icon;             // HUD/보상 UI에서 사용할 아이콘
    public string TriggerKeyword;   // 예: "2턴마다", "HP 30% 미만"
    public int TriggerValue;        // 예: 2, 30 등

    public Skill(string name, string desc, int power, SkillType type, SkillGrade grade, Sprite icon = null, string triggerKeyword = "", int triggerValue = 0)
    {
        Name = name;
        Description = desc;
        Power = power;
        Type = type;
        Grade = grade;
        Stack = 1;
        Icon = icon;
        TriggerKeyword = triggerKeyword;
        TriggerValue = triggerValue;
    }

    public Skill Clone()
    {
        return new Skill(Name, Description, Power, Type, Grade, Icon, TriggerKeyword, TriggerValue)
        {
            Stack = this.Stack
        };
    }

    public void ApplyStack(Skill other)
    {
        Stack += 1;
        Power += other.Power;
        // 설명은 누적시키지 않고 스택 수 표시로 대체하는 방식 권장
        Description = Description + $" (강화 x{Stack})";
    }
}
