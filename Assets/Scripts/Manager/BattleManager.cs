using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private UnitStats playerStats;
    [SerializeField] private UnitStats monsterStats;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform monsterTransform;

    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float attackDelay = 0.5f;

    [SerializeField] private ParallaxBackground_0 parallaxBackground;

    private UnitStats currentAttacker;
    private UnitStats currentDefender;

    private void Start()
    {
        StartCoroutine(StartBattle());
    }

    private IEnumerator StartBattle()
    {
        // 연출: 플레이어 왼쪽으로 이동
        Vector3 playerStart = playerTransform.position;
        Vector3 playerTarget = playerStart + new Vector3(1f,0,0) ;
        yield return MoveOverTime(playerTransform, playerStart, playerTarget, moveDuration);

        // 연출: 몬스터 오른쪽 바깥에서 등장
        Vector3 monsterStart = monsterTransform.position + Vector3.right * 2.5f;
        monsterTransform.position = monsterStart;
        Vector3 monsterTarget = monsterStart + Vector3.left * 3.5f;
        yield return MoveOverTime(monsterTransform, monsterStart, monsterTarget, moveDuration);

        yield return new WaitForSeconds(0.5f);

        // 연출: 배경 카메라 이동중지
        parallaxBackground.Camera_Move = false;

        DecideFirstTurn();

        yield return StartCoroutine(CombatLoop());
    }

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
    }

    private IEnumerator CombatLoop()
    {
        while (!playerStats.IsDead && !monsterStats.IsDead)
        {
            for (int i = 0; i < currentAttacker.AttackCount; i++)
            {
                currentDefender.TakeDamage(currentAttacker.Attack);
                
                if (currentDefender.IsDead) break;
                yield return new WaitForSeconds(attackDelay);
            }

            if (currentDefender.IsDead)
            {
                Debug.Log("die");
                // 연출: 배경 카메라 이동중지
                parallaxBackground.Camera_Move = true;
                yield break;
            }

            // 턴 교체
            (currentAttacker, currentDefender) = (currentDefender, currentAttacker);
            yield return new WaitForSeconds(0.5f);
        }
    }

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
