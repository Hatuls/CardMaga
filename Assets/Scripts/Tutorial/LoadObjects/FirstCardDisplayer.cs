using CardMaga.Battle.UI;
using CardMaga.UI.Card;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CardMaga.Input;

public class FirstCardDisplayer : MonoBehaviour
{
    [SerializeField] private UnityEvent OnReturnCard;
    [SerializeField] private DialoguesFlow _dialoguesFlow3;
    [SerializeField] private DialoguesFlow _dialoguesFlow4;
    [SerializeField] private DialoguesFlow _dialoguesFlow5;
    private TutorialClickHelper _tutorialClickHelper;
    private BattleUiManager _battleUIManager;
    private IReadOnlyList<CardUI> _cards;

    public IReadOnlyList<CardUI> FirstCard { get => _cards;}

    public void GetCard()
    {
        _tutorialClickHelper = TutorialClickHelper.Instance;
        _battleUIManager = BattleUiManager.Instance;
        _cards = _battleUIManager.HandUI.GetCardUIFromHand();
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
        _battleUIManager.CardUIManager.HandUI.ZoomCardUI.MoveToZoomPosition(_cards[0]);
        InputBehaviour<CardUI> tutorialZoomInInputBehaviour = new InputBehaviour<CardUI>();
        
        tutorialZoomInInputBehaviour.OnClick += MoveNextDialogues;
        _cards[0].Inputs.TrySetInputBehaviour(tutorialZoomInInputBehaviour);
    }

    private void MoveNextDialogues(CardUI cardUI)
    {
        if (_dialoguesFlow3.gameObject.activeSelf)
            _dialoguesFlow3.MoveNextDialogues();

        else if (_dialoguesFlow4.gameObject.activeSelf)
            _dialoguesFlow4.MoveNextDialogues();

        else if (_dialoguesFlow5.gameObject.activeSelf)
            _dialoguesFlow5.MoveNextDialogues();

        else
        {
            InputBehaviour<CardUI> returnCardInputBehaviour = new InputBehaviour<CardUI>();
            returnCardInputBehaviour.OnClick += ReturnCardToHand;
            _cards[0].Inputs.TrySetInputBehaviour(returnCardInputBehaviour);
        }
    }

    [Sirenix.OdinInspector.Button]
    public void ReturnCard()
    {
        ReturnCardToHand(_cards[0]);
    }

    private void ReturnCardToHand(CardUI cardUI)
    {
        _cards[0].Inputs.ForceResetInputBehaviour();
        _battleUIManager.CardUIManager.HandUI.ZoomCardUI.ForceExitState();
        _battleUIManager.CardUIManager.HandUI.SetToHandState(cardUI);
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

    private void OnDestroy()
    {
        
    }

}
