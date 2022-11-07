
using System;
using UnityEngine;
namespace CardMaga.Rewards
{
    [Serializable]
    public class CharacterReward : IRewardable
    {
     
        [SerializeField] private string _name;
        [SerializeField] private int _characterID;
      
        public string Name => _name;

        public bool TryRecieveReward()
        {
            throw new NotImplementedException();
        }

#if UNITY_EDITOR
        public void Init(string name, int characterID)
        {

            _name = name;
            _characterID = characterID;
        }
#endif
    }
}