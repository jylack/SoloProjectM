using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform monsterTransform;
    [SerializeField] private ParallaxBackground parallaxBackground;
    [SerializeField] private GameObject LogUIObj;

    [Header("Timing")]
    [SerializeField] private float moveDuration = 1.2f;
    [SerializeField] private float attackDelay = 0.5f;

    private LogUI _logUI;
    private Player _player;
    private MonsterBase _monster;
    private ICombatant _playerStats;
    private ICombatant _monsterStats;
    private Queue<ICombatant> _turnQueue;
    private Animator _monsterAnimator;
    private bool _playerHpBound;
    private UnitStats _runtimePlayerStats;
    private EncounterData _currentEncounter;
    private InGameSceneController _sceneController;

    private void Awake()
    {
        if (LogUIObj != null)
        {
            _logUI = LogUIObj.GetComponent<LogUI>();
        }

        if (playerTransform != null)
        {
            _player = playerTransform.GetComponent<Player>();
        }
    }

    private void OnDestroy()
    {
        if (_playerHpBound && _playerStats != null)
        {
            _playerStats.OnHpChanged -= OnPlayerHpChanged;
            _playerHpBound = false;
        }

        if (_monsterStats != null)
        {
            _monsterStats.OnHpChanged -= OnMonsterHpChanged;
        }
    }

    public void StartEncounterBattle(EncounterData encounterData, InGameSceneController sceneController)
    {
        if (encounterData == null)
        {
            sceneController?.OnBattleFinished(new BattleResult { isWin = false });
            return;
        }

        _currentEncounter = encounterData;
        _sceneController = sceneController;

        StopAllCoroutines();
        StartCoroutine(StartBattleSequence());
    }

    private IEnumerator StartBattleSequence()
    {
        if (_player == null && playerTransform != null)
        {
            _player = playerTransform.GetComponent<Player>();
        }

        if (_player == null)
        {
            _sceneController?.OnBattleFinished(new BattleResult { isWin = false });
            yield break;
        }

        BuildPlayerStats();

        if (!PrepareMonsterForEncounter(_currentEncounter))
        {
            _sceneController?.OnBattleFinished(new BattleResult { isWin = false, remainingHp = RunManager.Instance.RunState.stats.currentHp });
            yield break;
        }

        BindHpEvents();

        if (_logUI != null)
        {
            _logUI.AddDayLog(_currentEncounter.day, "Battle Start");
        }

        if (playerTransform != null)
        {
            yield return MoveOverTime(playerTransform, playerTransform.position, playerTransform.position + Vector3.right * 1f, moveDuration);
        }

        if (monsterTransform != null)
        {
            var monsterStart = monsterTransform.position + Vector3.right * 2.5f;
            yield return MoveOverTime(monsterTransform, monsterStart, monsterStart + Vector3.left * 3.5f, moveDuration);
        }

        yield return new WaitForSeconds(attackDelay);

        if (parallaxBackground != null)
        {
            parallaxBackground.cameraMove = false;
        }

        InitTurnQueue();
        yield return CombatLoop();
    }

    private void BuildPlayerStats()
    {
        var runStats = RunManager.Instance != null ? RunManager.Instance.RunState.stats : null;
        if (runStats == null)
        {
            _runtimePlayerStats = _player.GetStats() as UnitStats;
            _playerStats = _runtimePlayerStats;
            return;
        }

        int speed = Mathf.Max(1, runStats.attackSpeed);
        _runtimePlayerStats = new UnitStats("Player", runStats.maxHp, runStats.attack, speed, 1);
        int missingHp = runStats.maxHp - runStats.currentHp;
        if (missingHp > 0)
        {
            _runtimePlayerStats.TakeDamage(missingHp);
        }

        _playerStats = _runtimePlayerStats;
    }

    private bool PrepareMonsterForEncounter(EncounterData encounterData)
    {
        ClearExistingMonster();

        var candidates = Resources.LoadAll<MonsterDefinition>("");
        if (candidates == null || candidates.Length == 0)
        {
            return false;
        }

        int index = Mathf.Abs((encounterData.day * 7) + encounterData.encounterType.GetHashCode()) % candidates.Length;
        var selected = candidates[index];
        if (selected == null || selected.prefab == null)
        {
            return false;
        }

        MonsterDefinition runtimeDef = ScriptableObject.CreateInstance<MonsterDefinition>();
        runtimeDef.monsterName = selected.monsterName;
        runtimeDef.prefab = selected.prefab;
        runtimeDef.maxHp = selected.maxHp + (encounterData.day * (encounterData.encounterType == EncounterType.Boss ? 20 : 8));
        runtimeDef.attack = selected.attack + (encounterData.day * (encounterData.encounterType == EncounterType.Boss ? 3 : 1));
        runtimeDef.speed = Mathf.Max(1, selected.speed);
        runtimeDef.attackCount = Mathf.Max(1, selected.attackCount);

        MonsterBase spawned = MonsterFactory.Spawn(runtimeDef, monsterTransform, Vector3.zero);
        if (spawned == null)
        {
            return false;
        }

        _monster = spawned;
        _monsterStats = spawned;
        _monsterAnimator = spawned.GetComponent<Animator>();
        if (_monsterAnimator == null)
        {
            _monsterAnimator = spawned.GetComponentInChildren<Animator>();
        }

        return true;
    }

    private void InitTurnQueue()
    {
        var ordered = new List<ICombatant> { _playerStats, _monsterStats }.OrderByDescending(u => u.Speed);
        _turnQueue = new Queue<ICombatant>(ordered);
    }

    private IEnumerator CombatLoop()
    {
        while (_turnQueue.Count > 0)
        {
            var actor = _turnQueue.Dequeue();
            var defender = actor == _playerStats ? _monsterStats : _playerStats;

            if (actor.IsDead)
            {
                continue;
            }

            actor.ResetActions();

            while (actor.ActionsRemaining > 0 && !defender.IsDead)
            {
                if (actor == _playerStats)
                {
                    _player.SetAnim(PlayerState.ATTACK);
                }
                else
                {
                    PlayMonsterAnimation(MonsterState.Atk1);
                }

                defender.TakeDamage(actor.Attack);
                if (_logUI != null)
                {
                    _logUI.AddLog(actor.Name + " -> " + defender.Name + " : " + actor.Attack);
                }

                actor.ActionsRemaining--;
                yield return new WaitForSeconds(attackDelay);
            }

            if (defender.IsDead)
            {
                yield return HandleDeath(defender);
                yield break;
            }

            if (!actor.IsDead)
            {
                _turnQueue.Enqueue(actor);
            }

            yield return new WaitForSeconds(attackDelay);
        }
    }

    private IEnumerator HandleDeath(ICombatant fallen)
    {
        if (fallen == _playerStats)
        {
            _player.SetAnim(PlayerState.DEATH);
            if (_logUI != null)
            {
                _logUI.AddLog("Defeat");
            }

            yield return new WaitForSeconds(1f);
            ReportBattleFinished(false);
        }
        else
        {
            PlayMonsterAnimation(MonsterState.Death);
            if (_logUI != null)
            {
                _logUI.AddLog("Victory");
            }

            yield return new WaitForSeconds(1f);
            DestroyCurrentMonster();

            if (parallaxBackground != null)
            {
                parallaxBackground.cameraMove = true;
            }

            ReportBattleFinished(true);
        }
    }

    private void ReportBattleFinished(bool isWin)
    {
        var result = new BattleResult
        {
            isWin = isWin,
            remainingHp = _runtimePlayerStats != null ? _runtimePlayerStats.CurrentHp : 0,
            rewards = isWin && _currentEncounter != null && _currentEncounter.rewards != null
                ? new List<RewardData>(_currentEncounter.rewards)
                : new List<RewardData>(),
        };

        if (RunManager.Instance != null)
        {
            RunManager.Instance.SetCurrentHp(result.remainingHp);
        }

        _sceneController?.OnBattleFinished(result);
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

        if (monsterTransform == null)
        {
            return;
        }

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

        _monsterAnimator = null;
    }

    private void BindHpEvents()
    {
        if (_playerStats != null && !_playerHpBound)
        {
            _playerStats.OnHpChanged += OnPlayerHpChanged;
            _playerHpBound = true;
            if (_runtimePlayerStats != null)
            {
                OnPlayerHpChanged(_runtimePlayerStats.CurrentHp, _runtimePlayerStats.MaxHp);
            }
        }

        if (_monsterStats != null)
        {
            _monsterStats.OnHpChanged += OnMonsterHpChanged;
        }
    }

    private void OnPlayerHpChanged(int currentHp, int maxHp)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdatePlayerHp(currentHp, maxHp);
        }
    }

    private void OnMonsterHpChanged(int currentHp, int maxHp)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMonsterHp(currentHp, maxHp);
        }
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
