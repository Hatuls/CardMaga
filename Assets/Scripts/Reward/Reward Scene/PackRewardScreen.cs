using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.Rewards
{

    public class PackRewardScreen : BaseRewardsVisualHandler
    {

        private int[] _cardsIDS;


        public IEnumerable<PackReward> PackRewards
        {
            get
            {
                for (int i = 0; i < _rewards.Count; i++)
                    yield return _rewards[i] as PackReward;
            }
        }
        public override void Show()
        {
            base.Show();

            for (int i = 0; i < _cardsIDS.Length; i++)
            {
                Debug.Log("ID: " + _cardsIDS[i]);
            }
        }
        protected override void CalculateRewards()
        {
            List<int> ids = new List<int>();

            foreach (var card in PackRewards)
            {
                foreach (var id in card.CardsID)
                    ids.Add(id);
                
            }

            _cardsIDS = ids.ToArray();
        }
    }

}