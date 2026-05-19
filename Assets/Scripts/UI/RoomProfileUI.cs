using TMPro;
using UnityEngine;

public class RoomProfileUI : MonoBehaviour
{
    [Header("Account")]
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text diamondsText;
    [SerializeField] private TMP_Text energyText;

    [Header("Character Stats")]
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text defenseText;

    [Header("Character Preview")]
    [SerializeField] private CharacterPrefabPreview equipCharacterPreview;
    [SerializeField] private CharacterPrefabPreview adventureCharacterPreview;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[RoomProfileUI] GameManager.Instance is null.");
            return;
        }

        PlayerProfileData profile = GameManager.Instance.PlayerProfile;

        if (profile == null)
        {
            Debug.LogError("[RoomProfileUI] PlayerProfile is null.");
            return;
        }

        RefreshAccount(profile);
        RefreshCharacterStats(profile.stats);

        if(equipCharacterPreview != null)
        {
             equipCharacterPreview.Apply(profile.appearance);
             equipCharacterPreview.PlayIdle();
        }
        if(adventureCharacterPreview != null)
        {
            adventureCharacterPreview.Apply(profile.appearance);
            adventureCharacterPreview.PlayMove();
        }   
    }

    private void RefreshAccount(PlayerProfileData profile)
    {
        if (nicknameText != null) nicknameText.text = profile.nickname;
        if (goldText != null) goldText.text = FormatStat(profile.gold);
        if (diamondsText != null) diamondsText.text = FormatStat(profile.diamonds);
        if (energyText != null) energyText.text = FormatStat(profile.energy);
    }

    private void RefreshCharacterStats(PlayerStatsData stats)
    {
        if (stats == null)
        {
            Debug.LogError("[RoomProfileUI] PlayerStatsData is null.");
            return;
        }

        if (hpText != null) hpText.text = FormatStat(stats.maxHp);
        if (attackText != null) attackText.text = FormatStat(stats.attack);
        if (defenseText != null) defenseText.text = FormatStat(stats.defense);
    }

    private string FormatStat(int value)
    {
        if (value >= 1000)
        {
            return (value / 1000f).ToString("0.#") + "k";
        }

        return value.ToString();
    }
}
