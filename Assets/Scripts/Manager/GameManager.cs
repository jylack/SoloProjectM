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

    public UnitStats PlayerBaseStats; // ������� ��� �ݿ��� ���� ����

    public UnitStats PlayerCurrentStats; // �ΰ��� ���� ���纻

    [Header("�������� ����")]
    public int currentStage = 0;
    public int currentDay = 1; // �α� ����� ���� ���� ��¥

    public BuffSystem PlayerBuffs = new BuffSystem();

    public Dictionary<string, Skill> PlayerSkillDict = new();
    public List<Skill> PlayerSkills => new List<Skill>(PlayerSkillDict.Values);
    public List<Skill> PlayerCurrentSkills => new List<Skill>(PlayerSkillDict.Values);

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        SetPlayerBaseStats(new UnitStats("Player", 10000, 10, 0, 5, 1)); // �⺻ ���� �ʱ�ȭ
        
    }
   public void AddSkill(Skill newSkill)
    {
        if (PlayerSkillDict.ContainsKey(newSkill.Name))
        {
            PlayerSkillDict[newSkill.Name].ApplyStack(newSkill);
            //LogUI.Instance.AddLog($"��ų ��ȭ: {newSkill.Name} (���� {PlayerSkillDict[newSkill.Name].Stack})");
        }
        else
        {
            PlayerSkillDict[newSkill.Name] = newSkill.Clone();
            //LogUI.Instance.AddLog($"��ų ȹ��: {newSkill.Name}");
        }
    }

    public void ShowOptionUI()
    {
        //SkillHudUI.Instance.ShowSkills(PlayerSkills);
    }

    // ������� ��� ������� ���� ���� �� ȣ��
    public void SetPlayerBaseStats(UnitStats newStats)
    {
        PlayerBaseStats = newStats.Clone();
        
        //Debug.Log(newStats.Clone().Name);

        ResetPlayerStats();
    }

    // �ΰ��� ���� �� ���� ���� �ʱ�ȭ (��ȭ ���� ����)
    public void ResetPlayerStats()
    {
        PlayerCurrentStats = PlayerBaseStats.Clone();
        //Debug.Log(PlayerBaseStats.Clone().Name);

    }

    // ��ȭ ���� 
    public void ApplyStatBuff(int hp, int atk, int def, int spd)
    {
        PlayerCurrentStats.ApplyStatBuff(hp, atk, def, spd);
    }

    // �ΰ��� ���� �� ����
    public void OnInGameEnd()
    {
        ResetPlayerStats();
    }

    public void SceneLoad(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}