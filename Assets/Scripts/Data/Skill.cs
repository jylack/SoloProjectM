
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Encounters/Skill Encounter Data")]
[Serializable]
public class Skill : ScriptableObject
{
    public Sprite icon; // 스킬 아이콘
    public Sprite Icon { get => icon;} 

    public string skillName;
    public string SkillName { get => skillName; }
    
    public string description;
    public string Description { get => description; }

    public int damage; // 스킬 데미지    
    public int Damage { get => damage;  } 

    public int turns; // 스킬 사용가능 턴
    public int Turns { get => turns;  } 

    public int comboCount; // 스킬 콤보 수
    public int ComboCount { get => comboCount; }

    //public Skill(Sprite icon , string name, string description, int damage, int turns, int comboCount)
    //{
    //    Icon = icon;
    //    Name = name;
    //    Description = description;
    //    Damage = damage;
    //    Turns = turns;
    //    ComboCount = comboCount;
    //}

    public bool IsUsable(int userTurn)
    {
        //유저턴이 2턴일경우 2턴마다 사용가능 한스킬을 사용할수있음.
        return userTurn % Turns == 0;
    }
}

