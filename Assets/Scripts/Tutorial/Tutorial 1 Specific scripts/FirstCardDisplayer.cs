using CardMaga.Battle.UI;
using CardMaga.UI.Card;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CardMaga.Input;
using System;

public class FirstCardDisplayer : MonoBehaviour
{
    [SerializeField] private DialoguesFlow _dialoguesFlow3;
    [SerializeField] private DialoguesFlow _dialoguesFlow4;
    [SerializeField] private DialoguesFlow _dialoguesFlow5;
    private TutorialClickHelper _tutorialClickHelper;
    private BattleUiManager _battleUIManager;
    private IReadOnlyList<BattleCardUI> _cards;

    public event Action OnSecondsTimeLoad;
    public IReadOnlyList<BattleCardUI> FirstCard { get => _cards;}

    public void GetCard()
    {
        _tutorialClickHelper = TutorialClickHelper.Instance;
        _battleUIManager = BattleUiManager.Instance;
        _cards = _battleUIManager.HandUI.GetCardUIFromHand();
        InputBehaviour<BattleCardUI> tutorialZoomOutInputBehaviour = new InputBehaviour<BattleCardUI>();
        tutorialZoomOutInputBehaviour.OnClick += ZoomInCardInput;
        _cards[0].Inputs.TrySetInputBehaviour(tutorialZoomOutInputBehaviour);
        Debug.Log(_cards[0].Inputs.CurrentInputBehaviourState);
    }    
        

    public void LoadCardOnPanel()
    {
        _tutorialClickHelper.LoadObject(true, false, null, _cards[0].RectTransform);
    }

    public void ZoomInCardInput(BattleCardUI cardUI)
    {
        _battleUIManager.CardUIManager.HandUI.ZoomCardUI.MoveToZoomPosition(_cards[0]);
        InputBehaviour<BattleCardUI> tutorialZoomInInputBehaviour = new InputBehaviour<BattleCardUI>();
        
        tutorialZoomInInputBehaviour.OnClick += MoveNextDialogues;
        _cards[0].Inputs.TrySetInputBehaviour(tutorialZoomInInputBehaviour);
    }

    private void MoveNextDialogues(BattleCardUI cardUI)
    {
        if (_dialoguesFlow3.gameObject.activeSelf)
            _dialoguesFlow3.MoveNextDialogues();

        else if (_dialoguesFlow4.gameObject.activeSelf)
            _dialoguesFlow4.MoveNextDialogues();

        else if (_dialoguesFlow5.gameObject.activeSelf)
            _dialoguesFlow5.MoveNextDialogues();

        else
        {
            InputBehaviour<BattleCardUI> returnCardInputBehaviour = new InputBehaviour<BattleCardUI>();
            returnCardInputBehaviour.OnClick += ReturnCardToHand;
            _cards[0].Inputs.TrySetInputBehaviour(returnCardInputBehaviour);
        }
    }

    public void ReturnCard()
    {
        ReturnCardToHand(_cards[0]);
    }

    private void ReturnCardToHand(BattleCardUI cardUI)
    {
        _cards[0].Inputs.ForceResetInputBehaviour();
        _battleUIManager.CardUIManager.HandUI.ZoomCardUI.ForceExitState();
        _battleUIManager.CardUIManager.HandUI.SetToHandState(cardUI);
    }

    public void PutInputBehaviourAfterZoomIn()
    {
        InputBehaviour<BattleCardUI> afterZoomOut = new InputBehaviour<BattleCardUI>();
        _cards[0].Inputs.TrySetInputBehaviour(afterZoomOut);
        afterZoomOut.OnBeginHold += _battleUIManager.HandUI.SetToFollowState;
    }

    public void BlockCardHold()
    {
        _cards[0].Inputs.DisableHold = true;
    }

    public void UnBlockCardHold()
    {
        _cards[0].Inputs.DisableHold = false;
    }

    public void LockCardInput()
    {
        _cards[0].Inputs.Lock();
    }

    public void UnlockCardInput()
    {
        _cards[0].Inputs.Lock();
    }

    private void OnDestroy()
    {
        
    }

}
