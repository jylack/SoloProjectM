using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform monsterSpawnRoot;
    [SerializeField] private LogUI battleLogUI;
    [SerializeField] private StateUIManager stateUIManager;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float attackDelay = 0.5f;

    private UnitStats playerStats;
    private List<UnitStats> monsterStatsList = new List<UnitStats>();
    private List<BuffSystem> monsterBuffsList = new List<BuffSystem>();

    private Action<bool> onBattleEndCallback;

    private BuffSystem playerBuffs = new BuffSystem();
    private int turnIndex = 0;
    private int playerTurnCount = 0;

    private List<Skill> playerSkills;

    public void StartBattle(UnitStats player, List<UnitStats> monsters, Action<bool> onBattleEnd)
    {
        playerStats = player;
        monsterStatsList = monsters;
        onBattleEndCallback = onBattleEnd;

        playerBuffs.Clear();
        monsterBuffsList.Clear();

        foreach (var m in monsterStatsList)
            monsterBuffsList.Add(new BuffSystem());

        playerSkills = GameManager.instance.PlayerSkills;

        stateUIManager.RefreshUI();
        StartCoroutine(BattleRoutine());
    }

    private IEnumerator BattleRoutine()
    {
        yield return StartCoroutine(BattleIntro());
        yield return StartCoroutine(CombatLoop());
    }

    private IEnumerator BattleIntro()
    {
        Vector3 playerStart = playerTransform.position;
        Vector3 playerTarget = playerStart + Vector3.right;
        yield return MoveOverTime(playerTransform, playerStart, playerTarget, moveDuration);

        var count = monsterStatsList.Count;

        Vector3 monsterStart = monsterSpawnRoot.position;

        Vector3 monsterTarget;


        if (count > 1)
        {
            monsterTarget = monsterStart;
        }
        else
        {
            monsterTarget = monsterStart + Vector3.right;
        }

        yield return MoveOverTime(monsterSpawnRoot, monsterTarget, monsterTarget, moveDuration);
        yield return new WaitForSeconds(0.3f);
    }

    private IEnumerator CombatLoop()
    {
        while (!playerStats.IsDead && monsterStatsList.Count > 0)
        {
            if (turnIndex == 0)
            {
                playerTurnCount++;

                playerBuffs.ApplyTurnEffects(playerStats, battleLogUI);
                ApplySkillEffectsOnTurnStart();
                ApplySkillEffectsOnPlayerTurn();

                stateUIManager.RefreshUI();

                var target = monsterStatsList[0];
                int totalHits = playerStats.AttackCount;

                if (HasSkillChance("��Ÿ")) totalHits++;

                for (int i = 0; i < totalHits; i++)
                {
                    target.TakeDamage(playerStats.Attack);
                    battleLogUI.AddLog($"�÷��̾��� ����! {target.Name}���� {playerStats.Attack} ����!");

                    if (HasSkillChance("����"))
                    {
                        int healAmount = playerStats.Attack / 2;
                        playerStats.Heal(healAmount);
                        battleLogUI.AddLog($"[��ų] ���� �ߵ�! �÷��̾� HP {healAmount} ȸ��");
                    }

                    if (HasSkillChance("����"))
                    {
                        battleLogUI.AddLog($"[��ų] ���� ���� �ߵ�! {target.Name}���� ���� ȿ�� �����!");
                    }
                }

                stateUIManager.RefreshUI();

                if (target.IsDead)
                {
                    Debug.Log("���� ���");
                    battleLogUI.AddLog($"{target.Name} ���!");

                    monsterStatsList.RemoveAt(0);
                    monsterBuffsList.RemoveAt(0);
                }
            }
            else
            {
                int monsterIndex = turnIndex - 1;
                if (monsterIndex < monsterStatsList.Count)
                {
                    var attacker = monsterStatsList[monsterIndex];
                    var buff = monsterBuffsList[monsterIndex];

                    buff.ApplyTurnEffects(attacker, battleLogUI);
                    stateUIManager.RefreshUI();

                    playerStats.TakeDamage(attacker.Attack);
                    battleLogUI.AddLog($"{attacker.Name}�� ����! �÷��̾�� {attacker.Attack} ����!");

                    stateUIManager.RefreshUI();

                    if (playerStats.IsDead)
                    {
                        battleLogUI.AddLog("�÷��̾� ���!");
                        onBattleEndCallback?.Invoke(false);
                        yield break;
                    }
                }
            }

            turnIndex++;
            if (turnIndex > monsterStatsList.Count) turnIndex = 0;
            yield return new WaitForSeconds(attackDelay);
        }

        battleLogUI.AddLog("��� ���� óġ �Ϸ�!");
        onBattleEndCallback?.Invoke(true);
    }

    private void ApplySkillEffectsOnTurnStart()
    {
        foreach (var skill in playerSkills)
        {
            if (!CheckSkillCondition(skill)) continue;

            if (skill.Name.Contains("�ϸ��� ����") && UnityEngine.Random.value < skill.Power / 100f)
            {
                if (monsterStatsList.Count > 0)
                {
                    var target = monsterStatsList[UnityEngine.Random.Range(0, monsterStatsList.Count)];
                    target.TakeDamage(playerStats.Attack);
                    battleLogUI.AddLog($"[��ų] {skill.Name} �ߵ�! �� {target.Name}���� �߰� ���� ���� {playerStats.Attack}");
                }
            }

            if (skill.Name.Contains("��ȣ��") && UnityEngine.Random.value < skill.Power / 100f)
            {
                int shieldAmount = playerStats.Attack / 2;
                playerStats.Heal(shieldAmount);
                battleLogUI.AddLog($"[��ų] ��ȣ�� ȹ��! HP {shieldAmount} ��ȣ�� ȿ��");
            }
        }
    }

    private void ApplySkillEffectsOnPlayerTurn()
    {
        foreach (var skill in playerSkills)
        {
            if (!CheckSkillCondition(skill)) continue;

            if (skill.Name.Contains("2�ϸ���") && playerTurnCount % 2 == 0)
            {
                playerStats.IncrementAttackCount(1);
                battleLogUI.AddLog($"[��ų] {skill.Name} �ߵ� �� �̹� �� ���� Ƚ�� +1");
            }

            if (skill.Name.Contains("3�ϸ���") && playerTurnCount % 3 == 0)
            {
                playerStats.IncrementAttackCount(1);
                battleLogUI.AddLog($"[��ų] {skill.Name} �ߵ� �� �̹� �� ���� Ƚ�� +1");
            }
        }
    }

    private bool HasSkillChance(string keyword)
    {
        foreach (var skill in playerSkills)
        {
            if (skill.Name.Contains(keyword) && CheckSkillCondition(skill))
            {
                if (UnityEngine.Random.value < skill.Power / 100f)
                {
                    battleLogUI.AddLog($"[��ų] {skill.Name} ȿ�� �ߵ�!");
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckSkillCondition(Skill skill)
    {
        if (skill.Name.Contains("���") && playerStats.CurrentHp < playerStats.MaxHp * 0.3f)
            return true;

        if (skill.Name.Contains("HP 50%") && playerStats.CurrentHp < playerStats.MaxHp * 0.5f)
            return true;

        if (skill.Name.Contains("���� ����") && turnIndex == 0)
            return true;

        if (!skill.Name.Contains("���") && !skill.Name.Contains("���� ����") && !skill.Name.Contains("HP 50%"))
            return true;

        return false;
    }

    private IEnumerator MoveOverTime(Transform target, Vector3 from, Vector3 to, float duration)
    {

        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.position = to;

    }
}
