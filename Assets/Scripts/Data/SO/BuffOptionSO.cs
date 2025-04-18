using UnityEngine;

[CreateAssetMenu(menuName = "GameData/BuffOption")]
public class BuffOptionSO : ScriptableObject
{
    public string displayName;
    public string description;
    public Buff buff;
}
