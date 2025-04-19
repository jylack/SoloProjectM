using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRewardTrigger : MonoBehaviour
{
    [SerializeField] private int rewardCount = 3;

    private void Start()
    {
        StartCoroutine(GiveSkillReward());
    }

    private IEnumerator GiveSkillReward()
    {
        yield return new WaitForSeconds(1f);

        var rewardList = SkillDB.GetRandomSkills(rewardCount);

        yield return new WaitUntil(() => SkillRewardUI.Instance != null);
        
        SkillRewardUI.Instance.Open(rewardList);
    }
}
