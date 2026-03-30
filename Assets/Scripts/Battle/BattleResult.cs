using System;
using System.Collections.Generic;

[Serializable]
public class BattleResult
{
    public bool isWin;
    public int remainingHp;
    public List<RewardData> rewards = new List<RewardData>();
}
