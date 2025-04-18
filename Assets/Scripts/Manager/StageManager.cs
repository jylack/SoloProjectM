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
        int monsterCount = Random.Range(1, 4); // 1~3���� ����

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
                logUI.AddLog("���� ����! " + monster.Stats.Name);
            }
        }

        battleManager.StartBattle(GameManager.instance.PlayerCurrentStats, monsterStatsList, OnBattleEnd);
    }

    private void OnBattleEnd(bool playerWon)
    {
        if (!playerWon)
        {
            logUI.AddLog("�÷��̾� �й�! �������� ����");
            GameManager.instance.OnInGameEnd();
            return;
        }

        logUI.AddLog("�������� Ŭ����!");

        TriggerBuffEvent(); // ���⿡ �ִ� ��!

        GameManager.instance.OnInGameEnd();
    }
    public void TriggerBuffEvent()
    {
        Buff powerBuff = new Buff("���� ��", 3, 10, 0);
        GameManager.instance.PlayerBuffs.AddBuff(powerBuff);
        logUI.AddLog("���� ���� �ο��Ǿ����ϴ�! (3�ϰ� ���ݷ� +10)");
    }

}
