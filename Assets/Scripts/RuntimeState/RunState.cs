using System;
using System.Collections.Generic;

[Serializable]
public class RunState
{
    public int currentDay;
    public int currentGold;
    public bool isDead;
    public bool isCleared;
    public StatBlock stats = new StatBlock();
    public List<string> ownedSkillIds = new List<string>();
    public List<string> ownedItemIds = new List<string>();

    public void StartRun(StatBlock baseStats)
    {
        currentDay = 1;
        currentGold = 0;
        isDead = false;
        isCleared = false;
        stats = baseStats != null ? baseStats.Clone() : new StatBlock();
        if (stats.currentHp <= 0)
        {
            stats.currentHp = stats.maxHp;
        }

        ownedSkillIds = new List<string>();
        ownedItemIds = new List<string>();
    }

    public void AdvanceDay()
    {
        currentDay++;
    }

    public void MarkDead()
    {
        isDead = true;
    }

    public void MarkCleared()
    {
        isCleared = true;
    }
}
