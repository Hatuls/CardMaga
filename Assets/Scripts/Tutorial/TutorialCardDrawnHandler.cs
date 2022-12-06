using CardMaga.Battle;
using CardMaga.UI;
using CardMaga.UI.Card;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TutorialCardDrawn
{
    public class TutorialCardDrawnHandler : MonoBehaviour
    {
        [SerializeField] private int _cardId;
        public BattleCardUI DrawnCard { get; private set; }
        private IDisposable _token;

        public void WaitForCardToBeDrawn(ITokenReciever tokenReciever)
        {
            _token = tokenReciever.GetToken();
            HandUI.OnCardsAddToHand += CheckIfCardWasDrawn;
        }

        private void CheckIfCardWasDrawn(IReadOnlyList<BattleCardUI> battleCardUIs)
        {
            for (int i = 0; i < battleCardUIs.Count; i++)
            {
                if (battleCardUIs[i].BattleCardData.CardSO.ID == _cardId)
                {
                    HandUI.OnCardsAddToHand -= CheckIfCardWasDrawn;
                    ReleaseToken();
                }
            }
        }

        public void WaitForCardToAlign(ITokenReciever tokenReciever)
        {
            _token = tokenReciever.GetToken();
            BattleManager.Instance.BattleUIManager.HandUI.HandUIState.OnAllCardsDrawnAndAlign += UnlockOnlyDrawnCard;
        }

        public void UnlockOnlyDrawnCard()
        {
            IGetCardsUI cardsUI = BattleManager.Instance.BattleUIManager.HandUI.HandUIState;
            BattleManager.Instance.BattleUIManager.HandUI.HandUIState.OnAllCardsDrawnAndAlign -= UnlockOnlyDrawnCard;
            for (int i = 0; i < cardsUI.CardsUI.Count; i++)
            {
                if (cardsUI.CardsUI[i].BattleCardData.CardSO.ID != _cardId)
                    cardsUI.CardsUI[i].Inputs.Lock();

                else
                    DrawnCard = cardsUI.CardsUI[i];
            }
            ReleaseToken();
        }

        public void UnlockCards(ITokenReciever tokenReciever)
        {
            _token = tokenReciever.GetToken();
            IGetCardsUI cardsUI = BattleManager.Instance.BattleUIManager.HandUI.HandUIState;
            for (int i = 0; i < cardsUI.CardsUI.Count; i++)
            {
                cardsUI.CardsUI[i].Inputs.UnLock();
            }
            ReleaseToken();
        }

        private void ReleaseToken()
        {
            if (_token != null)
                _token.Dispose();

            else
                Debug.LogError("No token to release");
        }
    }
}

