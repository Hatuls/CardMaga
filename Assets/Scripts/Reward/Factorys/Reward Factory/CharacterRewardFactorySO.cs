using UnityEngine;


namespace CardMaga.Rewards
{
    [CreateAssetMenu(fileName = "New Special Pack Reward Factory", menuName = "ScriptableObjects/Rewards/Packs/New Special Pack Reward")]
    public class CharacterRewardFactorySO : BaseRewardFactorySO
    {
        [SerializeField]
        private CharacterReward _characterReward;
        public override IRewardable GenerateReward()
       => _characterReward;

     
#if UNITY_EDITOR
        public void Init(int characterID)
        {
            _characterReward = new CharacterReward();
            _characterReward.Init(Name, characterID);
        }
#endif
    }
}
