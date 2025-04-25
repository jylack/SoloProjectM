using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    private UnitStats playerStats;
    private UnitStats monsterStats;

    [SerializeField] private LogUI battleLogUI;

    [SerializeField] private Transform playerTransform;
    private Player player;

    [SerializeField] private Transform monsterTransform;
    private Monster monster;

    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float attackDelay = 0.5f;

    [SerializeField] private ParallaxBackground_0 parallaxBackground;

    private UnitStats currentAttacker;
    private UnitStats currentDefender;




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
        yield return new WaitUntil(() => GameManager.instance != null); // ���� ���� ���
        battleLogUI.AddDayLog(GameManager.instance.currentDay, "���� ����!");


        // ����: �÷��̾� �������� �̵�
        Vector3 playerStart = playerTransform.position;
        Vector3 playerTarget = playerStart + new Vector3(1f, 0, 0);
        yield return MoveOverTime(playerTransform, playerStart, playerTarget, moveDuration);

        // ����: ���� ������ �ٱ����� ����
        Vector3 monsterStart = monsterTransform.position + Vector3.right * 2.5f;
        monsterTransform.position = monsterStart;
        Vector3 monsterTarget = monsterStart + Vector3.left * 3.5f;
        yield return MoveOverTime(monsterTransform, monsterStart, monsterTarget, moveDuration);

        yield return new WaitForSeconds(attackDelay);

        // ����: ��� ī�޶� �̵�����
        parallaxBackground.Camera_Move = false;
        player.SetAnim(PlayerState.IDLE);

        //�Ʒ����� ����
        DecideFirstTurn();

        yield return StartCoroutine(CombatLoop());
    }

    //���� ���� �οﲨ��!
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

        //battleLogUI.AddLog(currentAttacker.Name + "��(��) ���� �����մϴ�!");
    }

    private IEnumerator CombatLoop()
    {
        //������ ���������� �ο��.
        while (!playerStats.IsDead && !monsterStats.IsDead)
        {
            //�����ڰ� ����Ƚ����ŭ ����
            for (int i = 0; i < currentAttacker.AttackCount; i++)
            {

                if (currentAttacker == playerStats)
                {
                    player.SetAnim(PlayerState.ATTACK);
                    attackDelay = player.
                }
                else if (currentAttacker == monsterStats)
                {
                    monster.SetAnim(MonsterState.ATK1);
                }

                if (currentDefender == playerStats)
                {
                    player.SetAnim(PlayerState.DAMAGED);
                }
                else if (currentDefender == monsterStats)
                {
                    monster.SetAnim(MonsterState.DAMAGE);
                }

                currentDefender.TakeDamage(currentAttacker.Attack);


                // battleLogUI.AddLog(currentAttacker.Name + "�� ����! " + currentDefender.Name + "���� " + currentAttacker.Attack + "�� ���ظ� �������ϴ�! ");
                //Debug.Log(currentAttacker.Name + "�� ����! " + currentDefender.Name + "���� " + currentAttacker.Attack + "�� ���ظ� �������ϴ�!");

                //����� �׾��°� �Ǻ�
                if (currentDefender.IsDead) break;


                //���� ������ �̰� ���߿� �ִϸ��̼� �ӵ��� ����ȭ���ٿ����� //���ִϸ��̼Ǹ��� ���̰� �ٸ�.
                yield return new WaitForSeconds(attackDelay);
            }

            //����ڰ� �׾��°� �Ǻ�
            if (currentDefender.IsDead)
            {

                if (currentDefender == playerStats)
                {
                    player.SetAnim(PlayerState.DEATH);
                    //Debug.Log("Player is dead");
                    battleLogUI.AddLog("YOU DIE");
                    yield return new WaitForSeconds(1f);
                    SceneManager.LoadScene(SceneName.RoomScene.ToString());
                }
                else
                {
                    monster.SetAnim(MonsterState.DEATH);
                    battleLogUI.AddLog(currentDefender.Name + "�� óġ�ߴ�!");
                    player.SetAnim(PlayerState.MOVE);
                    //Debug.Log("Monster is dead");
                    Destroy(monsterTransform.GetChild(0).gameObject, 1f);
                }

                yield return new WaitForSeconds(1f);

                //��� ī�޶� �̵�����
                parallaxBackground.Camera_Move = true;
                yield break;
            }

            // �� ��ü
            (currentAttacker, currentDefender) = (currentDefender, currentAttacker);


            //���� ��Ÿ�� �̰� ���߿� �ִϸ��̼� �ӵ��� ����ȭ���ٿ�����
            yield return new WaitForSeconds(attackDelay);
        }
    }

    //���� ���� 
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
