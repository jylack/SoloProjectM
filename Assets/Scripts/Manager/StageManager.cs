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

    public static StageManager Instance;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

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
            //Debug.Log(monsterSpawnPoint.position);
            currentMonsters.Add(monster);
        }

        StartCoroutine(StartBattle());
    }
    public void InitStage(StageInfoSO stageInfo)
    {
        // �������� ������ ���� ���� ����
        foreach (var monster in stageInfo.enemySet.possibleEnemies)
        {
            GameObject monsterObj = Instantiate(monster, monsterSpawnPoint);
            currentMonsters.Add(monsterObj);
        }
        StartCoroutine(StartBattle());
    }

    private IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(1f);

        List<UnitStats> monsterStatsList = new List<UnitStats>();

        var count = currentMonsters.Count;
        float temp = -1f;
 
        if(count > 1)
        {
            monsterSpawnPoint.position += Vector3.right;
        }

        for (int i = 0; i < count; i++)
        {
            var monster = currentMonsters[i].GetComponent<Monster>();

            if (monster != null)
            {
                if (i > 0)
                {
                    var x = monsterSpawnPoint.position.x + ((monster.gameObject.GetComponent<Renderer>().bounds.size.x / monster.transform.localScale.x) * temp);
                    //Debug.Log(x);
                    var y = monsterSpawnPoint.position.y;
                    var z = monsterSpawnPoint.position.z;
                    temp = temp * -1f;

                    monster.gameObject.transform.position = new Vector3(x, y, z);
                }
                monsterStatsList.Add(monster.Stats);
                logUI.AddLog( "["+ monster.Stats.Name + "] ���� ����! ");
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
        GameManager.instance.player.Buffs.AddBuff(powerBuff);
        logUI.AddLog("���� ���� �ο��Ǿ����ϴ�! (3�ϰ� ���ݷ� +10)");
    }

}
