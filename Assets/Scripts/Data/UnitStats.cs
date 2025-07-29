// UnitStats.cs
using System;
using UnityEngine;

[Serializable]
public class UnitStats : ICombatant
{
    [SerializeField] private int _lv;
    [SerializeField] private string _name;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _attack;
    [SerializeField] private int _speed;
    [SerializeField] private int _attackCount;
    [SerializeField] private int _defense;

    private int _currentHp;

    public int Lv => _lv;
    public string Name => _name;
    public int Speed => _speed;
    public int Attack => _attack;
    public int MaxHp => _maxHp;
    public int CurrentHp => _currentHp;

    public int MaxActionsPerTurn => Mathf.Max(1, _attackCount);
    public int Defense => _defense;

    public int ActionsRemaining { get; set; }

    public bool IsDead => _currentHp <= 0;

    public event Action<int, int> OnHpChanged;

    public UnitStats(string name, int maxHp, int attack, int speed, int attackCount)
    {
        _name = name;
        _maxHp = maxHp;
        _attack = attack;
        _speed = speed;
        _attackCount = attackCount;
        _currentHp = maxHp;
    }

    public void LevelUp()
    {
        _lv++;
        _maxHp += 10; // 예시: 레벨업 시 최대 HP 증가
        _attack += 2; // 예시: 레벨업 시 공격력 증가
        _speed += 1; // 예시: 레벨업 시 속도 증가
        OnHpChanged?.Invoke(_currentHp, _maxHp);
    }

    public void ResetActions()
    {
        ActionsRemaining = MaxActionsPerTurn;
    }

    public void TakeDamage(int amount)
    {
        _currentHp = Mathf.Max(0, _currentHp - amount);
        OnHpChanged?.Invoke(_currentHp, _maxHp);
    }

    public void Heal(int amount)
    {
        _currentHp = Mathf.Min(_maxHp, _currentHp + amount);
        OnHpChanged?.Invoke(_currentHp, _maxHp);
    }
}
