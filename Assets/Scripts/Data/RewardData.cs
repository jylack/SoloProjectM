using System;

[Serializable]
public class RewardData
{
    public RewardType rewardType;
    public int amount;
    public StatType statType;
    public string skillId;
    public string itemId;

    public static RewardData CreateHeal(int amount)
    {
        return new RewardData { rewardType = RewardType.Heal, amount = amount };
    }

    public static RewardData CreateGold(int amount)
    {
        return new RewardData { rewardType = RewardType.Gold, amount = amount };
    }

    public static RewardData CreateStat(StatType statType, int amount)
    {
        return new RewardData { rewardType = RewardType.Stat, statType = statType, amount = amount };
    }

    public static RewardData CreateSkill(string skillId)
    {
        return new RewardData { rewardType = RewardType.Skill, skillId = skillId };
    }

    public static RewardData CreateItem(string itemId)
    {
        return new RewardData { rewardType = RewardType.Item, itemId = itemId };
    }
}
