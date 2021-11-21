using Battles;
using UnityEngine;
namespace Rewards
{
    [CreateAssetMenu(fileName = "Battle Reward Collection", menuName = "ScriptableObjects/Collections/Battle Rewards")]
    public class BattleRewardCollectionSO : ScriptableObject
    {
        [SerializeField]
        BattleRewardSO[] _reward;
        public void Init(BattleRewardSO[] rewardSOs)
            => _reward = rewardSOs;
        public BattleReward GetReward(CharacterTypeEnum characterTypeEnum, ActsEnum act)
        {
            for (int i = 0; i < _reward.Length; i++)
            {
                if (_reward[i].CharacterDifficultyEnum == characterTypeEnum)
                    return _reward[i].CreateReward(act);
            }
            throw new System.Exception($"Could not make reward because it was not found in the Battle reward collection\nthe character was {characterTypeEnum.ToString()}\nthe collection has in it {_reward.Length} rewards");
        }
    }

}
