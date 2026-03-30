using System;
using System.Collections.Generic;

[Serializable]
public class MetaProgress
{
    public int totalGold;
    public int highestDay;
    public List<string> unlockedSkillIds = new List<string>();
    public List<string> unlockedItemIds = new List<string>();
    public List<string> equippedItemIds = new List<string>();
}
