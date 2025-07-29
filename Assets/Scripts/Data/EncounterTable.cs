// EncounterTable.cs
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Encounters/Encounter Table")]
public class EncounterTable : ScriptableObject
{
    [Tooltip("스테이지·날짜별 인카운터 설정")]
    public List<DailyEncounterData> dailyData;
}
