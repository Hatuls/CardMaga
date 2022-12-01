using CardMaga.Battle;
using CardMaga.UI;
using CardMaga.UI.Card;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierTutorialHandler : MonoBehaviour
{
    public BattleCardUI BarrierCard { get; private set; }
    private IDisposable _token;
    private bool _isFlag;

    private void Awake()
    {
        HandUIState.OnCardDrawnAndAlign += OnlyUnlockBarrier;
    }

    public void WaitForBarrierToDraw(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        HandUI.OnCardsAddToHand += CheckForBerrierCard;
    }

    private void CheckForBerrierCard(IReadOnlyList<BattleCardUI> battleCardUIs)
    {
        for (int i = 0; i < battleCardUIs.Count; i++)
        {
            if (battleCardUIs[i].BattleCardData.CardSO.ID == 100)
            {
                HandUI.OnCardsAddToHand -= CheckForBerrierCard;
                ReleaseToken();
            }
        }
    }

    public void Barrier(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        _isFlag = true;
    }

    public void OnlyUnlockBarrier()
    {
        if (!_isFlag)
            return;
        IGetCardsUI cardsUI = BattleManager.Instance.BattleUIManager.HandUI.HandUIState;
        HandUIState.OnCardDrawnAndAlign -= OnlyUnlockBarrier;
        for (int i = 0; i < cardsUI.CardsUI.Count; i++)
        {
            if (cardsUI.CardsUI[i].BattleCardData.CardSO.ID != 100)
                cardsUI.CardsUI[i].Inputs.Lock();

            else
                BarrierCard = cardsUI.CardsUI[i];
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
