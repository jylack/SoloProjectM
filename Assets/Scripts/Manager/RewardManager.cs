using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RewardManager : MonoBehaviour
{
    public static RewardManager instance;

    Dictionary<int ,Reward> rewards = new(); // 보상 리스트
                                             // 보상 ID를 키로 사용하여 보상을 저장하는 Dictionary를 사용합니다.

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddReward(Reward reward)
    {
        rewards.Add(reward.id,reward);
    }

    public Reward GetReward(int id)
    {
        return rewards[id];
    }

}