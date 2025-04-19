using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum SceneName
{
    TitleScene,
    RoomScene,
    InGameScene,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UnitStats PlayerBaseStats; // 룸씬에서 장비 반영된 최종 스탯

    public UnitStats PlayerCurrentStats; // 인게임 전용 복사본

    [Header("스테이지 진행")]
    public int currentStage = 0;
    public int currentDay = 1; // 로그 출력을 위한 현재 날짜

    public BuffSystem PlayerBuffs = new BuffSystem();

    public Dictionary<string, Skill> PlayerSkillDict = new();
    public List<Skill> PlayerSkills => new List<Skill>(PlayerSkillDict.Values);
    public List<Skill> PlayerCurrentSkills => new List<Skill>(PlayerSkillDict.Values);

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        SetPlayerBaseStats(new UnitStats("Player", 10000, 10, 0, 5, 1)); // 기본 스탯 초기화
        
    }
   public void AddSkill(Skill newSkill)
    {
        if (PlayerSkillDict.ContainsKey(newSkill.Name))
        {
            PlayerSkillDict[newSkill.Name].ApplyStack(newSkill);
            //LogUI.Instance.AddLog($"스킬 강화: {newSkill.Name} (스택 {PlayerSkillDict[newSkill.Name].Stack})");
        }
        else
        {
            PlayerSkillDict[newSkill.Name] = newSkill.Clone();
            //LogUI.Instance.AddLog($"스킬 획득: {newSkill.Name}");
        }
    }

    public void ShowOptionUI()
    {
        //SkillHudUI.Instance.ShowSkills(PlayerSkills);
    }

    // 룸씬에서 장비 기반으로 스탯 세팅 시 호출
    public void SetPlayerBaseStats(UnitStats newStats)
    {
        PlayerBaseStats = newStats.Clone();
        
        //Debug.Log(newStats.Clone().Name);

        ResetPlayerStats();
    }

    // 인게임 진입 시 현재 스탯 초기화 (강화 이전 상태)
    public void ResetPlayerStats()
    {
        PlayerCurrentStats = PlayerBaseStats.Clone();
        //Debug.Log(PlayerBaseStats.Clone().Name);

    }

    // 강화 적용 
    public void ApplyStatBuff(int hp, int atk, int def, int spd)
    {
        PlayerCurrentStats.ApplyStatBuff(hp, atk, def, spd);
    }

    // 인게임 종료 시 리셋
    public void OnInGameEnd()
    {
        ResetPlayerStats();
    }

    public void SceneLoad(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}