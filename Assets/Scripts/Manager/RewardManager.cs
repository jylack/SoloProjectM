using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RewardManager : MonoBehaviour
{
    public static RewardManager instance;

    Dictionary<int ,Reward> rewards = new(); // ���� ����Ʈ
                                             // ���� ID�� Ű�� ����Ͽ� ������ �����ϴ� Dictionary�� ����մϴ�.

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