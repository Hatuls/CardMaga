using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardMaga.UI;
using Battle;
using Cards;
using CardMaga.UI.Card;

public class ZoomOutCardMask : MonoBehaviour
{
    private ClickHelper _clickHelper;
    private TutorialClickHelper _tutorialClickHelper;
    private HandUI handUI;
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

    public void ReturnToOriginal()
    {
        _tutorialClickHelper.ReturnObjects();
    }

    public void StopCardInput()
    {
        _cards[0].Inputs.Lock();
    }

    public void ReturnCardInput()
    {
        _cards[0].Inputs.UnLock();
    }
}
