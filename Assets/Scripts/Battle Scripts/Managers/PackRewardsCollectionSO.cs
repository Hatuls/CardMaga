using Cards;
using UnityEngine;
namespace Rewards
{
    [CreateAssetMenu(fileName = "Pack Reward Collection", menuName = "ScriptableObjects/Collection/Pack Rewards Collection")]
    public class PackRewardsCollectionSO : ScriptableObject
    {

        [SerializeField]
        private PackRewardSO[] _packs;


        public void Init(PackRewardSO[] packRewardSOs)
        {
            _packs = packRewardSOs;
        }
        public PackRewardSO[] PacksRewardSO => _packs;
   
    }

}
