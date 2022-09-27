using System;
using Battle;
using Battle.Deck;
using CardMaga.UI;
using CardMaga.UI.Card;
using DG.Tweening;
using CardMaga.Sequence;
using ReiTools.TokenMachine;
using UnityEngine;

public class FollowCardUI : BaseHandUIState, ISequenceOperation<IBattleManager>
{
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

    private void Start()
    {
        _executionBoundry_Y = _executionBoundry.position.y;
        BattleManager.Register(this, OrderType.Default);
        InputReciever.OnTouchDetectedLocation += GetMousePos;
        
        _inputBehaviour.OnHold += FollowHand;
        _inputBehaviour.OnPointUp += ReleaseCardUI;
    }

    public void OnDestroy()
    {
        _inputBehaviour.OnHold -= FollowHand;
        _inputBehaviour.OnPointUp -= ReleaseCardUI;
        InputReciever.OnTouchDetectedLocation -= GetMousePos;
        KillTween();
    }

    public override void ExitState(CardUI cardUI)
    {
        if (!ReferenceEquals(cardUI, SelectedCardUI))
            return;

        base.ExitState(cardUI);
    }
    
    private void ReleaseCardUI(CardUI cardUI)
    {
        if (cardUI.transform.position.y > _executionBoundry_Y)
        {
            ExitState(cardUI);
            _handUI.TryExecuteCard(cardUI);
            return;
        }
        
        _handUI.SetToHandState(cardUI);
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

    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    {
    }
}