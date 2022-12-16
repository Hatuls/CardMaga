using CardMaga.UI.Card;
using CardMaga.UI.Combos;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.UI.Collections.Zoom
{

    public class BattleCollectionZoomHandler : MonoBehaviour
    {
        private readonly TransitionPackSO _zoomInTransition;
        private readonly TransitionPackSO _zoomOutTransition;


        private readonly ComboAndDeckCollectionHandler _comboAndDeckCollectionHandler;
        private readonly ComboAndDeckCollectionBattleHandler _comboAndDeckCollectionBattleHandler;
        private readonly RectTransform _cardZoomPosition;
        //  private RectTransform _comboZoomPosition;


        public BattleCollectionZoomHandler(ComboAndDeckCollectionBattleHandler comboAndDeckCollectionBattleHandler, RectTransform cardZoomPosition) //,RectTransform comboZoomPosition=null)
        {
            _cardZoomPosition = cardZoomPosition;
            _comboAndDeckCollectionBattleHandler = comboAndDeckCollectionBattleHandler;
            _comboAndDeckCollectionHandler = comboAndDeckCollectionBattleHandler.ComboAndDeckCollectionHandler;

            _comboAndDeckCollectionHandler.OnBattleCardUIShown += CardCollectionShown;
            _comboAndDeckCollectionHandler.OnBattleComboUIShown += ComboCollectionShown;

            _comboAndDeckCollectionBattleHandler.CardInputBehaviour.OnClick += ZoomInCardUI;
            //  _comboZoomPosition = comboZoomPosition;
        }
        ~BattleCollectionZoomHandler()
        {
            _comboAndDeckCollectionBattleHandler.CardInputBehaviour.OnClick -= ZoomInCardUI;
            _comboAndDeckCollectionHandler.OnBattleCardUIShown -= CardCollectionShown;
            _comboAndDeckCollectionHandler.OnBattleComboUIShown -= ComboCollectionShown;
        }
        private void CardCollectionShown(IReadOnlyList<BattleCardUI> battleCardUIs)
        {
            BattleCardUI card = null;
            for (int i = 0; i < battleCardUIs.Count; i++)
            {
                card = battleCardUIs[0];
                card.Inputs.TrySetInputBehaviour(_comboAndDeckCollectionBattleHandler.CardInputBehaviour);
                ForceResetOnObject(card);
            }

        }
        private void ComboCollectionShown(IReadOnlyList<BattleComboUI> battleComboUIs)
        {
            BattleComboUI card = null;
            for (int i = 0; i < battleComboUIs.Count; i++)
            {
                card = battleComboUIs[0].BattleCardUI;
                card.Inputs.TrySetInputBehaviour(_comboAndDeckCollectionBattleHandler.CardInputBehaviour);
                ForceResetOnObject(card);
            }

        }

        private void ForceResetOnObject(IZoomableObject obj)
        => obj.ZoomHandler.ForceReset();



        public void ZoomInCardUI(BattleComboUI battleCardUI)
        {
            battleCardUI.RectTransform.Transition(_cardZoomPosition, _zoomInTransition)
               .AppendCallback(ZoomIn);

            void ZoomIn() => battleCardUI.ZoomIn();
        }
        public void ZoomInCardUI(BattleCardUI battleCardUI)
        {
            battleCardUI.CurrentSequence = battleCardUI.RectTransform.Transition(_cardZoomPosition, _zoomInTransition)
                .AppendCallback(ZoomIn);

            void ZoomIn() => battleCardUI.ZoomIn();
        }

        public void Zoom(IZoomableObject zoomableObject)
            => Zoom(zoomableObject.ZoomHandler);
        public void Zoom(IZoomable zoomable)
        {
            zoomable.ZoomIn(false);
        }
    }

}