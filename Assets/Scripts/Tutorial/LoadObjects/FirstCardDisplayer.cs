using CardMaga.Battle.UI;
using CardMaga.UI.Card;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CardMaga.UI;
using Battle;
using Cards;
using CardMaga.UI.Card;
using CardMaga.Input;

public class FirstCardDisplayer : MonoBehaviour
{
    [SerializeField] private UnityEvent OnReturnCard;

    private TutorialClickHelper _tutorialClickHelper;
    private BattleUiManager _battleUIManager;
    private IReadOnlyList<CardUI> _cards;

    public IReadOnlyList<CardUI> FirstCard { get => _cards;}

    public void GetCard()
    {
        _tutorialClickHelper = TutorialClickHelper.Instance;
        _battleUIManager = BattleUiManager.Instance;
        _cards = _battleUIManager.CardUIManager.HandUI.GetCardUIFromHand();
        InputBehaviour<CardUI> tutorialZoomOutInputBehaviour = new InputBehaviour<CardUI>();
        tutorialZoomOutInputBehaviour.OnClick += ZoomInCardInput;
        _cards[0].Inputs.TrySetInputBehaviour(tutorialZoomOutInputBehaviour);
        Debug.Log(_cards[0].Inputs.CurrentInputBehaviourState);
    }    
        

    public void LoadCardOnPanel()
    {
        _tutorialClickHelper.LoadObject(true, false, null, _cards[0].RectTransform);
        
    }

    public void ZoomInCardInput(CardUI cardUI)
    {
        _battleManager.CardUIManager.HandUI.ZoomCardUI.EnterState(_cards[0]);
        InputBehaviour<CardUI> tutorialZoomInInputBehaviour = new InputBehaviour<CardUI>();
        //tutorialZoomInInputBehaviour.OnClick += ReturnToHand;
        _cards[0].Inputs.TrySetInputBehaviour(tutorialZoomInInputBehaviour);
    }

    public void ReturnToHand(CardUI cardUI)
    {
        _cards[0].Inputs.ForceResetInputBehaviour();
        _battleManager.CardUIManager.HandUI.ZoomCardUI.ForceExitState();
        _battleManager.CardUIManager.HandUI.SetToHandState(cardUI);
        _tutorialClickHelper.LoadObject(true, false, null, _cards[0].RectTransform);
        OnReturnCard.Invoke();
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

    public void LockCard()
    {
        _cards[0].Inputs.Lock();
    }

    public void UnlockCard()
    {
        _cards[0].Inputs.Lock();
    }
}
