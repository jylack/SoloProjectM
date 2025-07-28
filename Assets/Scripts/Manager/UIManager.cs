using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("플레이어 UI")]
    [SerializeField] private Slider playerHpBar;
    [SerializeField] private TextMeshProUGUI playerHpText;

    [Header("몬스터 UI")]
    [SerializeField] private Slider monsterHpBar;
    [SerializeField] private TextMeshProUGUI monsterHpText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 플레이어의 HP 바와 텍스트를 갱신합니다.
    /// </summary>
    /// <param name="currentHp">현재 HP 값</param>
    /// <param name="maxHp">최대 HP 값</param>
    public void UpdatePlayerHp(int currentHp, int maxHp)
    {
        if (playerHpBar != null)
            playerHpBar.value = (float)currentHp / maxHp;

        if (playerHpText != null)
            playerHpText.text = $"{currentHp} / {maxHp}";
    }

    /// <summary>
    /// 몬스터의 HP 바와 텍스트를 갱신합니다.
    /// </summary>
    /// <param name="currentHp">현재 HP 값</param>
    /// <param name="maxHp">최대 HP 값</param>
    public void UpdateMonsterHp(int currentHp, int maxHp)
    {
        if (monsterHpBar != null)
            monsterHpBar.value = (float)currentHp / maxHp;

        if (monsterHpText != null)
            monsterHpText.text = $"{currentHp} / {maxHp}";
    }
}
