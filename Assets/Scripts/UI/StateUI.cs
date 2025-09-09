using System.Collections;
using TMPro;
using UnityEngine;

public class StateUI : MonoBehaviour
{
    UnitStats player;

    [SerializeField] TextMeshProUGUI LvlText;
    [SerializeField] TextMeshProUGUI HpText;
    [SerializeField] TextMeshProUGUI AtkText;
    [SerializeField] TextMeshProUGUI DefText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayerSetting());
    }


    IEnumerator PlayerSetting()
    {
        Debug.Log("StateUI - PlayerSetting");
        yield return new WaitUntil(() => GameManager.Instance.GetPlayer() != null); // 전투 시작 대기
        //모든 추가능력치 다 합쳐진거 호출
        player = GameManager.Instance.GetPlayer().GetStats();

        LvlText.text = "LV." + player.Lv.ToString();
        HpText.text = player.CurrentHp.ToString() + "/" + player.MaxHp.ToString();
        AtkText.text = player.Attack.ToString();
        DefText.text = player.Defense.ToString();
        Debug.Log(AtkText.text);
        Debug.Log(DefText.text);
    }

    //추후 1만 넘어가면 K 단위로 나눌거임

}
