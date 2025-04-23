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

        // ��Ʈ ��� ȸ�� ��ų
        AllSkills.Add(new Skill("��Ʈ ȸ��", "��Ʈ�� 3ȸ �����Ǹ� HP�� 3% ȸ���մϴ�.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));
        AllSkills.Add(new Skill("��Ʈ ȸ��+", "��Ʈ�� 2ȸ �����Ǹ� HP�� 5% ȸ���մϴ�.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));

        // ���Ǻ� HP ȸ�� (HP 30% ����)
        AllSkills.Add(new Skill("��� ȸ��", "HP�� ó������ 30% �̸��� �Ǹ� 3�� ���� �� �ϸ��� 10% ȸ���˴ϴ�.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));
        AllSkills.Add(new Skill("��� ȸ��+", "HP�� ó������ 30% �̸��� �Ǹ� 3�� ���� �� �ϸ��� 15% ȸ���˴ϴ�.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));

        // HP ȸ�� ����
        AllSkills.Add(new Skill("HP ȸ�� ����", "HP ȸ������ +2% �����մϴ�.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));
        AllSkills.Add(new Skill("HP ȸ�� ����+", "HP ȸ������ +3.5% �����մϴ�.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/heart")));

        // ����� ����
        AllSkills.Add(new Skill("���� ����", "�޴� ���ذ� 25% �����մϴ�.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/shield")));
        AllSkills.Add(new Skill("���� ����+", "�޴� ���ذ� 40% �����մϴ�.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/shield")));

        // �Ϲ� ���� �� ��ȣ�� ȹ��
        AllSkills.Add(new Skill("�Ϲ� ��ȣ��", "�Ϲ� ���� �� Ȯ���� ��ȣ�� ȹ��. (��ȣ��: HP�� 5%)", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/shield")));

        // �г� ��ȣ��
        AllSkills.Add(new Skill("�г� ��ȣ��", "�г� ��ų ��� �� ��ȣ�� ȹ��. (��ȣ��: HP�� 10%)", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/shield")));

        // ������ ��ô
        AllSkills.Add(new Skill("������", "2�ϸ��� ������ 1���� �߻�. �������� �⺻ ������ 45% ������� �ݴϴ�.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/dagger")));
        AllSkills.Add(new Skill("������+", "2�ϸ��� ������ 2�� �߻�. �������� �⺻ ������ 45% ������� �ݴϴ�.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/dagger")));

        // ������ + ȸ��
        AllSkills.Add(new Skill("ȸ�� ������", "2�ϸ��� ������ 1�� �߻�. �������� Ȯ�������� 3% HP ȸ�� ����.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/dagger")));
        AllSkills.Add(new Skill("ȸ�� ������+", "2�ϸ��� ������ 1�� �߻�. �������� Ȯ�������� 3% HP ȸ�� ����. Ȯ�� ����.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/dagger")));

        // ������ + ����
        AllSkills.Add(new Skill("���� ������", "2�ϸ��� ������ 1�� �߻�. ��� �������� ���� Ȯ���� ���� �ߵ� ����. ���� �����: �⺻ ������ 30%", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/bolt")));
        AllSkills.Add(new Skill("���� ������+", "2�ϸ��� ������ 1�� �߻�. ���� Ȯ�� ����. ���� �����: �⺻ ������ 30%", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/bolt")));

        // ������ + ����
        AllSkills.Add(new Skill("���� ������", "2�ϸ��� ������ 1�� �߻�. ���� Ȯ���� ������ �ߵ� ȿ��. �ߵ� �����: �⺻ ������ 20%", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/poison")));
        AllSkills.Add(new Skill("���� ������+", "2�ϸ��� ������ 1�� �߻�. Ȯ�� ����. �ߵ� �����: �⺻ ������ 20%", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/poison")));

        // ġ��Ÿ ���� ��ų
        AllSkills.Add(new Skill("ġ��Ÿ Ȯ�� ���", "�⺻ ������ ġ��Ÿ Ȯ����.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/crit")));
        AllSkills.Add(new Skill("ġ��Ÿ Ȯ�� ���+", "�⺻ ������ ġ��Ÿ Ȯ���� ���.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/crit")));

        // ��Ÿ ���� ��ų
        AllSkills.Add(new Skill("��Ÿ Ȯ�� ���", "�⺻ ������ ��Ÿ�� �ߵ��� Ȯ����.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/multi")));
        AllSkills.Add(new Skill("��Ÿ Ȯ�� ���+", "�⺻ ������ ��Ÿ�� �ߵ��� Ȯ�����.", 0, SkillType.Passive, SkillGrade.Normal, Resources.Load<Sprite>("SkillIcons/multi")));

        //Ư�� ��ų
        AllSkills.Add(new Skill("����", "���ݷ� �����, HP �ִ�ġ ���.", 25, SkillType.Passive, SkillGrade.Legendary, masteryIcon));
        AllSkills.Add(new Skill("��� ���ǿ�", "�ϸ��� ����� ���� ��.", 30, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("��� ���ǿ�", "�ϸ��� ���ݷ� ���.", 30, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("���� ���ݷ�", "���ݷ� ������.", 35, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("���� �����", "HP ������, HP �ִ�ġ���� ȸ��.", 35, SkillType.Passive, SkillGrade.Mythic, mythIcon));
        AllSkills.Add(new Skill("õ���� ��", "���� ���� ��, HP �����������. õ���� ��ȣ�� ȹ���ϸ� 6�� ����.", 40, SkillType.Passive, SkillGrade.Immortal, angelIcon));
        AllSkills.Add(new Skill("�Ǹ��� ��", "HP ó������ 50% �̸��� ��� ���ݷ� �����������. �Ǹ��� ��ȣ�� ȹ���ϸ� ���� ������� ����.", 40, SkillType.Passive, SkillGrade.Immortal, demonIcon));


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
