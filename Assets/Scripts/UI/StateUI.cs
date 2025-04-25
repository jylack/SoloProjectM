using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        yield return new WaitUntil(() => GameManager.instance.GetPlayer() != null); // ���� ���� ���
        //��� �߰��ɷ�ġ �� �������� ȣ��
        player = GameManager.instance.GetPlayer().GetStats();

        LvlText.text =  "LV." + player.Lvl.ToString();
        HpText.text = player.CurrentHp.ToString() +"/" + player.MaxHp.ToString();
        AtkText.text = player.Attack.ToString();
        DefText.text = player.Defense.ToString();
    }

    


    //���� 1�� �Ѿ�� K ������ ��������

}
