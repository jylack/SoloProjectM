
// StageManager.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 스테이지별 정의를 기반으로 현재 스테이지/일차를 관리하는 싱글톤 매니저입니다.
/// </summary>
public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }


    [Header("등록된 스테이지 정의 목록")]
    [SerializeField] private List<StageDefinition> stageDefinitions = new List<StageDefinition>();

    /// <summary>
    /// 현재 선택된 스테이지 정의.
    /// </summary>
    public StageDefinition CurrentStageDefinition { get; private set; }

    /// <summary>
    /// 현재 진행 중인 스테이지 ID. 스테이지가 없으면 -1을 반환합니다.
    /// </summary>
    public int CurrentStage => CurrentStageDefinition != null ? CurrentStageDefinition.stageId : -1;

    /// <summary>
    /// 현재 스테이지에서 진행 중인 일차.
    /// </summary>
    public int CurrentDay { get; private set; } = 1;

    /// <summary>
    /// 현재 스테이지의 최대 일수.
    /// </summary>
    public int MaxDays => _currentMaxDays;

    /// <summary>
    /// 현재 스테이지에서 사용할 인카운터 테이블.
    /// </summary>
    public EncounterTable CurrentEncounterTable => _currentEncounterTable;

    /// <summary>
    /// 스테이지·일수가 바뀔 때 호출되는 이벤트 (stageId, day).
    /// </summary>
    public UnityEvent<int, int> OnStageDayChanged = new UnityEvent<int, int>();


    private readonly Dictionary<int, StageDefinition> _stageLookup = new Dictionary<int, StageDefinition>();
    private EncounterTable _currentEncounterTable;
    private int _currentMaxDays;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


        RebuildLookup(stageDefinitions);

        if (CurrentStageDefinition == null && stageDefinitions.Count > 0)
        {
            ApplyStage(stageDefinitions[0], true, false);
            PushStageDay();
        }
    }
    private void OnEnable()
    {
        // 다른 매니저들이 구독을 마친 뒤에도 초기 상태를 다시 한 번 알려준다.
        PushStageDay();
    }
    /// <summary>
    /// 외부에서 전달된 스테이지 정의 목록을 StageManager에 등록합니다.
    /// </summary>
    /// <param name="definitions">등록할 스테이지 정의 목록.</param>
    /// <param name="selectFirstWhenNoneSelected">현재 선택된 스테이지가 없을 때 첫 번째 정의를 자동 선택할지 여부.</param>
    public void ConfigureStages(IEnumerable<StageDefinition> definitions, bool selectFirstWhenNoneSelected = true)
    {
        if (definitions == null)
        {
            stageDefinitions.Clear();
            RebuildLookup(stageDefinitions);
            ApplyStage(null, true, true);
            return;
        }

        int previousStageId = CurrentStage;

        stageDefinitions.Clear();

        foreach (var def in definitions)
        {
            if (def == null)
                continue;

            stageDefinitions.Add(def);
        }

        RebuildLookup(stageDefinitions);

        if (previousStageId != -1 && _stageLookup.TryGetValue(previousStageId, out var previousDefinition))
        {
            ApplyStage(previousDefinition, false, false);
            PushStageDay();
            return;
        }

        if (selectFirstWhenNoneSelected && stageDefinitions.Count > 0)
        {
            ApplyStage(stageDefinitions[0], true);
        }
        else
        {

            ApplyStage(null, true, true);
        }
    }

    /// <summary>
    /// 하루를 진행시키고, 최대 일수를 넘기면 스테이지 종료 로직을 호출합니다.
    /// </summary>
    public void AdvanceDay()
    {
        if (CurrentStageDefinition == null)
        {
            Debug.LogWarning("StageManager: AdvanceDay called without an active stage definition.");
            return;
        }

        CurrentDay++;




        if (_currentMaxDays > 0 && CurrentDay > _currentMaxDays)
        {
            EndStage();
        }

        PushStageDay();
    }

    /// <summary>
    /// 특정 ID의 스테이지를 강제로 선택합니다.
    /// </summary>
    public void ForceStage(int stageId)
    {

        if (_stageLookup.TryGetValue(stageId, out var definition))
        {
            ApplyStage(definition, true);
        }
        else
        {
            Debug.LogWarning($"StageManager: Requested stage id {stageId} is not registered.");
        }
    }

    /// <summary>
    /// 특정 스테이지 정의를 강제로 선택합니다.
    /// </summary>
    public void ForceStage(StageDefinition definition)
    {
        if (definition == null)
        {
            Debug.LogWarning("StageManager: Cannot force a null stage definition.");
            return;
        }

        if (!_stageLookup.ContainsKey(definition.stageId))
        {
            stageDefinitions.Add(definition);
            _stageLookup[definition.stageId] = definition;
        }

        ApplyStage(definition, true);
    }

    /// <summary>
    /// stageId로 스테이지 정의를 조회합니다.
    /// </summary>
    public bool TryGetStageDefinition(int stageId, out StageDefinition definition)
    {
        return _stageLookup.TryGetValue(stageId, out definition);
    }

    private void ApplyStage(StageDefinition definition, bool resetDay, bool invokeEvent = true)
    {
        CurrentStageDefinition = definition;

        if (definition == null)
        {
            _currentEncounterTable = null;
            _currentMaxDays = 0;
            CurrentDay = 0;
            if (invokeEvent)
                OnStageDayChanged?.Invoke(-1, 0);
            return;
        }

        _currentEncounterTable = definition.encounterTable;
        _currentMaxDays = Mathf.Max(1, definition.maxDays);

        if (resetDay || CurrentDay < 1)
        {
            CurrentDay = 1;
        }
        else
        {
            CurrentDay = Mathf.Clamp(CurrentDay, 1, _currentMaxDays);
        }

        if (invokeEvent)
        {
            PushStageDay();
        }
    }

    private void RebuildLookup(IEnumerable<StageDefinition> definitions)
    {
        _stageLookup.Clear();

        if (definitions == null)
            return;

        foreach (var def in definitions)
        {
            if (def == null)
                continue;

            if (_stageLookup.ContainsKey(def.stageId))
            {
                Debug.LogWarning($"StageManager: Duplicate stageId {def.stageId} detected. Using the last registered definition.");
            }

            _stageLookup[def.stageId] = def;
        }
    }

    private void PushStageDay()
    {
        if (CurrentStageDefinition == null)
            return;

        OnStageDayChanged?.Invoke(CurrentStageDefinition.stageId, CurrentDay);
    }

    private void EndStage()
    {
        Debug.Log($"Stage {CurrentStage} 완료! 룸으로 돌아가던지 다음스테이지 바로가던지 결정");
    }
}
