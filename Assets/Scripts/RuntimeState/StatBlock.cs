using System;
using UnityEngine;

[Serializable]
public class StatBlock
{
    public int maxHp = 100;
    public int currentHp = 100;
    public int attack = 10;
    public int defense = 0;
    public int attackSpeed = 1;
    public int critChance = 0;
    public int critDamage = 150;

    public StatBlock Clone()
    {
        return new StatBlock
        {
            maxHp = maxHp,
            currentHp = currentHp,
            attack = attack,
            defense = defense,
            attackSpeed = attackSpeed,
            critChance = critChance,
            critDamage = critDamage,
        };
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        currentHp = Mathf.Min(maxHp, currentHp + amount);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        currentHp = Mathf.Max(0, currentHp - amount);
    }

    public void AddStat(StatType statType, int value)
    {
        if (value == 0)
        {
            return;
        }

        switch (statType)
        {
            case StatType.MaxHp:
                maxHp += value;
                currentHp = Mathf.Clamp(currentHp, 0, maxHp);
                break;
            case StatType.Attack:
                attack += value;
                break;
            case StatType.Defense:
                defense += value;
                break;
            case StatType.AttackSpeed:
                attackSpeed += value;
                break;
            case StatType.CritChance:
                critChance += value;
                break;
            case StatType.CritDamage:
                critDamage += value;
                break;
        }
    }
}
