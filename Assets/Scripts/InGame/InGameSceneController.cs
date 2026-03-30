using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameSceneController : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private RewardSelectUI rewardSelectUI;
    [SerializeField] private Text resultText;
    [SerializeField] private float returnDelay = 0.6f;

    private EncounterData _encounter;

    private void Start()
    {
        EnsureRuntimeRefs();

        var runManager = RunManager.Instance;
        if (runManager == null)
        {
            ReturnToRoom();
            return;
        }

        _encounter = runManager.CurrentEncounter;
        if (_encounter == null)
        {
            ReturnToRoom();
            return;
        }

        ExecuteEncounter(_encounter);
    }

    private void EnsureRuntimeRefs()
    {
        if (battleManager == null)
        {
            battleManager = FindObjectOfType<BattleManager>();
        }

        if (rewardSelectUI == null)
        {
            rewardSelectUI = FindObjectOfType<RewardSelectUI>();
        }

        if (rewardSelectUI == null)
        {
            var rewardUiObject = new GameObject("RewardSelectUI");
            rewardSelectUI = rewardUiObject.AddComponent<RewardSelectUI>();
        }
    }

    private void ExecuteEncounter(EncounterData encounter)
    {
        switch (encounter.encounterType)
        {
            case EncounterType.Rest:
            case EncounterType.Event:
                SetResult("Reward Applied");
                RunManager.Instance.ApplyRewards(encounter.rewards);
                StartCoroutine(CompleteEncounterSuccess());
                break;
            case EncounterType.Reward:
                HandleRewardEncounter(encounter);
                break;
            case EncounterType.Battle:
            case EncounterType.Boss:
                if (battleManager == null)
                {
                    battleManager = FindObjectOfType<BattleManager>();
                }

                if (battleManager == null)
                {
                    SetResult("BattleManager Missing");
                    RunManager.Instance.MarkDead();
                    StartCoroutine(ReturnToRoomRoutine());
                    return;
                }

                SetResult("Battle Start");
                battleManager.StartEncounterBattle(encounter, this);
                break;
        }
    }

    private void HandleRewardEncounter(EncounterData encounter)
    {
        if (encounter.hasSelectionReward && rewardSelectUI != null && encounter.rewards != null && encounter.rewards.Count > 0)
        {
            SetResult("Select Reward");
            rewardSelectUI.Show(encounter.rewards, OnSelectedReward);
            return;
        }

        SetResult("Reward Applied");
        RunManager.Instance.ApplyRewards(encounter.rewards);
        StartCoroutine(CompleteEncounterSuccess());
    }

    private void OnSelectedReward(RewardData reward)
    {
        RunManager.Instance.ApplyReward(reward);
        StartCoroutine(CompleteEncounterSuccess());
    }

    public void OnBattleFinished(BattleResult result)
    {
        if (result == null)
        {
            RunManager.Instance.MarkDead();
            StartCoroutine(ReturnToRoomRoutine());
            return;
        }

        RunManager.Instance.SetCurrentHp(result.remainingHp);

        if (!result.isWin)
        {
            SetResult("Defeat");
            RunManager.Instance.MarkDead();
            StartCoroutine(ReturnToRoomRoutine());
            return;
        }

        SetResult("Victory");
        RunManager.Instance.ApplyRewards(result.rewards);
        StartCoroutine(CompleteEncounterSuccess());
    }

    private IEnumerator CompleteEncounterSuccess()
    {
        bool isLastDay = RunManager.Instance.IsLastDay();
        RunManager.Instance.ClearEncounter();

        if (isLastDay)
        {
            RunManager.Instance.MarkCleared();
        }
        else
        {
            RunManager.Instance.AdvanceDay();
        }

        yield return new WaitForSeconds(returnDelay);
        ReturnToRoom();
    }

    private IEnumerator ReturnToRoomRoutine()
    {
        yield return new WaitForSeconds(returnDelay);
        ReturnToRoom();
    }

    private void ReturnToRoom()
    {
        SceneManager.LoadScene("RoomScene");
    }

    private void SetResult(string message)
    {
        if (resultText != null)
        {
            resultText.text = message;
        }
    }
}
