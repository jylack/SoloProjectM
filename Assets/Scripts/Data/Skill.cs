public enum SkillType
{
    Active,
    Passive
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

public class Skill
{
    public string Name;
    public string Description;
    public int Power;
    public SkillType Type;
    public SkillGrade Grade;
    public int Stack; // ÁßÃ¸ ¼öÄ¡

    public Skill(string name, string desc, int power, SkillType type, SkillGrade grade)
    {
        Name = name;
        Description = desc;
        Power = power;
        Type = type;
        Grade = grade;
        Stack = 1;
    }

    public Skill Clone()
    {
        return new Skill(Name, Description, Power, Type, Grade) { Stack = Stack };
    }

    public void ApplyStack(Skill other)
    {
        Power += other.Power;
        Stack++;
        Description += "+";
        
    }
}
