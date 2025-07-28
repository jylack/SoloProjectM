// BattleManager.cs
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform monsterTransform;
    [SerializeField] private ParallaxBackground parallaxBackground;
    [SerializeField] private GameObject LogUI;

    [Header("Timing")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float attackDelay = 0.5f;

    private LogUI _logUI;
    private Player _player;
    private Monster _monster;
    private ICombatant _playerStats;
    private ICombatant _monsterStats;
    private Queue<ICombatant> _turnQueue;

    Coroutine combat;

    private void Awake()
    {
        _logUI = LogUI.GetComponent<LogUI>();
    }

    private void OnEnable()
    {
        // 컴포넌트 & 스탯 초기화
        _player = playerTransform.GetComponent<Player>();
        _monster = monsterTransform.GetComponent<Monster>();
        _playerStats = _player.GetStats();
        _monsterStats = _monster.GetStats();

        // HP 변경 UI 바인딩 예시
        _playerStats.OnHpChanged += (cur, max) => UIManager.Instance.UpdatePlayerHp(cur, max);
        _monsterStats.OnHpChanged += (cur, max) => UIManager.Instance.UpdateMonsterHp(cur, max);
        
        combat = StartCoroutine(StartBattleSequence());
        Debug.Log(combat != null ? "전투 시작!" : "전투 시작 실패");
    }

    private IEnumerator StartBattleSequence()
    {
        if (GameManager.Instance == null) yield return new WaitUntil(() => GameManager.Instance != null);

        // 전투 시작 로그
        _logUI.AddDayLog(GameManager.Instance.CurrentDay, "전투 시작!");

        Debug.Log("??");
        // 연출: 플레이어/몬스터 접근
        yield return MoveOverTime(playerTransform, playerTransform.position,
                                 playerTransform.position + Vector3.right * 1f, moveDuration);

        var monsterStart = monsterTransform.position + Vector3.right * 2.5f;
        yield return MoveOverTime(monsterTransform, monsterStart,
                                 monsterStart + Vector3.left * 3.5f, moveDuration);

        yield return new WaitForSeconds(attackDelay);
        parallaxBackground.cameraMove = false;

        // 턴 큐 초기화 & 전투 루프 시작
        InitTurnQueue();
        StartCoroutine(CombatLoop());
    }

    private void InitTurnQueue()
    {
        // 속도 내림차순 정렬 후 큐에 넣기
        var ordered = new List<ICombatant> { _playerStats, _monsterStats }
                      .OrderByDescending(u => u.Speed);
        _turnQueue = new Queue<ICombatant>(ordered);
    }

    private IEnumerator CombatLoop()
    {
        while (_turnQueue.Count > 0)
        {
            var actor = _turnQueue.Dequeue();
            var defender = actor == _playerStats ? _monsterStats : _playerStats;

            if (actor.IsDead)
                continue;

            actor.ResetActions();

            // 남은 행동만큼 반복
            while (actor.ActionsRemaining > 0 && !defender.IsDead)
            {
                // 공격 애니메이션
                if (actor == _playerStats) _player.SetAnim(PlayerState.ATTACK);
                else _monster.SetAnim(MonsterState.Atk1);

                // 피해 적용
                defender.TakeDamage(actor.Attack);
                _logUI.AddLog($"{actor.Name} → {defender.Name} : {actor.Attack} 피해");

                actor.ActionsRemaining--;
                yield return new WaitForSeconds(attackDelay);
            }

            // 사망 처리
            if (defender.IsDead)
            {
                yield return HandleDeath(defender);
                yield break;
            }

            // 다시 큐에 넣기
            if (!actor.IsDead)
                _turnQueue.Enqueue(actor);

            yield return new WaitForSeconds(attackDelay);
        }
    }

    private IEnumerator HandleDeath(ICombatant fallen)
    {
        if (fallen == _playerStats)
        {
            _player.SetAnim(PlayerState.DEATH);
            _logUI.AddLog("YOU DIED");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(nameof(SceneNames.RoomScene));
        }
        else
        {
            _monster.SetAnim(MonsterState.Death);
            _logUI.AddLog($"{fallen.Name} 처치!");
            yield return new WaitForSeconds(1f);
            Destroy(monsterTransform.GetChild(0).gameObject, 1f);
            parallaxBackground.cameraMove = true;
        }
    }

    private IEnumerator MoveOverTime(Transform t, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            t.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        t.position = to;
    }
}
