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
    [SerializeField] private GameObject LogUIObj;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private EncounterManager encounterManager;

    [Header("Timing")]
    [SerializeField] private float moveDuration = 5.5f;
    [SerializeField] private float attackDelay = 0.5f;

    private LogUI _logUI;
    private Player _player;
    private MonsterBase _monster;
    private ICombatant _playerStats;
    private ICombatant _monsterStats;
    private Queue<ICombatant> _turnQueue;
    private Animator _monsterAnimator;
    private MonsterDefinition _currentMonsterDefinition;
    private bool _playerHpBound;

    Coroutine combat = null;

    private void Awake()
    {
        _logUI = LogUIObj.GetComponent<LogUI>();
    }


    private void Start()
    {
        // 컴포넌트 & 스탯 초기화
        EnsureManagerReferences();
        _player = playerTransform.GetComponent<Player>();
        _playerStats = _player.GetStats();
        combat = StartCoroutine(StartBattleSequence());
    }

    private void OnDestroy()
    {
        if (_playerHpBound && _playerStats != null)
        {
            _playerStats.OnHpChanged -= OnPlayerHpChanged;
        }

        if (_monsterStats != null)
        {
            _monsterStats.OnHpChanged -= OnMonsterHpChanged;
        }
    }

    private void EnsureManagerReferences()
    {
        if (stageManager == null)
            stageManager = StageManager.Instance;

        if (encounterManager == null)
            encounterManager = EncounterManager.Instance;
    }

    private IEnumerator StartBattleSequence()
    {
        Debug.Log("전투 시작 시퀀스 시작");
        EnsureManagerReferences();

        if (stageManager == null)
        {
            yield return new WaitUntil(() => StageManager.Instance != null);
            stageManager = StageManager.Instance;
        }

        if (encounterManager == null)
        {
            yield return new WaitUntil(() => EncounterManager.Instance != null);
            encounterManager = EncounterManager.Instance;
        }

        if (!PrepareMonsterForToday())
        {
            yield break;
        }

        if (UIManager.Instance == null)
            yield return new WaitUntil(() => UIManager.Instance != null);
        // HP 변경 UI 바인딩
        BindHpEvents();

        Debug.Log("전투 시작 시퀀스");

        // 전투 시작 로그
        int currentDay = stageManager != null ? stageManager.CurrentDay : 0;
        _logUI.AddDayLog(currentDay, "전투 시작!");

        Debug.Log(playerTransform.gameObject.name);

        // 연출 플레이어/몬스터 접근
        yield return MoveOverTime(playerTransform, playerTransform.position,
                                 playerTransform.position + Vector3.right * 1f, moveDuration);

        Debug.Log(monsterTransform.gameObject.name);

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
                if (actor == _playerStats)
                {
                    _player.SetAnim(PlayerState.ATTACK);
                }
                else PlayMonsterAnimation(MonsterState.Atk1);



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


    private IEnumerator EndBattleRoutine()
    {
        EnsureManagerReferences();

        if (stageManager != null)
        {
            stageManager.AdvanceDay();
            _logUI.AddDayLog(stageManager.CurrentDay, $"Stage {stageManager.CurrentStage}");
        }
        else
        {
            Debug.LogWarning("BattleManager: StageManager reference missing when ending battle.");
        }

        combat = StartCoroutine(StartBattleSequence());
        yield return null;
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
            PlayMonsterAnimation(MonsterState.Death);
            _logUI.AddLog($"{fallen.Name} 처치!");

            yield return new WaitForSeconds(1f);

            DestroyCurrentMonster();
            parallaxBackground.cameraMove = true;
            yield return EndBattleRoutine();
        }
    }

    private bool PrepareMonsterForToday()
    {
        if (encounterManager == null)
        {
            Debug.LogWarning("BattleManager: EncounterManager reference is missing.");
            return false;
        }

        var encounters = encounterManager.GetRandomEncounters(1);
        if (encounters == null || encounters.Count == 0)
        {
            Debug.LogWarning("BattleManager: No monster definitions available for today's encounter.");
            return false;
        }

        var monsterDefinition = encounters[0];
        if (monsterDefinition == null)
        {
            Debug.LogWarning("BattleManager: EncounterManager returned a null MonsterDefinition.");
            return false;
        }

        ClearExistingMonster();

        MonsterBase spawned = MonsterFactory.Spawn(monsterDefinition, monsterTransform, Vector3.zero);
        if (spawned == null)
        {
            Debug.LogError("BattleManager: Failed to spawn monster from definition.");
            return false;
        }

        _monster = spawned;
        _monsterStats = spawned;
        _monsterAnimator = spawned.GetComponent<Animator>();
        if (_monsterAnimator == null)
        {
            _monsterAnimator = spawned.GetComponentInChildren<Animator>();
        }

        _currentMonsterDefinition = monsterDefinition;

        return true;
    }

    private void ClearExistingMonster()
    {
        if (_monsterStats != null)
        {
            _monsterStats.OnHpChanged -= OnMonsterHpChanged;
            _monsterStats = null;
        }

        _monster = null;
        _monsterAnimator = null;
        _currentMonsterDefinition = null;

        for (int i = monsterTransform.childCount - 1; i >= 0; i--)
        {
            var child = monsterTransform.GetChild(i);
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void DestroyCurrentMonster()
    {
        if (_monsterStats != null)
        {
            _monsterStats.OnHpChanged -= OnMonsterHpChanged;
            _monsterStats = null;
        }

        if (_monster != null)
        {
            Destroy(_monster.gameObject);
            _monster = null;
        }

        for (int i = monsterTransform.childCount - 1; i >= 0; i--)
        {
            var child = monsterTransform.GetChild(i);
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }

        _monsterAnimator = null;
        _currentMonsterDefinition = null;
    }

    private void BindHpEvents()
    {
        if (_playerStats != null && !_playerHpBound)
        {
            _playerStats.OnHpChanged += OnPlayerHpChanged;
            _playerHpBound = true;

            if (UIManager.Instance != null && _playerStats is UnitStats playerStats)
            {
                UIManager.Instance.UpdatePlayerHp(playerStats.CurrentHp, playerStats.MaxHp);
            }
        }

        if (_monsterStats != null)
        {
            _monsterStats.OnHpChanged += OnMonsterHpChanged;

            if (UIManager.Instance != null && _currentMonsterDefinition != null)
            {
                UIManager.Instance.UpdateMonsterHp(_currentMonsterDefinition.maxHp, _currentMonsterDefinition.maxHp);
            }
        }
    }

    private void OnPlayerHpChanged(int currentHp, int maxHp)
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UpdatePlayerHp(currentHp, maxHp);
    }

    private void OnMonsterHpChanged(int currentHp, int maxHp)
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateMonsterHp(currentHp, maxHp);
    }

    private void PlayMonsterAnimation(MonsterState state)
    {
        if (_monsterAnimator != null)
        {
            _monsterAnimator.SetTrigger(state.ToString());
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
