using System;
using UnityEngine;
namespace CardMaga.Rewards
{
    [Serializable]
    public class PackReward : IRewardable
    {
        [SerializeField]
        private string _packName;

        [SerializeField]
        private int[] _cardsID;

        public string Name => _packName;

        public bool TryRecieveReward()
        {
            return true;
        }
        public PackReward(string name, int[] cardsID)
        {
            _packName = name;
            _cardsID = cardsID;
        }
#if UNITY_EDITOR
        public void Init(string name, int[] cardsID)
        {
   
        }
#endif
    }





    public interface IRewardable//<T>
    {
        string Name { get; }
        bool TryRecieveReward();//T reciever);
    }
 

    public enum RewardType
    {
        Currency =0,
        Character = 1,
        Pack = 2,
        Gift = 3,
        Bundle = 4,
        Arena = 5,
        Arena_Skin = 6,
        Character_Skin= 7,
        Character_Color = 8,
        Account_Icons = 9,
    }
    public enum CurrencyType
    {
        None = 0,
        Coins = 1,
        Diamonds = 2,
        Chips = 3,
        Account_EXP = 4,
        Free = 5
    }
}