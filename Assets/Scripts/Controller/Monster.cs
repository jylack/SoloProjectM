using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    IDLE,
    ATK1,
    ATK2,
    DAMAGE,
    MOVE,
    DEATH,    
}

public class Monster : MonoBehaviour
{
    string Name;
    UnitStats stats = new UnitStats("Monster", 100, 10, 5, 1);
    UnitStats ApplyState = new UnitStats("", 0, 0, 0, 0);

    [SerializeField] Animator animCtrl;

    private void Awake()
    {
        
    }

    //�⺻ �ִϸ��̼� ����
    public void SetAnim(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.IDLE:
                animCtrl.SetTrigger("Idle");
                break;
            case MonsterState.ATK1:
                animCtrl.SetTrigger("Atk1");
                break;
            case MonsterState.ATK2:
                animCtrl.SetTrigger("Atk2");
                break;
            case MonsterState.DAMAGE:
                animCtrl.SetTrigger("Hit");
                break;
            case MonsterState.MOVE:
                animCtrl.SetTrigger("Run");
                break;
            case MonsterState.DEATH:
                animCtrl.SetTrigger("Death");
                break;
        }
    }

    //�߰� ���� ����
    public void StateApply(UnitStats stats)
    {
        ApplyState = stats;
    }
    //�������� ��ȯ
    public UnitStats GetOriginStats()
    {
        return stats;
    }
    //����� ���� ��ȯ
    public UnitStats GetApplyState()
    {
        return ApplyState;
    }
    //����� ���ݰ� ���������� ���ļ� ��ȯ
    public UnitStats GetStats()
    {
        UnitStats value = new UnitStats(stats.Name, stats.MaxHp + ApplyState.MaxHp, stats.Attack + ApplyState.Attack, stats.Speed + ApplyState.Speed, stats.AttackCount + ApplyState.AttackCount);

        return value;
    }

}
