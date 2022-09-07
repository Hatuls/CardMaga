using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CardMaga.BaseRewardSO
{
    [CreateAssetMenu(fileName = "New Base Reward SO", menuName = "ScriptableObjects/Rewards/Base Reward SO")]
    abstract public class BaseRewardSO : ScriptableObject
    {
        public abstract RewardPack GenerateReward();
    }
}
