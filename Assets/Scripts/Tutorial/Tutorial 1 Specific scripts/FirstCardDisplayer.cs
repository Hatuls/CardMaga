using CardMaga.Battle.UI;
using CardMaga.Input;
using CardMaga.UI.Card;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FirstCardDisplayer : MonoBehaviour
{
    [SerializeField] private DialoguesFlow _dialoguesFlow3;
    [SerializeField] private DialoguesFlow _dialoguesFlow4;
    [SerializeField] private DialoguesFlow _dialoguesFlow5;
    private BattleUiManager BattleUIManager => BattleUiManager.Instance;
    private IReadOnlyList<BattleCardUI> _cards;
    private InputBehaviour<BattleCardUI> _tutorialZoomInInputBehaviour = new InputBehaviour<BattleCardUI>();
    public event Action OnZoomingOutCard;
    public event Action OnLoadCardOnPanelSecondsTime;

    private TutorialClickHelper TutorialClickHelper  =>  TutorialClickHelper.Instance;

    public BattleCardUI GetFirstCard => BattleUIManager.HandUI.GetCardUIFromHand()[0];
    public void GetCard()
    {

        _cards = BattleUIManager.HandUI.GetCardUIFromHand();
        InputBehaviour<BattleCardUI> tutorialZoomOutInputBehaviour = new InputBehaviour<BattleCardUI>();
        _cards[0].Inputs.TrySetInputBehaviour(tutorialZoomOutInputBehaviour);
        tutorialZoomOutInputBehaviour.OnClick += ZoomInCardInput;
        Debug.Log(_cards[0].Inputs.CurrentInputBehaviourState);
    }


    public void LoadCardOnPanel()
    {
        TutorialClickHelper.LoadObject(true, false, null, GetFirstCard.RectTransform);
       BlockCardHold();
    }

    public void ZoomInCardInput(BattleCardUI cardUI)
    {
        BattleUIManager.CardUIManager.HandUI.ZoomCardUI.MoveToZoomPosition(_cards[0]);

        GetFirstCard.Inputs.TrySetInputBehaviour(_tutorialZoomInInputBehaviour);
        _tutorialZoomInInputBehaviour.OnClick += MoveNextDialogues;
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
            GetFirstCard.Inputs.TrySetInputBehaviour(returnCardInputBehaviour);
        }

    }

    private void ReturnCardToHand(BattleCardUI cardUI)
    {
        if (OnZoomingOutCard != null)
            OnZoomingOutCard.Invoke();
        GetFirstCard.Inputs.ForceResetInputBehaviour();
        BattleUIManager.CardUIManager.HandUI.ZoomCardUI.ForceExitState();
        BattleUIManager.CardUIManager.HandUI.SetToHandState(cardUI);
        GetFirstCard.Inputs.DisableClick = true;
    }

    public void PutInputBehaviourAfterZoomIn()
    {
        InputBehaviour<BattleCardUI> afterZoomOut = new InputBehaviour<BattleCardUI>();
        GetFirstCard.Inputs.TrySetInputBehaviour(afterZoomOut);
        afterZoomOut.OnBeginHold += BattleUIManager.HandUI.SetToFollowState;
    }

    public void BlockCardHold()
    {
        GetFirstCard.Inputs.DisableHold = true;
    }

    public void UnBlockCardHold()
    {
        GetFirstCard.Inputs.DisableHold = false;
    }

    public void LockCardInput()
    {
        GetFirstCard.Inputs.Lock();
    }

    public void UnlockCardInput()
    {
        GetFirstCard.Inputs.Lock();
    }

    private void OnDestroy()
    {
       // _tutorialZoomInInputBehaviour.OnBeginHold -= 
    }

}
