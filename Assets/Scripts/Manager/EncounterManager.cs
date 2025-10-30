using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance { get; private set; }

    //[Header("에디터에서 만든 EncounterTable 연결")]
    //[SerializeField] private EncounterTable table;
    [Header("스테이지에서 EncounterTable을 제공하지 않을 때 사용할 기본 테이블")]
    [SerializeField] private EncounterTable defaultEncounterTable;

    // 현재 날짜 스테이지에 맞는 데이터
    private DailyEncounterData _todayData;
    private EncounterTable _activeTable;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        var stageManager = StageManager.Instance;

        if (stageManager != null)
        {
            stageManager.OnStageDayChanged.AddListener(SetupForDay);

            // StageManager가 이미 초기화되어 있다면 즉시 첫 데이터 세트를 로드한다.
            var initialStage = stageManager.CurrentStage;
            var initialDay = stageManager.CurrentDay;

            SetupForDay(initialStage, initialDay);
        }
        else
        {
            Debug.LogError("EncounterManager: StageManager가 초기화되지 않았습니다.");
        }
    }

    private void OnEnable()
    {
        //StageManager.Instance.OnStageDayChanged.AddListener(SetupForDay);
    }

    private void OnDisable()
    {
        if (StageManager.Instance != null)
            StageManager.Instance.OnStageDayChanged.RemoveListener(SetupForDay);
    }

    private void SetupForDay(int stage, int day)
    {
        //_todayData = table.dailyData.Find(d => d.stage == stage && d.day == day);
        if (stage < 0 || day <= 0)
        {
            _activeTable = null;
            _todayData = null;
            return;
        }

        _activeTable = null;

        if (StageManager.Instance != null)
            _activeTable = StageManager.Instance.CurrentEncounterTable;

        if (_activeTable == null)
            _activeTable = defaultEncounterTable;

        if (_activeTable == null)
        {
            _todayData = null;
            Debug.LogWarning($"EncounterManager: Stage {stage}에 사용할 EncounterTable이 설정되지 않았습니다.");
            return;
        }

        if (_activeTable.dailyData == null)
        {
            _todayData = null;
            Debug.LogWarning($"EncounterManager: EncounterTable '{_activeTable.name}'에 DailyEncounterData가 비어 있습니다.");
            return;
        }

        _todayData = _activeTable.dailyData.Find(d => d.stage == stage && d.day == day);

        if (_todayData == null)
            //   Debug.LogWarning($"EncounterTable에 Stage {stage}, Day {day} 설정이 없습니다.");
            Debug.LogWarning($"EncounterManager: EncounterTable '{_activeTable.name}'에 Stage {stage}, Day {day} 설정이 없습니다.");
        else
            Debug.Log($"EncounterManager: Stage {stage}, Day {day} 데이터 로드 완료 (몬스터 {(_todayData.possibleMonsters?.Count ?? 0)}종).");
    }

    /// <summary>
    /// 인카운터 발생 시 호출. 가중치를 고려해 N마리 뽑아서 반환합니다.
    /// </summary>
    public List<MonsterDefinition> GetRandomEncounters(int count)
    {
        var result = new List<MonsterDefinition>();

        if (_todayData == null || _todayData.possibleMonsters.Count == 0)
            return result;

        // 전체 가중치 합산
        int totalWeight = 0;

        foreach (var e in _todayData.possibleMonsters)
            totalWeight += e.weight;

        for (int i = 0; i < count; i++)
        {
            int roll = Random.Range(0, totalWeight);
            int cum = 0;

            foreach (var e in _todayData.possibleMonsters)
            {
                cum += e.weight;

                if (roll < cum)
                {
                    result.Add(e.monsterDef);
                    break;
                }
            }
        }
        return result;
    }
}
