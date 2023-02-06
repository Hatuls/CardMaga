using CardMaga.CinematicSystem;
using CardMaga.MetaUI;
using Factory;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace CardMaga.Rewards
{

    public class PackRewardScreen : BaseRewardsVisualHandler
    {
        [SerializeField]
        private MetaCardUI _metaCardUI;
        [SerializeField]
        private CinematicManager _cinematicManager;
        [ReadOnly,ShowInInspector]
        private Queue<int> _cardsIDS = new Queue<int>();

        [SerializeField]
        private ClickHelper _clickHelper;
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
            var  cardInstance = GameFactory.Instance.CardFactoryHandler.CreateCardInstance(coreID);

            //Insert CardCore To Pack
            _metaCardUI.AssignVisual(cardInstance);
            //Cinematics
            _cinematicManager.ResetAll();
            _cinematicManager.StartCinematicSequence();

        }

        protected override void CalculateRewards()
        {
            foreach (var card in PackRewards)
            {
                foreach (var id in card.CardsID)
                    _cardsIDS.Enqueue(id);
            }
        }

        public  void WaitForInput()
        {
            _clickHelper.Open(MoveNext);
        }
    }

}