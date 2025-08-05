// StageManager.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 스테이지별 최대 일수를 관리하고
/// 현재 스테이지·일을 진행시키는 싱글톤 매니저입니다.
/// </summary>
public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [Header("스테이지별 최대 일수 설정")]
    [SerializeField] private int stage1Days = 10;
    [SerializeField] private int stage2Days = 30;
    // 필요하면 3스테이지 이후도 여기 추가

    public int CurrentStage { get; private set; } = 1;
    public int CurrentDay { get; private set; } = 1;

    // 스테이지·일이 바뀔 때 호출되는 이벤트
    //  stage ,day
    public UnityEvent<int, int > OnStageDayChanged;

    private Dictionary<int, int> _stageDurations;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 스테이지별 일수 매핑 초기화
        _stageDurations = new Dictionary<int, int>()
        {
            { 1, stage1Days },
            { 2, stage2Days },
        };
        // 초기 이벤트 발동
        OnStageDayChanged?.Invoke(CurrentStage, CurrentDay);
    }

    /// <summary>
    /// 하루를 넘기고, 만약 스테이지 최대일수를 초과하면 다음 스테이지로 넘어갑니다.
    /// </summary>
    public void AdvanceDay()
    {
        CurrentDay++;

        // 현재 스테이지가 딕셔너리에 있고, 최대일수보다 초과했으면
        if (_stageDurations.TryGetValue(CurrentStage, out var maxDay)
            && CurrentDay > maxDay)
        {
           EndStage();
        }

        OnStageDayChanged?.Invoke(CurrentStage, CurrentDay);
    }

    public void ForceStage(int stageId)
    {
        CurrentStage = stageId;
        CurrentDay = 1;
        PushStageDay();
    }

    private void PushStageDay()
    {
        OnStageDayChanged?.Invoke(CurrentStage, CurrentDay);
    }

    private void EndStage()
    {
        Debug.Log($"Stage {CurrentStage} 완료! 룸으로 돌아가던지 다음스테이지 바로가던지 결정");
    }
}
