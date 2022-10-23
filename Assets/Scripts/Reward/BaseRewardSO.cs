using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CardMaga.BaseRewardSO
{
    [CreateAssetMenu(fileName = "New BasePoolObject Reward SO", menuName = "ScriptableObjects/Rewards/BasePoolObject Reward SO")]
    abstract public class BaseRewardSO : ScriptableObject
    {
        public abstract RewardPack GenerateReward();
    }
}
