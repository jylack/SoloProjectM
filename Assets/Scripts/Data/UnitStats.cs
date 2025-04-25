using UnityEngine;

[System.Serializable]
public class UnitStats
{
    private string _name;
    private int _lvl = 1;
    private float _currentExp = 0;
    private float _maxExp;
    private int _currentHp;
    private int _maxHp;
    private int _attack;
    private int _defense;
    private int _speed;
    private int _attackCount; // 한 턴에 몇 번 공격하는지

    //private bool _isAtk = false; // 공격중인지

    public string Name => _name;

    public int Lvl => _lvl;
    public float CurrentExp => _currentExp;
    public float MaxExp => _maxExp;
    public int MaxHp => _maxHp;
    public int CurrentHp => _currentHp;
    public int Attack => _attack;
    public int Defense => _defense;
    public int Speed => _speed;
    public int AttackCount => _attackCount;
    public bool IsDead => _currentHp <= 0;
    //public bool IsAtk => _isAtk;

    public UnitStats(string name,int maxHp, int attack, int speed, int attackCount)
    {
        _name = name;
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

    //만들었다가 클래스 == 연산자 알아서 지움
    //public void SetAttack(bool isAtk)
    //{
    //    _isAtk = isAtk;
    //}

}
