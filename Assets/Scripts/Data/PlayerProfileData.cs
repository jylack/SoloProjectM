using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerProfileData
{
    public string uid;
    public string nickname;
    public int energy;
    public int diamonds;
    public int gold;
    public PlayerStatsData stats;
    public PlayerAppearanceData appearance;
    public List<PlayerItemData> items;
    public List<PlayerSkillData> skills;

    public static PlayerProfileData CreateDefault(string uid, string nickname)
    {
        return new PlayerProfileData
        {
            uid = uid,
            nickname = nickname,
            energy = 120,
            diamonds = 50,
            gold = 1000,
            stats = PlayerStatsData.CreateDefault(),
            appearance = PlayerAppearanceData.CreateDefault(),
            items = new List<PlayerItemData>
            {
                new PlayerItemData { itemId = "potion_small", quantity = 3 },
                new PlayerItemData { itemId = "elixir_basic", quantity = 1 }
            },
            skills = new List<PlayerSkillData>
            {
                new PlayerSkillData { skillId = "basic_slash", level = 1 }
            }
        };
    }
}

[Serializable]
public class PlayerStatsData
{
    public int level;
    public int maxHp;
    public int attack;
    public int speed;
    public int attackCount;
    public int defense;

    public static PlayerStatsData CreateDefault()
    {
        return new PlayerStatsData
        {
            level = 1,
            maxHp = 1000,
            attack = 10,
            speed = 5,
            attackCount = 1,
            defense = 0
        };
    }
}

[Serializable]
public class PlayerAppearanceData
{
    public string characterPrefabId;
    public string portraitId;
    public string bodyId;
    public string eyeId;
    public string hairId;
    public string clothId;
    public string weaponId;
    public string shieldId;

    public static PlayerAppearanceData CreateDefault()
    {
        return new PlayerAppearanceData
        {
            characterPrefabId = string.Empty,
            portraitId = string.Empty,
            bodyId = string.Empty,
            eyeId = string.Empty,
            hairId = string.Empty,
            clothId = string.Empty,
            weaponId = string.Empty,
            shieldId = string.Empty
        };
    }
}

[Serializable]
public class PlayerItemData
{
    public string itemId;
    public int quantity;
}

[Serializable]
public class PlayerSkillData
{
    public string skillId;
    public int level;
}