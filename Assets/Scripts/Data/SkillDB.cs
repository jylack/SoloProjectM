using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class SkillDB
{
    public static List<Skill> AllSkills = new List<Skill>
    {
        CreateSkill("�Ϲ� ���� ����", "�Ϲ� ���� �� ���� Ȯ���� ���� 2�� �ߵ� ����.", SkillType.Passive, SkillGrade.Normal),
        CreateSkill("�Ϲ� ���� ����+", "�Ϲ� ���� �� ���� Ȯ���� ���� 2�� �ߵ� ����. (Ȯ�� ���)", SkillType.Passive, SkillGrade.Normal),
        CreateSkill("��Ÿ �����͸�", "��Ÿ Ȯ�� ���, ��Ÿ �� ���ݷ� ��, ��ø �����ϸ� ���� ����� ������ ����.", SkillType.Passive, SkillGrade.Legendary),
        CreateSkill("ġ��Ÿ �����͸�", "ġ��Ÿ Ȯ�� ���, ���� ��, ��ø ����.", SkillType.Passive, SkillGrade.Legendary),
        CreateSkill("�г� �����͸�", "�г� ��ų ����� +100%, 1ȸ ������ ������ �г� ��ų -25%.", SkillType.Passive, SkillGrade.Legendary),
        CreateSkill("������ �����͸�", "������ ����� +60%. ����� ��.", SkillType.Passive, SkillGrade.Legendary),
        CreateSkill("����", "���ݷ� �����, HP �ִ�ġ ���.", SkillType.Passive, SkillGrade.Legendary),
        CreateSkill("��� ���ǿ�", "�ϸ��� ����� ���� ��.", SkillType.Passive, SkillGrade.Mythic),
        CreateSkill("��� ���ǿ�", "�ϸ��� ���ݷ� ���.", SkillType.Passive, SkillGrade.Mythic),
        CreateSkill("���� ���ݷ�", "���ݷ� ������.", SkillType.Passive, SkillGrade.Mythic),
        CreateSkill("���� �����", "HP ������, HP �ִ�ġ���� ȸ��.", SkillType.Passive, SkillGrade.Mythic),
        CreateSkill("õ���� ��[C]", "���� ���� ��, HP �����������. õ���� ��ȣ�� ȹ���ϸ� 6�� ����.", SkillType.Passive, SkillGrade.Immortal),
        CreateSkill("�Ǹ��� ��[C]", "HP ó������ 50% �̸��� ��� ���ݷ� �����������. �Ǹ��� ��ȣ�� ȹ���ϸ� ���� ������� ����.", SkillType.Passive, SkillGrade.Immortal)
    };

    private static Skill CreateSkill(string name, string desc, SkillType type, SkillGrade grade)
    {
        int power = CalculatePowerFromArrows(desc);
        return new Skill(name, desc, power, type, grade);
    }

    private static int CalculatePowerFromArrows(string text)
    {
        int total = 0;
        var matches = Regex.Matches(text, "[���]+");

        foreach (Match match in matches)
        {
            int factor = match.Value.Contains("��") ? 10 : -10;
            total += factor * match.Value.Length;
        }

        return total;
    }

    public static List<Skill> GetRandomSkills(int count)
    {
        var shuffled = new List<Skill>(AllSkills);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[rnd]) = (shuffled[rnd], shuffled[i]);
        }
        return shuffled.GetRange(0, count);
    }
}
