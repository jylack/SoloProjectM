using System;

public interface ICombatant
{
    string Name { get; }
    int Speed { get; }
    int Attack { get; }

    /// <summary>한 턴에 수행 가능한 최대 행동 횟수</summary>
    int MaxActionsPerTurn { get; }

    /// <summary>남은 행동 횟수</summary>
    int ActionsRemaining { get; set; }

    bool IsDead { get; }

    /// <summary>HP 변화 시 (현재HP, 최대HP)를 발행</summary>
    event Action<int, int> OnHpChanged;

    /// <summary>턴 시작 시 행동 횟수 초기화</summary>
    void ResetActions();

    /// <summary>데미지 적용</summary>
    void TakeDamage(int amount);
}
