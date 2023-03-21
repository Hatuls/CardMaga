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

    public void CheckCardToGlow(IReadOnlyList<BattleCardUI> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (_playerStaminaHandler.CanPlayCard(cards[i].BattleCardData))
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
            BattleCardUI battleCardUI = cards[i];

            if (_playerStaminaHandler.CanPlayCard(battleCardUI.BattleCardData))
                battleCardUI.CardVisuals.ActivateGlow();
            else
                battleCardUI.CardVisuals.DeactivateGlow();
        }
    }
   

    public void ActiveDeckCardsGlow(BattleCardUI selectedBattleCard)
    {
        for (int i = 0; i < _cardSlots.CardsUI.Count; i++)
        {
            if (selectedBattleCard != _cardSlots.CardsUI[i])
                _cardSlots.CardsUI[i].CardVisuals.DeactivateGlow();
        }
    }

    public void DeactiveDeckCards(BattleCardUI selectedBattleCard)
    {
        if (_playerStaminaHandler.CanPlayCard(selectedBattleCard.BattleCardData))
            selectedBattleCard.CardVisuals.ActivateGlow();

        for (int i = 0; i < _cardSlots.CardsUI.Count; i++)
        {
            if (_playerStaminaHandler.CanPlayCard(_cardSlots.CardsUI[i].BattleCardData))
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

    public void ExecuteTask(ITokenReceiver tokenMachine, IBattleUIManager data)
    {
        _playerStaminaHandler = data.BattleDataManager.PlayersManager.GetCharacter(true).StaminaHandler;
        _handUI = data.HandUI;
        _handUI.OnHandUICardsUpdated += UpdateCardsGlow;
        _handUI.OnCardSelect += ActiveDeckCardsGlow;
        _handUI.OnCardSelect += DeactiveDeckCards;
    }
}
