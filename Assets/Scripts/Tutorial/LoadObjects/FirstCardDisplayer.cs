using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardMaga.UI;
using Battle;
using Cards;
using CardMaga.UI.Card;

public class FirstCardDisplayer : MonoBehaviour
{
    private ClickHelper _clickHelper;
    private TutorialClickHelper _tutorialClickHelper;
    private BattleManager _battleManager;
    private IReadOnlyList<CardUI> _cards;

    public void GetCard()
    {
        _clickHelper = ClickHelper.Instance;
        _tutorialClickHelper = TutorialClickHelper.Instance;
        _battleManager = BattleManager.Instance;
        _cards = _battleManager.CardUIManager.HandUI.GetCardUIFromHand();
    }    

    public void LoadCardOnPanel()
    {
        _tutorialClickHelper.LoadObject(true, false, null, _cards[0].RectTransform);
    }

    public void ZoomInCard()
    {
        _clickHelper.LoadObject(true, false, null, _cards[0].RectTransform);
    }

    public void ReturnToHand()
    {
        _battleManager.CardUIManager.HandUI.ZoomCardUI.ReturnToHandState(_cards[0]);
    }

    public void ReturnCardInput()
    {
        _cards[0].Inputs.UnLock();
    }
}
