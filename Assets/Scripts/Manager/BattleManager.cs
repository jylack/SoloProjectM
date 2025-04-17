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
        // ����: �÷��̾� �������� �̵�
        Vector3 playerStart = playerTransform.position;
        Vector3 playerTarget = playerStart + new Vector3(1f,0,0) ;
        yield return MoveOverTime(playerTransform, playerStart, playerTarget, moveDuration);

        // ����: ���� ������ �ٱ����� ����
        Vector3 monsterStart = monsterTransform.position + Vector3.right * 2.5f;
        monsterTransform.position = monsterStart;
        Vector3 monsterTarget = monsterStart + Vector3.left * 3.5f;
        yield return MoveOverTime(monsterTransform, monsterStart, monsterTarget, moveDuration);

        yield return new WaitForSeconds(0.5f);

        // ����: ��� ī�޶� �̵�����
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
            // �ӵ��� ������ �������� ����
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
                // ����: ��� ī�޶� �̵�����
                parallaxBackground.Camera_Move = true;
                yield break;
            }

            // �� ��ü
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
