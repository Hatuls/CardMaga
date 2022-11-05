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

#if UNITY_EDITOR
        public void Init(string name, int[] cardsID)
        {
            _packName = name;
            _cardsID = cardsID;
        }
#endif
    }





    public interface IRewardable//<T>
    {
        string Name { get; }
        bool TryRecieveReward();//T reciever);
    }
 


    public enum CurrencyType
    {
        None = 0,
        Coins = 1,
        Diamonds = 2,
        Chips = 3,
        Account_EXP = 4,
        Free = 0 
    }
}