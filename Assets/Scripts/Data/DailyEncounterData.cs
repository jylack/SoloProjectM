// DailyEncounterData.cs
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Encounters/Daily Encounter Data")]
public class DailyEncounterData : ScriptableObject
{
    [Tooltip("몇 번째 스테이지인지")]
    public int stage;

    [Tooltip("스테이지 내 며칠차(1부터 시작)인지")]
    public int day;

    [Tooltip("이 날 등장 가능한 몬스터 리스트")]
    public List<MonsterEncounter> possibleMonsters;
}
