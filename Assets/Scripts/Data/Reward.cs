
public enum RewardType
{
    Skill = 0, // 스킬 보상
    Gold = 1, // 골드 보상
    Exp = 2, // 경험치 보상
    Stat = 3, // 스텟 보상 (공격력, 방어력 등)
    Item = 4,
}

public class Reward
{
    public int id; // 보상 ID
    public string name; // 보상 이름
    public string description; // 보상 설명
    public int amount; // 보상 수량
    public RewardType type; // 보상 타입 (0: 스킬, 1: 골드, 2: 경험치, 3: 스텟(공,방))
    public int value; // 보상 값 (아이템 ID, 골드 양, 경험치 양 등)
    public bool isClaimed; // 보상 수령 여부
    public Reward(int id, string name, string description, int amount, RewardType type, int value)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.amount = amount;
        this.type = type;
        this.value = value;
        this.isClaimed = false;
    }
}

