using UnityEngine;

[System.Serializable]
public class UnitStats
{
    private int _maxHp;
    private int _currentHp;
    private int _attack;
    private int _speed;
    private int _attackCount; // 한 턴에 몇 번 공격하는지

    public int MaxHp => _maxHp;
    public int CurrentHp => _currentHp;
    public int Attack => _attack;
    public int Speed => _speed;
    public int AttackCount => _attackCount;
    public bool IsDead => _currentHp <= 0;


    public UnitStats(int maxHp, int attack, int speed, int attackCount)
    {
        _maxHp = maxHp;
        _currentHp = maxHp;
        _attack = attack;
        _speed = speed;
        _attackCount = attackCount;
    }

    public void TakeDamage(int damage)
    {
        _currentHp = Mathf.Max(0, _currentHp - damage);
    }

    public void Heal(int healAmount)
    {
        _currentHp = Mathf.Min(_maxHp, _currentHp + healAmount);
    }


}
