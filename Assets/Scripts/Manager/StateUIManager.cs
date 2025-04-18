using System.Collections;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class StateUIManager : MonoBehaviour
{
    [Header("플레이어 스탯 UI")]
    [SerializeField] TextMeshProUGUI lvl;
    [SerializeField] TextMeshProUGUI hp;
    [SerializeField] TextMeshProUGUI atk;
    [SerializeField] TextMeshProUGUI def;
    [SerializeField] TextMeshProUGUI speed;

    private UnitStats playerStats;

    public static StateUIManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(InitStats());
    }

    private IEnumerator InitStats()
    {
        yield return new WaitUntil(() => GameManager.instance != null && GameManager.instance.PlayerCurrentStats != null);

        playerStats = GameManager.instance.PlayerCurrentStats; // 실시간 참조
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (playerStats == null) return;
        //Debug.Log("RefreshUI called");
        lvl.text = "LV. " + playerStats.Level;
        hp.text = ChangeNumber(playerStats.CurrentHp) + " / " + ChangeNumber(playerStats.MaxHp);
        atk.text = ChangeNumber(playerStats.Attack);
        def.text = ChangeNumber(playerStats.Defense);
        //speed.text = playerStats.Speed.ToString();
    }

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
