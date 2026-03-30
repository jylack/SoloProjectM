using System;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    [SerializeField] private StatBlock startStats = new StatBlock
    {
        maxHp = 100,
        currentHp = 100,
        attack = 10,
        defense = 0,
        attackSpeed = 1,
        critChance = 0,
        critDamage = 150,
    };

    public event Action StateChanged;

    public MetaProgress MetaProgress { get; private set; } = new MetaProgress();
    public RunState RunState { get; private set; } = new RunState();
    public StageRunData CurrentStageRun { get; private set; }

    private bool _encounterLocked;

    public bool HasActiveRun => CurrentStageRun != null && !RunState.isDead && !RunState.isCleared;

    public EncounterData CurrentEncounter
    {
        get
        {
            if (_encounterLocked || CurrentStageRun == null)
            {
                return null;
            }

            return CurrentStageRun.GetCurrentEncounter();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartNewRun(int stageId, int totalDays, int seed)
    {
        RunState.StartRun(startStats);
        CurrentStageRun = StageGenerator.Generate(stageId, totalDays, seed);
        _encounterLocked = false;
        NotifyStateChanged();
    }

    public void ApplyReward(RewardData reward)
    {
        if (reward == null || RunState == null)
        {
            return;
        }

        switch (reward.rewardType)
        {
            case RewardType.Heal:
                RunState.stats.Heal(reward.amount);
                break;
            case RewardType.Gold:
                RunState.currentGold += reward.amount;
                break;
            case RewardType.Stat:
                RunState.stats.AddStat(reward.statType, reward.amount);
                break;
            case RewardType.Skill:
                if (!string.IsNullOrEmpty(reward.skillId) && !RunState.ownedSkillIds.Contains(reward.skillId))
                {
                    RunState.ownedSkillIds.Add(reward.skillId);
                }
                break;
            case RewardType.Item:
                if (!string.IsNullOrEmpty(reward.itemId) && !RunState.ownedItemIds.Contains(reward.itemId))
                {
                    RunState.ownedItemIds.Add(reward.itemId);
                }
                break;
            case RewardType.Exp:
                break;
        }

        NotifyStateChanged();
    }

    public void ApplyRewards(System.Collections.Generic.IEnumerable<RewardData> rewards)
    {
        if (rewards == null)
        {
            return;
        }

        foreach (var reward in rewards)
        {
            ApplyReward(reward);
        }
    }

    public void SetCurrentHp(int currentHp)
    {
        if (RunState == null || RunState.stats == null)
        {
            return;
        }

        RunState.stats.currentHp = Mathf.Clamp(currentHp, 0, RunState.stats.maxHp);
        NotifyStateChanged();
    }

    public void AdvanceDay()
    {
        if (CurrentStageRun == null)
        {
            return;
        }

        CurrentStageRun.AdvanceDay();
        RunState.AdvanceDay();
        NotifyStateChanged();
    }

    public bool IsLastDay()
    {
        return CurrentStageRun != null && CurrentStageRun.IsLastDay();
    }

    public void MarkDead()
    {
        RunState.MarkDead();
        NotifyStateChanged();
    }

    public void MarkCleared()
    {
        RunState.MarkCleared();
        NotifyStateChanged();
    }

    public void FinalizeRun()
    {
        if (RunState != null)
        {
            MetaProgress.totalGold += RunState.currentGold;
            MetaProgress.highestDay = Mathf.Max(MetaProgress.highestDay, RunState.currentDay);
        }

        CurrentStageRun = null;
        _encounterLocked = false;
        NotifyStateChanged();
    }

    public void ClearEncounter()
    {
        _encounterLocked = true;
        NotifyStateChanged();
    }

    public void UnlockEncounter()
    {
        _encounterLocked = false;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        StateChanged?.Invoke();
    }
}
