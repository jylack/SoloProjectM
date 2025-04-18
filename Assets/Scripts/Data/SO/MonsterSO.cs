using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "GameData/Monster")]
public class MonsterSO : ScriptableObject
{
    public GameObject _prefab;

    public string _name;
    public int _level = 1;
    public int _maxHp;
    public int _attack;
    public int _speed;
    public int _attackCount;
    public int _defense;

    public UnitStats Stats => new UnitStats(_name, _maxHp, _attack, _defense, _speed, _attackCount);
}
