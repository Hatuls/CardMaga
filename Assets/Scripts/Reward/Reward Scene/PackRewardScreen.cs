using Account.GeneralData;
using CardMaga.Battle.Players;
using CardMaga.CinematicSystem;
using CardMaga.MetaUI;
using CardMaga.UI.Visuals;
using Factory;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace CardMaga.Rewards
{
    [System.Serializable]
    public class PackInfo
    {
        public PackType PackType;
        public RewardTagSO RewardTagSO;
        public MetaCardUI MetaCardUI;
        public CinematicManager CinematicManager;
    }
    public class PackRewardScreen : BaseRewardsVisualHandler
    {

        public static event Action<IEnumerable<CoreID>> OnCardsGifted;

        [SerializeField]
        private PackInfo[] _packInfo;
   
        private CinematicManager _cinematicManager;
        [ReadOnly,ShowInInspector]
        private Queue<CardPack> _cardsIDS = new Queue<CardPack>();

        [SerializeField]
        private ObjectActivationManager _objectRenderer;

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
        private IEnumerable<CoreID> CardsReceived
        {
            get
            {
                foreach (var pack in PackRewards)
                {
                    IReadOnlyList<int> cardsID = pack.CardsID;
                    for (int i = 0; i < cardsID.Count; i++)
                        yield return new CoreID(cardsID[i]);
                }
            }
        }

        public override void Show()
        {
            base.Show();
            MoveNext();
        }

        protected override void AddRewards()
        {
            foreach (var item in PackRewards)
                item.AddToDevicesData();

            OnCardsGifted?.Invoke(CardsReceived);
        }
        private void MoveNext()
        {
            if (_cardsIDS.Count == 0)
            {
                Hide();
                return;
            }
            // Set Pack Card to id
            var core = _cardsIDS.Dequeue();
            var  cardInstance = GameFactory.Instance.CardFactoryHandler.CreateCardInstance(core.ID);

            //Insert CardCore To Pack
            TagSO packType = core.PackType;
            _objectRenderer.Activate(packType);
            var pack = GetCard(packType);


            //Cinematics
            _cinematicManager = pack.CinematicManager;
            _cinematicManager.ResetAll();
            _cinematicManager.StartCinematicSequence();
            pack.MetaCardUI.AssignVisual(cardInstance);



            PackInfo GetCard(TagSO pt)
            {
                for (int i = 0; i < _packInfo.Length; i++)
                {
                    if (_packInfo[i].RewardTagSO == pt)
                        return _packInfo[i];
                }
                throw new System.Exception($"Pack Tag was not found!\nType: " + pt.ToString());
            }
    
  
       
        }

        protected override void CalculateRewards()
        {
            foreach (var card in PackRewards)
            {
                foreach (var id in card.CardsID)
                {
                    _cardsIDS.Enqueue(new CardPack(Pack(card.PackType),id));
                }
            }

            TagSO Pack(PackType packType)
            {

                for (int i = 0; i < _packInfo.Length; i++)
                {
                    if (_packInfo[i].PackType == packType)
                        return _packInfo[i].RewardTagSO;
                }
                        throw new System.Exception("Pack Type was not found " + packType.ToString());
                
            }
        }

        public  void WaitForInput()
        {
            _clickHelper.Open(MoveNext);
        }
    }


    [System.Serializable]
    public class CardPack
    {
        private TagSO _packType;
        private int _id;
        public int ID => _id;

        public TagSO PackType { get => _packType; }

        public CardPack(TagSO packType, int id)
        {
            _packType = packType;
            _id = id;
        }
    }
}