
using System;
using UnityEngine;

[Serializable]
public class Skill
{
    public Sprite Icon { get; private set; } // 스킬 아이콘
    public string Name { get; private set; } // 스킬 이름
    public string Description { get; private set; } // 스킬 설명
    public int Damage { get; private set; } // 스킬 데미지
    public int Turns { get; private set; } // 스킬 사용가능 턴 
    public int ComboCount { get; private set; } // 스킬 콤보 수

    public Skill(Sprite icon , string name, string description, int damage, int turns, int comboCount)
    {
        Icon = icon;
        Name = name;
        Description = description;
        Damage = damage;
        Turns = turns;
        ComboCount = comboCount;
    }

    public bool IsUsable(int userTurn)
    {
        //유저턴이 2턴일경우 2턴마다 사용가능 한스킬을 사용할수있음.
        return userTurn % Turns == 0;
    }
}

