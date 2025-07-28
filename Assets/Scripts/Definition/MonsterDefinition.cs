using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터 타입별 정의를 담는 ScriptableObject
/// </summary>
[CreateAssetMenu(menuName = "Game/Monster Definition")]
public class MonsterDefinition : ScriptableObject
{
    public string monsterName;
    public GameObject prefab;
    public int maxHp;
    public int attack;
    public int speed;
    public int attackCount;
}

/// <summary>
/// 전투 시스템에서 사용할 공통 인터페이스를 구현한 몬스터 베이스 클래스
/// </summary>
public abstract class MonsterBase : MonoBehaviour, ICombatant
{
    protected UnitStats stats;

    /// <summary>
    /// ScriptableObject 데이터를 기반으로 초기화
    /// </summary>
    public virtual void Initialize(MonsterDefinition def)
    {
        stats = new UnitStats(def.monsterName,
                              def.maxHp,
                              def.attack,
                              def.speed,
                              def.attackCount);
    }

    public string Name => stats.Name;
    public int Speed => stats.Speed;
    public int Attack => stats.Attack;
    public int MaxActionsPerTurn => stats.MaxActionsPerTurn;
    public int ActionsRemaining 
    { 
        get => stats.ActionsRemaining;
        set => stats.ActionsRemaining = value; 
    }

    public bool IsDead => stats.IsDead;
    public event System.Action<int, int> OnHpChanged
    {
        add { stats.OnHpChanged += value; }
        remove { stats.OnHpChanged -= value; }
    }

    public void ResetActions() => stats.ResetActions();
    public void TakeDamage(int a) => stats.TakeDamage(a);
}

/// <summary>
/// 팩토리 클래스: 정의된 MonsterDefinition으로부터 몬스터 인스턴스를 생성
/// </summary>
public static class MonsterFactory
{
    /// <summary>
    /// 지정된 위치에 몬스터를 생성하고 초기화까지 수행
    /// </summary>
    public static MonsterBase Spawn(MonsterDefinition def, Transform parent, Vector3 position)
    {
        GameObject go = Object.Instantiate(def.prefab, parent);
        go.transform.localPosition = position;
        MonsterBase monster = go.GetComponent<MonsterBase>();
        if (monster == null)
        {
            Debug.LogError("Prefab에 MonsterBase를 상속한 컴포넌트가 필요합니다.");
            return null;
        }
        monster.Initialize(def);
        return monster;
    }
}
