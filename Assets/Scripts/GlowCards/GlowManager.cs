using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using CardMaga.UI;
using CardMaga.Card;
using Characters.Stats;
using CardMaga.UI.Card;
using Managers;
using Battle;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;

public class GlowManager : MonoBehaviour ,ISequenceOperation<IBattleManager>
{
    IGetCardsUI cardUI;
    private StaminaHandler _playerStaminaHandler;
    public int Priority => 0;

    private void Awake()
    {
        BattleManager.Register(this, OrderType.After);
        cardUI = GetComponent<IGetCardsUI>();
        HandUI.OnCardsAddToHand += CheckCardToGlow;
        HandUI.OnCardsExecuteGetCards += CheckCardGlowAfterExecute;
        HandUI.OnCardSelect += ActiveDeckCardsGlow;
        HandUI.OnCardSetToHandState += DeactiveDeckCards;
    }

    public void CheckCardToGlow(IReadOnlyList<CardUI> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (_playerStaminaHandler.CanPlayCard(cards[i].CardData))
            {
                cards[i].CardVisuals.ActivateGlow();
            }

            else
                cards[i].CardVisuals.DeactivateGlow();
        }
    }

    public void CheckCardGlowAfterExecute(IReadOnlyList<CardUI> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (_playerStaminaHandler.CanPlayCard(cards[i].CardData))
                cards[i].CardVisuals.ActivateGlow();

            else
                cards[i].CardVisuals.DeactivateGlow();
        }
    }

    public void ActiveDeckCardsGlow(CardUI selectedCard)
    {
        for (int i = 0; i < cardUI.CardsUI.Count; i++)
        {
            if(selectedCard!=cardUI.CardsUI[i])
                cardUI.CardsUI[i].CardVisuals.DeactivateGlow();
        }
    }

    public void DeactiveDeckCards(CardUI selectedCard)
    {
        if (_playerStaminaHandler.CanPlayCard(selectedCard.CardData))
            selectedCard.CardVisuals.ActivateGlow();
            
        for (int i = 0; i < cardUI.CardsUI.Count; i++)
        {
            if (_playerStaminaHandler.CanPlayCard(cardUI.CardsUI[i].CardData))
                cardUI.CardsUI[i].CardVisuals.ActivateGlow();
        }

    }

    private void OnDestroy()
    {
        HandUI.OnCardsAddToHand -= CheckCardToGlow;
        HandUI.OnCardsExecuteGetCards -= CheckCardGlowAfterExecute;
        HandUI.OnCardSelect -= ActiveDeckCardsGlow;
        HandUI.OnCardSelect -= DeactiveDeckCards;
    }

    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    {
        _playerStaminaHandler = data.PlayersManager.GetCharacter(true).StaminaHandler;
    }
}
