using System;
using System.Collections.Generic;

[Serializable]
public class EncounterData
{
    public string encounterId;
    public EncounterType encounterType;
    public int day;
    public string enemyId;
    public bool hasSelectionReward;
    public List<RewardData> rewards = new List<RewardData>();
}
