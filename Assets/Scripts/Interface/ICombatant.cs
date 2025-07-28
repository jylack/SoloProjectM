public interface ICombatant
{
    string Name { get; }
    int Speed { get; }
    int Attack { get; }
    bool IsDead { get; }
    int AttackCount { get; }    // 이번 턴에 사용할 수 있는 최대 행동 횟수
    int CurrentActions { get; set; }
    void ResetActions();
    void TakeDamage(int amount);
}
