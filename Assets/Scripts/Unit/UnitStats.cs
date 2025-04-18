using UnityEngine;

[System.Serializable]
public class UnitStats
{
    private string _name;
    private int _level = 1;
    private int _maxHp;
    private int _currentHp;
    private int _attack;
    private int _speed;
    private int _attackCount;
    private int _defense;

    public string Name => _name;
    public int Level => _level;
    public int MaxHp => _maxHp;
    public int CurrentHp => _currentHp;
    public int Attack => _attack;
    public int Defense => _defense;
    public int Speed => _speed;
    public int AttackCount => _attackCount;
    public bool IsDead => _currentHp <= 0;

    public UnitStats(string name, int maxHp, int attack, int def, int speed, int attackCount)
    {
        _name = name;
        _maxHp = maxHp;
        _currentHp = maxHp;
        _attack = attack;
        _defense = def;
        _speed = speed;
        _attackCount = attackCount;
    }
    public void IncrementAttackCount(int amount)
    {
        _attackCount += amount;
    }
    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(1, damage - _defense);
        _currentHp = Mathf.Max(0, _currentHp - finalDamage);
    }

    public void Heal(int healAmount)
    {
        _currentHp = Mathf.Min(_maxHp, _currentHp + healAmount);
    }

    public UnitStats Clone()
    {
        return new UnitStats(_name, _maxHp, _attack, _defense, _speed, _attackCount)
        {
            _currentHp = _currentHp,
            _level = _level            
        };
    }

    public void ApplyStatBuff(int hp, int atk, int def, int spd)
    {
        _maxHp += hp;
        _currentHp += hp;
        _attack += atk;
        _defense += def;
        _speed += spd;
    }

    public void ModifyAttack(int amount)
    {
        _attack += amount;
    }

    public void ModifyDefense(int amount)
    {
        _defense += amount;
    }
}
