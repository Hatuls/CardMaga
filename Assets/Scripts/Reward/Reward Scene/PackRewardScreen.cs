using CardMaga.CinematicSystem;
using Factory;
using ReiTools.TokenMachine;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.Rewards
{

    public class PackRewardScreen : BaseRewardsVisualHandler
    {
        [SerializeField]
        private CinematicManager _cinematicManager;
        private Queue<int> _cardsIDS = new Queue<int>();

        private ITokenReciever _tokenReciever;
        public IEnumerable<PackReward> PackRewards
        {
            get
            {
                for (int i = 0; i < _rewards.Count; i++)
                    yield return _rewards[i] as PackReward;
            }
        }

        public override void Init()
        {
            base.Init();
            _tokenReciever = new TokenMachine(MoveNext);
        }
        public override void Show()
        {
            base.Show();
            MoveNext();
        }

        private void MoveNext()
        {
            if (_cardsIDS.Count == 0)
            {
                Hide();
                return;
            }
            // Set Pack Card to id
            int coreID = _cardsIDS.Dequeue();
            Account.GeneralData.CardCore cardCore = GameFactory.Instance.CardFactoryHandler.CreateCardCore(coreID);

            //Insert CardCore To Pack

            //Cinematics
            _cinematicManager.ResetAll();
            _cinematicManager.StartCinematicSequence(_tokenReciever);

        }

        protected override void CalculateRewards()
        {
            foreach (var card in PackRewards)
            {
                foreach (var id in card.CardsID)
                    _cardsIDS.Enqueue(id);
            }
        }
    }

}