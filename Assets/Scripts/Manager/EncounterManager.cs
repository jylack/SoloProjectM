// EncounterManager.cs
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance { get; private set; }

    [Header("에디터에서 만든 EncounterTable 연결")]
    [SerializeField] private EncounterTable table;

    // 현재 날짜 스테이지에 맞는 데이터
    private DailyEncounterData _todayData;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        StageManager.Instance.OnStageDayChanged.AddListener(SetupForDay);
    }

    private void OnDisable()
    {
        if (StageManager.Instance != null)
            StageManager.Instance.OnStageDayChanged.RemoveListener(SetupForDay);
    }

    private void SetupForDay(int stage, int day)
    {
        _todayData = table.dailyData.Find(d => d.stage == stage && d.day == day);
        if (_todayData == null)
            Debug.LogWarning($"EncounterTable에 Stage {stage}, Day {day} 설정이 없습니다.");
    }

    /// <summary>
    /// 인카운터 발생 시 호출. 가중치를 고려해 N마리 뽑아서 반환합니다.
    /// </summary>
    public List<GameObject> GetRandomEncounters(int count)
    {
        var result = new List<GameObject>();
        if (_todayData == null || _todayData.possibleMonsters.Count == 0)
            return result;

        // 전체 가중치 합산
        int totalWeight = 0;
        foreach (var e in _todayData.possibleMonsters) totalWeight += e.weight;

        for (int i = 0; i < count; i++)
        {
            int roll = Random.Range(0, totalWeight);
            int cum = 0;
            foreach (var e in _todayData.possibleMonsters)
            {
                cum += e.weight;
                if (roll < cum)
                {
                    result.Add(e.monsterPrefab);
                    break;
                }
            }
        }
        return result;
    }
}
