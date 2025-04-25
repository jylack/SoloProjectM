using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateUIManager : MonoBehaviour
{
    [Header("�÷��̾� ���� UI")]
    [SerializeField] TextMeshProUGUI lvl;
    [SerializeField] TextMeshProUGUI hp;
    [SerializeField] TextMeshProUGUI atk;
    [SerializeField] TextMeshProUGUI def;
    [SerializeField] TextMeshProUGUI speed;

    [SerializeField] Slider ExpSlider;
    [SerializeField] Slider HPSlider;


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

        playerStats = GameManager.instance.PlayerCurrentStats; // �ǽð� ����
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

        ExpSlider.value = playerStats.CurrentExp / playerStats.MaxExp;
        HPSlider.value = (float)playerStats.CurrentHp / (float)playerStats.MaxHp;
        Debug.Log(HPSlider.value);
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
