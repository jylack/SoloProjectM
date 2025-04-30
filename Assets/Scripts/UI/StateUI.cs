using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StateUI : MonoBehaviour
{
    UnitStats _player;

    [SerializeField] TextMeshProUGUI LvlText;
    [SerializeField] TextMeshProUGUI HpText;
    [SerializeField] TextMeshProUGUI AtkText;
    [SerializeField] TextMeshProUGUI DefText;
    [SerializeField] Slider ExpSlider;
    [SerializeField] Slider HpSlider;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayerSetting());
    }
    

    IEnumerator PlayerSetting()
    {
        yield return new WaitUntil(() => GameManager.instance.GetPlayer() != null); // 전투 시작 대기
        //모든 추가능력치 다 합쳐진거 호출
        _player = GameManager.instance.GetPlayer().GetStats();

        LvlText.text = "LV. " + _player.Lvl;
        HpText.text = ChangeNumber(_player.CurrentHp) + " / " + ChangeNumber(_player.MaxHp);
        AtkText.text = ChangeNumber(_player.Attack);
        DefText.text = ChangeNumber(_player.Defense);

        ExpSlider.value = _player.CurrentExp / _player.MaxExp;
        HpSlider.value = (float)_player.CurrentHp / (float)_player.MaxHp;
    }

    public void RefreshUI(UnitStats player)
    {
        if (player == null) return;
        //Debug.Log("RefreshUI called");
        LvlText.text = "LV. " + player.Lvl;
        HpText.text = ChangeNumber(player.CurrentHp) + " / " + ChangeNumber(player.MaxHp);
        AtkText.text = ChangeNumber(player.Attack);
        DefText.text = ChangeNumber(player.Defense);
        //speed.text = playerStats.Speed.ToString();

        ExpSlider.value = player.CurrentExp / player.MaxExp;
        HpSlider.value = (float)player.CurrentHp / (float)player.MaxHp;
        Debug.Log(player.CurrentHp);
        //Debug.Log(HpSlider.value);
    }
    //추후 1만 넘어가면 K 단위로 나눌거임

    private string ChangeNumber(int value)
    {
        if (value >= 1_000_000)
            return (value / 1_000_000f).ToString("0.0") + "M";
        else if (value >= 10_000)
            return (value / 1_000f).ToString("0.0") + "K";
        else
            return value.ToString();
    }

}
