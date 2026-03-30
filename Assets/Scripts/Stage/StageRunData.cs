using System;
using System.Collections.Generic;

[Serializable]
public class StageRunData
{
    public int stageId;
    public int seed;
    public int currentDayIndex;
    public List<EncounterData> days = new List<EncounterData>();

    public int TotalDayCount => days != null ? days.Count : 0;
    public int CurrentDayNumber => currentDayIndex + 1;
    public bool HasRemainingDays => days != null && currentDayIndex >= 0 && currentDayIndex < days.Count;

    public EncounterData GetCurrentEncounter()
    {
        if (!HasRemainingDays)
        {
            return null;
        }

        return days[currentDayIndex];
    }

    public bool IsLastDay()
    {
        return HasRemainingDays && currentDayIndex == days.Count - 1;
    }

    public void AdvanceDay()
    {
        if (currentDayIndex < TotalDayCount)
        {
            currentDayIndex++;
        }
    }
}
