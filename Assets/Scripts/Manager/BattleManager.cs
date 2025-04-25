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
        parallaxBackground.Camera_Move = false;
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
        //누군가 죽을때까지 싸운다.
        while (!playerStats.IsDead && !monsterStats.IsDead)
        {
            //공격자가 공격횟수만큼 공격
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


                // battleLogUI.AddLog(currentAttacker.Name + "의 공격! " + currentDefender.Name + "에게 " + currentAttacker.Attack + "의 피해를 입혔습니다! ");
                //Debug.Log(currentAttacker.Name + "의 공격! " + currentDefender.Name + "에게 " + currentAttacker.Attack + "의 피해를 입혔습니다!");

                //방어자 죽었는가 판별
                if (currentDefender.IsDead) break;


                //공격 딜레이 이걸 나중에 애니메이션 속도랑 동기화해줄예정임 //각애니메이션마다 길이가 다름.
                yield return new WaitForSeconds(attackDelay);
            }

            //방어자가 죽었는가 판별
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
                    battleLogUI.AddLog(currentDefender.Name + "를 처치했다!");
                    player.SetAnim(PlayerState.MOVE);
                    //Debug.Log("Monster is dead");
                    Destroy(monsterTransform.GetChild(0).gameObject, 1f);
                }

                yield return new WaitForSeconds(1f);

                //배경 카메라 이동시작
                parallaxBackground.Camera_Move = true;
                yield break;
            }

            // 턴 교체
            (currentAttacker, currentDefender) = (currentDefender, currentAttacker);


            //공격 쿨타임 이걸 나중에 애니메이션 속도랑 동기화해줄예정임
            yield return new WaitForSeconds(attackDelay);
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
