using System.Collections.Generic;

public static class StageGenerator
{
    public static StageRunData Generate(int stageId, int totalDays, int seed)
    {
        var random = new System.Random(seed);
        var result = new StageRunData
        {
            stageId = stageId,
            seed = seed,
            currentDayIndex = 0,
            days = new List<EncounterData>(),
        };

        int dayCount = totalDays < 1 ? 1 : totalDays;

        for (int i = 1; i <= dayCount; i++)
        {
            bool isLast = i == dayCount;
            var data = new EncounterData
            {
                day = i,
                encounterId = "stage" + stageId + "_day" + i,
                encounterType = isLast ? EncounterType.Boss : RollEncounterType(random),
                rewards = new List<RewardData>(),
            };

            if (data.encounterType == EncounterType.Reward)
            {
                data.hasSelectionReward = true;
                data.rewards.Add(RollReward(random));
                data.rewards.Add(RollReward(random));
                data.rewards.Add(RollReward(random));
            }
            else
            {
                data.hasSelectionReward = false;
                data.rewards.Add(RollReward(random));
            }

            if (data.encounterType == EncounterType.Battle || data.encounterType == EncounterType.Boss)
            {
                data.enemyId = data.encounterType == EncounterType.Boss ? "boss_" + stageId : "enemy_" + stageId + "_" + i;
            }

            result.days.Add(data);
        }

        return result;
    }

    private static EncounterType RollEncounterType(System.Random random)
    {
        int roll = random.Next(0, 100);
        if (roll < 60)
        {
            return EncounterType.Battle;
        }

        if (roll < 80)
        {
            return EncounterType.Reward;
        }

        return EncounterType.Rest;
    }

    private static RewardData RollReward(System.Random random)
    {
        int roll = random.Next(0, 100);
        if (roll < 35)
        {
            return RewardData.CreateGold(random.Next(20, 51));
        }

        if (roll < 70)
        {
            return RewardData.CreateHeal(random.Next(10, 31));
        }

        StatType statType = random.Next(0, 2) == 0 ? StatType.Attack : StatType.MaxHp;
        int amount = statType == StatType.Attack ? random.Next(1, 4) : random.Next(5, 16);
        return RewardData.CreateStat(statType, amount);
    }
}
