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

    public Sprite Icon;             // HUD/���� UI���� ����� ������
    public string TriggerKeyword;   // ��: "2�ϸ���", "HP 30% �̸�"
    public int TriggerValue;        // ��: 2, 30 ��

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
        // ������ ������Ű�� �ʰ� ���� �� ǥ�÷� ��ü�ϴ� ��� ����
        Description = Description + $" (��ȭ x{Stack})";
    }
}
