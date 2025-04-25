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
        int monsterCount = Random.Range(1, 4); // 1~3마리 등장

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
        // 스테이지 정보에 따라 몬스터 생성
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
                logUI.AddLog( "["+ monster.Stats.Name + "] 몬스터 등장! ");
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
        GameManager.instance.player.Buffs.AddBuff(powerBuff);
        logUI.AddLog("힘의 룬이 부여되었습니다! (3턴간 공격력 +10)");
    }

}
