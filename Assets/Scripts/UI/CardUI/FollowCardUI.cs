using System;
using Battle;
using Battle.Deck;
using CardMaga.UI;
using CardMaga.UI.Card;
using DG.Tweening;
using Managers;
using ReiTools.TokenMachine;
using UnityEngine;

public class FollowCardUI : BaseHandUIState, ISequenceOperation<BattleManager>
{
    public static event Action<CardUI> OnCardExecute;
    [Header("Scripts Reference")]
    [SerializeField] private HandUI _handUI;
    [Header("TransitionPackSO")]
    [SerializeField] private TransitionPackSO _followHand;
    [Header("RectTransforms")]
    [SerializeField] private RectTransform _executionBoundry;
    public int Priority => 0;
    private Sequence _currentSequence;
    private float _executionBoundry_Y;

    private Vector2 _mousePosition;
    private DeckHandler _playerDeck;
    

    private void Start()
    {
        _executionBoundry_Y = _executionBoundry.position.y;
        BattleManager.Register(this, OrderType.Default);
        InputReciever.OnTouchDetectedLocation += GetMousePos;
        
        _inputBehaviour.OnHold += FollowHand;
        _inputBehaviour.OnPointUp += ReturnToHandState;
    }

    public void OnDestroy()
    {
        _inputBehaviour.OnHold -= FollowHand;
        _inputBehaviour.OnPointUp -= ReturnToHandState;
        InputReciever.OnTouchDetectedLocation -= GetMousePos;
        KillTween();
    }

    public override void ExitState(CardUI cardUI)
    {
        if (!ReferenceEquals(cardUI, SelectedCardUI))
            return;
        
        if (cardUI.transform.position.y > _executionBoundry_Y)
        {
           ExecuteCardUI(cardUI);
        }
        else
        {
            _handUI.ReturnCardToHand(cardUI);
        }
        
        base.ExitState(cardUI);
    }
    
    private void ReturnToHandState(CardUI cardUI)
    {
        ExitState(cardUI);
    }
    
    private void FollowHand(CardUI cardUI)
    {
        KillTween();
        _currentSequence = cardUI.RectTransform.Move(_mousePosition, _followHand);
    }

    private void GetMousePos(Vector2 mousePos)
    {
        _mousePosition = mousePos;
    }

    private void KillTween()
    {
        if (_currentSequence != null) _currentSequence.Kill();
    }

    private void ExecuteCardUI(CardUI cardUI)
    {
        _playerDeck.TransferCard(DeckEnum.Hand, DeckEnum.Selected, cardUI.CardData);
            
        if (CardExecutionManager.Instance.TryExecuteCard(cardUI))
        {
            cardUI.Inputs.ForceChangeState(false);
            OnCardExecute?.Invoke(cardUI);
        }
    }
    
    public void ExecuteTask(ITokenReciever tokenMachine, BattleManager data)
    {
        _playerDeck = data.PlayersManager.GetCharacter(true).DeckHandler;
    }
}