using Battles;
using Cards;
using UnityEngine;
namespace Rewards
{
    [CreateAssetMenu(fileName = "Reward", menuName = "ScriptableObjects/Rewards/Battle Reward")]
    public class BattleRewardSO :ScriptableObject 
    {
        [SerializeField]
        CharacterTypeEnum _rarityEnum;
        public CharacterTypeEnum RarityEnum => _rarityEnum;


        [SerializeField]
        ushort _minMoneyReward;
        [SerializeField]
        ushort _maxMoneyReward;
    
        public ushort MoneyReward { get { return (ushort)Random.Range(_minMoneyReward, _maxMoneyReward); } }

        [SerializeField]
        private ushort[] _rewardCardSOID;
        public ushort[] RewardCardSOID { get => _rewardCardSOID; private set => _rewardCardSOID = value; }

        [SerializeField]
        private byte _minCardsLevel;
        [SerializeField]
        private byte _maxCardsLevel;


        public void Init(string[] row)
        {





        }


        public BattleReward CreateReward()
        {
            const byte cardsRewardAmount = 3;
            ushort[] cardSOID = new ushort[cardsRewardAmount] {0,0,0};
            int rewardID = Random.Range(0, _rewardCardSOID.Length - 1);

            for (int i = 0; i < cardsRewardAmount; i++)
            {  
                if (i != 0)
                {
                    for (int j = 0; j < cardSOID.Length; j++)
                    {
                        if (cardSOID[j] == 0)
                            break;

                        do
                        {
                            rewardID = Random.Range(0, _rewardCardSOID.Length - 1);
                        }
                        while (cardSOID[j] == rewardID);
                    }
                }

                cardSOID[i] = (_rewardCardSOID[rewardID]);
            }


            Card[] rewardCards = new Card[cardsRewardAmount];
            var factoryHandler = Factory.GameFactory.Instance.CardFactoryHandler;

            for (byte i = 0; i < cardsRewardAmount; i++)
                rewardCards[i] = factoryHandler.CreateCard(cardSOID[i], (byte)Random.Range(_minCardsLevel, _maxCardsLevel));
            

            BattleReward reward = new BattleReward(MoneyReward, rewardCards);

            return reward;
        }
    }


}

