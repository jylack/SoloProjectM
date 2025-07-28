using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    private UnitStats playerStats;
    private UnitStats monsterStats;

    [SerializeField] private LogUI battleLogUI;

    [SerializeField] private Transform playerTransform;
    Player player;

    [SerializeField] private Transform monsterTransform;
    Monster monster;

    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float attackDelay = 0.5f;

    [SerializeField] private ParallaxBackground parallaxBackground;

    private UnitStats currentAttacker;
    private UnitStats currentDefender;

    private Queue<ICombatant> turnQueue;
    private void InitTurnQueue()
    {
        var list = new List<ICombatant> { playerStats, monsterStats };
        list = list.OrderByDescending(u => u.Speed).ToList();
        turnQueue = new Queue<ICombatant>(list);
    }

    private void OnEnable()
    {
        player = playerTransform.GetComponent<Player>();
        playerStats = player.GetStats();
        player.AnimSetting();

        monster = monsterTransform.GetComponent<Monster>();
        monsterStats = monster.GetStats();

        player.SetAnim(PlayerState.MOVE);
        monster.SetAnim(MonsterState.MOVE);

        StartCoroutine(StartBattle());
    }

    private IEnumerator StartBattle()
    {
        yield return new WaitUntil(() => GameManager.instance != null); // 전투 시작 대기
        battleLogUI.AddDayLog(GameManager.instance.currentDay, "전투 시작!");


        // 연출: 플레이어 왼쪽으로 이동
        Vector3 playerStart = playerTransform.position;
        Vector3 playerTarget = playerStart + new Vector3(1f, 0, 0);
        yield return MoveOverTime(playerTransform, playerStart, playerTarget, moveDuration);

        // 연출: 몬스터 오른쪽 바깥에서 등장
        Vector3 monsterStart = monsterTransform.position + Vector3.right * 2.5f;
        monsterTransform.position = monsterStart;
        Vector3 monsterTarget = monsterStart + Vector3.left * 3.5f;
        yield return MoveOverTime(monsterTransform, monsterStart, monsterTarget, moveDuration);

        yield return new WaitForSeconds(attackDelay);

        // 연출: 배경 카메라 이동중지
        parallaxBackground.cameraMove = false;
        player.SetAnim(PlayerState.IDLE);

        //아래부터 전투
        DecideFirstTurn();

        yield return StartCoroutine(CombatLoop());
    }

    //누가 먼저 싸울꺼냐!
    private void DecideFirstTurn()
    {
        if (playerStats.Speed > monsterStats.Speed)
        {
            currentAttacker = playerStats;
            currentDefender = monsterStats;
        }
        else if (playerStats.Speed < monsterStats.Speed)
        {
            currentAttacker = monsterStats;
            currentDefender = playerStats;
        }
        else
        {
            // 속도가 같으면 랜덤으로 결정
            if (Random.value < 0.5f)
            {
                currentAttacker = playerStats;
                currentDefender = monsterStats;
            }
            else
            {
                currentAttacker = monsterStats;
                currentDefender = playerStats;
            }
        }

        //battleLogUI.AddLog(currentAttacker.Name + "이(가) 먼저 공격합니다!");
    }

    private IEnumerator CombatLoop()
    {
        while (turnQueue.Count > 0)
        {
            var actor = turnQueue.Dequeue();
            var defender = (actor == playerStats) ? (ICombatant)monsterStats : playerStats;

            if (actor.IsDead) continue;

            actor.ResetActions();

            // 행동 가능 횟수만큼 반복
            while (actor.CurrentActions > 0 && !defender.IsDead)
            {
                // 애니메이션 세팅
                if (actor == playerStats) player.SetAnim(PlayerState.ATTACK);
                else monster.SetAnim(MonsterState.ATK1);
                if (defender == playerStats) player.SetAnim(PlayerState.DAMAGED);
                else monster.SetAnim(MonsterState.DAMAGE);

                // 데미지 적용
                defender.TakeDamage(actor.Attack);
                battleLogUI.AddLog($"{actor.Name} → {defender.Name} : {actor.Attack} 피해");

                // 사망 체크
                if (defender.IsDead) break;

                actor.CurrentActions--;
                yield return new WaitForSeconds(attackDelay);
            }

            // 승패 판단
            if (monsterStats.IsDead || playerStats.IsDead)
            {
                yield return HandleDeath(defender);
                yield break;
            }

            // 남아있으면 다시 큐에
            if (!actor.IsDead) turnQueue.Enqueue(actor);

            yield return new WaitForSeconds(attackDelay);
        }
    }

    private IEnumerator HandleDeath(ICombatant fallen)
    {
        if (fallen == playerStats)
        {
            player.SetAnim(PlayerState.DEATH);
            battleLogUI.AddLog("YOU DIE");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("RoomScene");
        }
        else
        {
            monster.SetAnim(MonsterState.DEATH);
            battleLogUI.AddLog($"{fallen.Name} 처치!");
            yield return new WaitForSeconds(1f);
            Destroy(monsterTransform.GetChild(0).gameObject, 1f);
            parallaxBackground.cameraMove = true;
        }
    }


    //무빙 무빙 
    private IEnumerator MoveOverTime(Transform target, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.position = to;
    }
}
