using UnityEngine;

public enum AttackType
{
    Normal,
    NormalSKill,
    BowAtk,
    BowSkill,
    Magic,
    MagicSkill,
}

public enum OtherType
{
    Conecntrate,
    Buff,
    Sit
}

public class Player : MonoBehaviour
{
    UnitStats stats = new UnitStats("User", 1000, 10, 5, 1);
    UnitStats ApplyState = new UnitStats("", 0, 0, 0, 0);

    [SerializeField] SPUM_Prefabs animCtrl;


    //��� ��� �޾Ƽ� ���� ������Ʈ ����.
    //� �����Ҷ� Ȯ�������� ũ��Ƽ�ð����� �߸� ����ؼ� ������ �Ѱ���.
    //������ �Ѱ��ְ� �ִϸ��̼��� �Ѱ���.
    //��밡���� ��ų ����Ʈ ���

    //�⺻ �ִϸ��̼� ����
    public void SetAnim(PlayerState playerState)
    {
        animCtrl.PlayAnimation(playerState, 0);
    }

    //���� �ִϸ��̼� ����
    public void SetAtkAnim(PlayerState playerState, AttackType attackType)
    {
        animCtrl.PlayAnimation(playerState, (int)attackType);
    }

    //���� �ִϸ��̼� ����
    public void SetOtherAnim(PlayerState playerState, OtherType otherType)
    {
        animCtrl.PlayAnimation(playerState, (int)otherType);
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
