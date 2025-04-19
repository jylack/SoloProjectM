using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{    
    [SerializeField] MonsterSO monsterSO;
    private UnitStats _unitStats;

    public UnitStats Stats => _unitStats;

    private void Awake()
    {
        _unitStats = monsterSO.Stats;
        //Debug.Log(_unitStats.Name);
    }
    
    
}
