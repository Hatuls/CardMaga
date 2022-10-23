using Battle;
using Battle.Combo;
using CardMaga.Card;
using System.Collections.Generic;
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
        public BattleReward GetReward(CharacterTypeEnum characterTypeEnum, ActsEnum act, IEnumerable<ComboData> workOnCombo)
        {
            return BattleReward(characterTypeEnum).CreateReward(act,workOnCombo);
        }
        public CardData[] GetRewardCards(ActsEnum act, byte amount)
        {
            return BattleReward(CharacterTypeEnum.Elite_Enemy).GenerateCardsRewards(act, amount);
        }
        public RunReward GetRunReward(CharacterTypeEnum characterType, ActsEnum act)
            => BattleReward(characterType).CreateRunReward(act);
        public ComboData[] GetRewardCombos(ActsEnum act, byte amount, IEnumerable<ComboData> workOnCombo)
        {
            return BattleReward(CharacterTypeEnum.Elite_Enemy).GenerateComboReward(act,workOnCombo, amount);
        }

        private BattleRewardSO BattleReward(CharacterTypeEnum characterTypeEnum)
        {
            for (int i = 0; i < _reward.Length; i++)
            {
                if (_reward[i].CharacterDifficultyEnum == characterTypeEnum)
                    return _reward[i];
            }
            throw new System.Exception($"Could not make reward because it was not found in the Battle reward collection\nthe character was {characterTypeEnum.ToString()}\nthe collection has in it {_reward.Length} rewards");

        }
    }

}
