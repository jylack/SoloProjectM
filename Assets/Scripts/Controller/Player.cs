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


    //장비 등등 받아서 스텟 업데이트 해줌.
    //어떤 공격할때 확률적으로 크리티컬같은거 뜨면 계산해서 데미지 넘겨줌.
    //스텟을 넘겨주고 애니메이션을 넘겨줌.
    //사용가능한 스킬 리스트 사용

    //기본 애니메이션 세팅
    public void SetAnim(PlayerState playerState)
    {
        animCtrl.PlayAnimation(playerState, 0);
    }

    //공격 애니메이션 세팅
    public void SetAtkAnim(PlayerState playerState, AttackType attackType)
    {
        animCtrl.PlayAnimation(playerState, (int)attackType);
    }

    //번외 애니메이션 세팅
    public void SetOtherAnim(PlayerState playerState, OtherType otherType)
    {
        animCtrl.PlayAnimation(playerState, (int)otherType);
    }

    //추가 스탯 적용
    public void StateApply(UnitStats stats)
    {
        ApplyState = stats;
    }
    //원본스텟 반환
    public UnitStats GetOriginStats()
    {
        return stats;
    }
    //적용된 스텟 반환
    public UnitStats GetApplyState()
    {
        return ApplyState;
    }
    //적용된 스텟과 원본스텟을 합쳐서 반환
    public UnitStats GetStats()
    {
        UnitStats value = new UnitStats(stats.Name, stats.MaxHp + ApplyState.MaxHp, stats.Attack + ApplyState.Attack, stats.Speed + ApplyState.Speed, stats.AttackCount + ApplyState.AttackCount);

        return value;
    }

}
