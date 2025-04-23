public enum RewardType
{
    None,
    SkillSelect,
    SkillSelect_Rare,
    SkillSelect_Legendary,
    Gold,
    StatBuff,
    Item
}

[System.Serializable]
public class RewardCondition
{
    public RewardType type = RewardType.None;
    public int optionCount = 3;
    public SkillGrade minGrade = SkillGrade.Normal;
    public SkillGrade maxGrade = SkillGrade.Rare;
}
