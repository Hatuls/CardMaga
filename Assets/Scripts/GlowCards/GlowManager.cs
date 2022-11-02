using CardMaga.Battle.UI;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using CardMaga.UI.Card;
using Characters.Stats;
using ReiTools.TokenMachine;
using System.Collections.Generic;
using UnityEngine;

public class GlowManager : MonoBehaviour, ISequenceOperation<IBattleUIManager>
{

    private HandUI _handUI;
    private IGetCardsUI _cardSlots;
    private StaminaHandler _playerStaminaHandler;
    public int Priority => 0;

    private void Awake()
    {
        _cardSlots = GetComponent<IGetCardsUI>();
       // HandUI.OnCardsAddToHand += CheckCardToGlow;
       // HandUI.OnCardsExecuteGetCards += CheckCardToGlow;
       // HandUI.OnCardSelect += ActiveDeckCardsGlow;
       // HandUI.OnCardSetToHandState += DeactiveDeckCards;
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
    private void UpdateCardsGlow()
    {
        var cards = _cardSlots.CardsUI;
        for (int i = 0; i < cards.Count; i++)
        {
            CardUI cardUI = cards[i];

            if (_playerStaminaHandler.CanPlayCard(cardUI.CardData))
                cardUI.CardVisuals.ActivateGlow();
            else
                cardUI.CardVisuals.DeactivateGlow();
        }
    }
   

    public void ActiveDeckCardsGlow(CardUI selectedCard)
    {
        for (int i = 0; i < _cardSlots.CardsUI.Count; i++)
        {
            if (selectedCard != _cardSlots.CardsUI[i])
                _cardSlots.CardsUI[i].CardVisuals.DeactivateGlow();
        }
    }

    public void DeactiveDeckCards(CardUI selectedCard)
    {
        if (_playerStaminaHandler.CanPlayCard(selectedCard.CardData))
            selectedCard.CardVisuals.ActivateGlow();

        for (int i = 0; i < _cardSlots.CardsUI.Count; i++)
        {
            if (_playerStaminaHandler.CanPlayCard(_cardSlots.CardsUI[i].CardData))
                _cardSlots.CardsUI[i].CardVisuals.ActivateGlow();
        }

    }

    private void OnDestroy()
    {
        // HandUI.OnCardsAddToHand -= CheckCardToGlow;
        // HandUI.OnCardsExecuteGetCards -= CheckCardToGlow;
        // HandUI.OnCardSelect -= ActiveDeckCardsGlow;
        _handUI.OnCardSelect -= DeactiveDeckCards;
        _handUI.OnCardSelect -= ActiveDeckCardsGlow;
        _handUI.OnHandUICardsUpdated -= UpdateCardsGlow;
    }

    public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager data)
    {
        _playerStaminaHandler = data.BattleDataManager.PlayersManager.GetCharacter(true).StaminaHandler;
        _handUI = data.HandUI;
        _handUI.OnHandUICardsUpdated += UpdateCardsGlow;
        _handUI.OnCardSelect += ActiveDeckCardsGlow;
        _handUI.OnCardSelect += DeactiveDeckCards;
    }
}
