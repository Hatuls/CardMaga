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
    private IDisposable _token;
    public BattleCardUI BarrierCard { get; private set; }

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
                BarrierCard = battleCardUIs[i];
                ReleaseToken();
            }
        }
    }

    public void OnlyUnlockBarrier(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        IGetCardsUI cardsUI = BattleManager.Instance.BattleUIManager.HandUI.HandUIState;
        for (int i = 0; i < cardsUI.CardsUI.Count; i++)
        {
            if (cardsUI.CardsUI[i].BattleCardData.CardSO.ID != 100)
            {
                cardsUI.CardsUI[i].Inputs.Lock();
                ReleaseToken();
            }
        }
    }

    private void ReleaseToken()
    {
        if (_token != null)
            _token.Dispose();

        else
            Debug.LogError("No token to release");
    }
}
