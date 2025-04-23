using UnityEditor;
using UnityEngine;
public enum StageType
{
    Normal,
    Elite,
    Boss,
    Fairy,
    Demon,
    Angel,
    Shop,
    Reward
}


[CreateAssetMenu(fileName = "StageInfo", menuName = "Game/Stage Info")]
public class StageInfoSO : ScriptableObject
{
    public int stageIndex;
    public StageType type;
    [TextArea] public string description;

    public GameObject npcPrefab;
    public GameObject backgroundPrefab;
    public AudioClip sfxOnEnter;

    public EnemySet enemySet;
    public RewardCondition rewardCondition;
    public bool isCleared;
}
