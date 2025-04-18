using UnityEngine;

public class Player : MonoBehaviour
{
    public UnitStats stats = new UnitStats("User", 10000, 10, 0, 5, 1);
    PlayerState playerState = PlayerState.IDLE;

    //장비 등등 받아서 스텟 업데이트 해줌.
    //어떤 공격할때 확률적으로 크리티컬같은거 뜨면 계산해서 데미지 넘겨줌.
    //스텟을 넘겨주고 애니메이션을 넘겨줌.
    //사용가능한 스킬 리스트 사용


}
