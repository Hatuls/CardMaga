using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardMaga.UI;
using Battle;
using Cards;
using CardMaga.UI.Card;
using CardMaga.Input;

public class FirstCardDisplayer : MonoBehaviour
{
    private ClickHelper _clickHelper;
    private TutorialClickHelper _tutorialClickHelper;
    private BattleManager _battleManager;
    private IReadOnlyList<CardUI> _cards;

    public void GetCard()
    {
        ZoomCardUI.OnEnterZoomTutorial += ReturnToHand;
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
        _clickHelper.ZoomInClicker.Lock();
        _clickHelper.LoadObject(true, false, null, _cards[0].RectTransform);
    }

    public void ReturnToHand()
    {
        //_clickHelper.Close();
        //_battleManager.CardUIManager.HandUI.ZoomCardUI.ReturnToHandState(_cards[0]);
        _tutorialClickHelper.gameObject.SetActive(true);
        _battleManager.CardUIManager.HandUI.SetToHandState(_cards[0]);
        _tutorialClickHelper.Close();
        
    }

    public void BlockCardHold()
    {
        _cards[0].Inputs.DisableHold = true;
    }

    public void UnBlockCardHold()
    {
        _cards[0].Inputs.DisableHold = false;
    }
}
