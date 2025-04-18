using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private GameObject[] monsterPrefabs;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private LogUI logUI;

    private List<GameObject> currentMonsters = new List<GameObject>();

    private void Start()
    {
        InitStage();
    }

    private void InitStage()
    {
        int monsterCount = Random.Range(1, 4); // 1~3마리 등장

        for (int i = 0; i < monsterCount; i++)
        {
            int rand = Random.Range(0, monsterPrefabs.Length);
            GameObject monster = Instantiate(monsterPrefabs[rand], monsterSpawnPoint);
            currentMonsters.Add(monster);
        }

        StartCoroutine(StartBattle());
    }

    private IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(1f);

        List<UnitStats> monsterStatsList = new List<UnitStats>();

        foreach (var monsterGO in currentMonsters)
        {
            var monster = monsterGO.GetComponent<Monster>();
            if (monster != null)
            {
                monsterStatsList.Add(monster.Stats);
                logUI.AddLog("몬스터 등장! " + monster.Stats.Name);
            }
        }

        battleManager.StartBattle(GameManager.instance.PlayerCurrentStats, monsterStatsList, OnBattleEnd);
    }

    private void OnBattleEnd(bool playerWon)
    {
        if (!playerWon)
        {
            logUI.AddLog("플레이어 패배! 스테이지 종료");
            GameManager.instance.OnInGameEnd();
            return;
        }

        logUI.AddLog("스테이지 클리어!");

        TriggerBuffEvent(); // 여기에 넣는 것!

        GameManager.instance.OnInGameEnd();
    }
    public void TriggerBuffEvent()
    {
        Buff powerBuff = new Buff("힘의 룬", 3, 10, 0);
        GameManager.instance.PlayerBuffs.AddBuff(powerBuff);
        logUI.AddLog("힘의 룬이 부여되었습니다! (3턴간 공격력 +10)");
    }

}
