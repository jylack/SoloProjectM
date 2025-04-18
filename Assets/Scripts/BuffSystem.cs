using System.Collections.Generic;
using UnityEngine;


public class BuffSystem
{
    private List<Buff> buffs = new List<Buff>();

    public void AddBuff(Buff newBuff)
    {
        buffs.Add(newBuff);
    }

    public void ApplyTurnEffects(UnitStats target, LogUI logUI)
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            var buff = buffs[i];

            if (!buff.HasApplied)
            {
                target.ModifyAttack(buff.AttackModifier);
                target.ModifyDefense(buff.DefenseModifier);
                buff.HasApplied = true;
                logUI.AddLog($"{target.Name}���� ���� ����: {buff.Name} (���� {buff.AttackModifier}, ��� {buff.DefenseModifier})");
            }

            buff.Duration--;
            if (buff.Duration <= 0)
            {
                target.ModifyAttack(-buff.AttackModifier);
                target.ModifyDefense(-buff.DefenseModifier);
                logUI.AddLog($"{target.Name}�� {buff.Name} ���� ����");
                buffs.RemoveAt(i);
            }
        }
    }

    public void Clear()
    {
        buffs.Clear();
    }
}

public class Buff
{
    public string Name;
    public int Duration;
    public int AttackModifier;
    public int DefenseModifier;
    public bool HasApplied;

    public Buff(string name, int duration, int atkMod, int defMod)
    {
        Name = name;
        Duration = duration;
        AttackModifier = atkMod;
        DefenseModifier = defMod;
        HasApplied = false;
    }
}
