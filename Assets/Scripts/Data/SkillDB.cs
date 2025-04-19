using System.Collections.Generic;
using UnityEngine;

public static class SkillDB
{
    public static List<Skill> AllSkills = new List<Skill>();

    public static void InitDB()
    {
        AllSkills.Clear();

        Sprite boltIcon = Resources.Load<Sprite>("SkillIcons/bolt");
        Sprite masteryIcon = Resources.Load<Sprite>("SkillIcons/mastery");
        Sprite mythIcon = Resources.Load<Sprite>("SkillIcons/myth");
        Sprite angelIcon = Resources.Load<Sprite>("SkillIcons/angel");
        Sprite demonIcon = Resources.Load<Sprite>("SkillIcons/demon");

        // ���� ��ų�� ������ ���� �������� ����
        AllSkills.Add(new Skill("�Ϲ� ���� ����", "�Ϲ� ���� �� ���� Ȯ���� ���� 2�� �ߵ� ����.", 15, SkillType.Passive, SkillGrade.Normal, boltIcon));
        AllSkills.Add(new Skill("�Ϲ� ���� ����+", "�Ϲ� ���� �� ���� Ȯ���� ���� 2�� �ߵ� ����. (Ȯ�� ���)", 20, SkillType.Passive, SkillGrade.Normal, boltIcon));
        AllSkills.Add(new Skill("��Ÿ �����͸�", "��Ÿ Ȯ�� ���, ��Ÿ �� ���ݷ� ��, ��ø �����ϸ� ���� ����� ������ ����.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("ġ��Ÿ �����͸�", "ġ��Ÿ Ȯ�� ���, ���� ��, ��ø ����.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("�г� �����͸�", "�г� ��ų ����� +100%, 1ȸ ������ ������ �г� ��ų -25%.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("������ �����͸�", "������ ����� +60%. ����� ��.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("����", "���ݷ� �����, HP �ִ�ġ ���.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("��� ���ǿ�", "�ϸ��� ����� ���� ��.", 30, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("��� ���ǿ�", "�ϸ��� ���ݷ� ���.", 30, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("���� ���ݷ�", "���ݷ� ������.", 35, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("���� �����", "HP ������, HP �ִ�ġ���� ȸ��.", 35, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("õ���� ��[C]", "���� ���� ��, HP �����������. õ���� ��ȣ�� ȹ���ϸ� 6�� ����.", 40, SkillType.Passive, SkillGrade.Immortal, angelIcon));
        AllSkills.Add(new Skill("�Ǹ��� ��[C]", "HP ó������ 50% �̸��� ��� ���ݷ� �����������. �Ǹ��� ��ȣ�� ȹ���ϸ� ���� ������� ����.", 40, SkillType.Passive, SkillGrade.Immortal, demonIcon));
    }

    public static List<Skill> GetRandomSkills(int count)
    {
        if (AllSkills.Count == 0) InitDB();

        List<Skill> temp = new List<Skill>(AllSkills);
        List<Skill> result = new List<Skill>();

        for (int i = 0; i < count && temp.Count > 0; i++)
        {
            int rand = Random.Range(0, temp.Count);
            result.Add(temp[rand].Clone());
            temp.RemoveAt(rand);
        }

        return result;
    }
}
