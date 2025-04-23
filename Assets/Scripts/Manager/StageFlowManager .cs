using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StageFlowManager : MonoBehaviour
{
    public static StageFlowManager instance;

    [Header("스테이지 데이터 리스트")]
    public List<StageInfoSO> stages;
    private int currentIndex = 0;

    public StageInfoSO CurrentStage => stages[currentIndex];

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartStage();
    }

    public void StartStage()
    {
        StageInfoSO stage = CurrentStage;

        // 설명 출력
        if (!string.IsNullOrEmpty(stage.description))
            LogUI.Instance?.AddLog(stage.description);

        // 효과음 재생
        if (stage.sfxOnEnter != null)
            AudioSource.PlayClipAtPoint(stage.sfxOnEnter, Camera.main.transform.position);

        // 배경 및 NPC 생성
        if (stage.backgroundPrefab != null)
            Instantiate(stage.backgroundPrefab);
        if (stage.npcPrefab != null)
            Instantiate(stage.npcPrefab);

        // 스테이지 타입에 따라 분기
        switch (stage.type)
        {
            case StageType.Normal:
            case StageType.Elite:
            case StageType.Boss:
                StageManager.Instance.InitStage(stage);
                break;

            //case StageType.Fairy:
            //    UIManager.Instance.ShowFairyEvent(stage);
            //    break;

            //case StageType.Demon:
            //    UIManager.Instance.ShowDemonContract(stage);
            //    break;

            //case StageType.Angel:
            //    UIManager.Instance.ShowAngelBlessing(stage);
            //    break;

            case StageType.Reward:
                UIManager.Instance.ShowReward(stage.rewardCondition);
                
                break;

            default:
                break;
        }
    }

    public void AdvanceToNextStage()
    {
        currentIndex++;

        if (currentIndex < stages.Count)
        {
            StartStage();
        }
        else
        {
            Debug.Log("모든 스테이지 클리어!");
            GameManager.instance.OnInGameEnd(); // 예: 룸씬으로 이동
        }
    }

    public void ResetProgress()
    {
        currentIndex = 0;
        foreach (var s in stages) s.isCleared = false;
    }
}
