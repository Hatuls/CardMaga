using CardMaga.Rewards.Bundles;
using System;
using UnityEngine;
namespace CardMaga.Rewards
{
    [Serializable]
    public class CurrencyReward : IRewardable
    {

        [SerializeField] 
        private string _name;
        [SerializeField]
        ResourcesCost _resourcesCost;
        public string Name => _name;

        public ResourcesCost ResourcesCost => _resourcesCost; 

        public bool TryRecieveReward()
        {
            throw new NotImplementedException();
        }

#if UNITY_EDITOR
        public void Init( string name, ResourcesCost resourcesCost)
        {
            _name = name;
            _resourcesCost= resourcesCost;
        }
#endif
    }
}