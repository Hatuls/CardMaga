using CardMaga.Battle.UI;
using CardMaga.UI.Card;
using System.Collections.Generic;
using UnityEngine;

public class FirstCardDisplayer : MonoBehaviour
{
    private ClickHelper _clickHelper;
    private TutorialClickHelper _tutorialClickHelper;
    private BattleUiManager _battleUIManager;
    private IReadOnlyList<CardUI> _cards;

    public void GetCard()
    {
        ZoomCardUI.OnEnterZoomTutorial += ReturnToHand;
        _clickHelper = ClickHelper.Instance;
        _tutorialClickHelper = TutorialClickHelper.Instance;
        _battleUIManager = BattleUiManager.Instance;
        _cards = _battleUIManager.CardUIManager.HandUI.GetCardUIFromHand();
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
        _battleUIManager.CardUIManager.HandUI.ZoomCardUI.ReturnToHandState(_cards[0]);
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
