// MonsterEncounter.cs
using UnityEngine;

[System.Serializable]
public class MonsterEncounter
{
    [Tooltip("인카운터 대상 몬스터 Prefab")]
    public GameObject monsterPrefab;

    [Tooltip("가중치. 높을수록 자주 등장합니다.")]
    public int weight = 1;
}
