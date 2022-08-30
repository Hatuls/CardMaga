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
    
    [SerializeField] private HandUI _handUI;
    [SerializeField] private TransitionPackSO _followHand;
    [SerializeField] private RectTransform _executionBoundry;
    public int Priority => 0;
    private Sequence _currentSequence;
    private float _executionBoundry_Y;

    private Vector2 _mousePosition;
    private DeckHandler _playerDeck;


    private void Awake()
    {
        _executionBoundry_Y = _executionBoundry.position.y;
        BattleManager.Register(this, OrderType.Default);
        InputReciever.OnTouchDetected += GetMousePos;
    }

    public void OnDestroy()
    {
        InputReciever.OnTouchDetected -= GetMousePos;
        KillTween();
    }

    public override void ExitState(CardUI cardUI)
    {
        if (!ReferenceEquals(cardUI, SelectedCardUI))
            return;
        
        if (cardUI.transform.position.y > _executionBoundry_Y)
        {
            _playerDeck.TransferCard(DeckEnum.Hand, DeckEnum.Selected, SelectedCardUI.CardData);
            
            if (CardExecutionManager.Instance.TryExecuteCard(SelectedCardUI))
            {
                SelectedCardUI.Inputs.ForceChangeState(false);
                _handUI.RemoveCardUIsFromTable(SelectedCardUI);
                OnCardExecute?.Invoke(SelectedCardUI);
                return;
            }
        }
        
        base.ExitState(cardUI);
    }
    
    public void FollowHand(CardUI cardUI)
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

    public void ExecuteTask(ITokenReciever tokenMachine, BattleManager data)
    {
        _playerDeck = data.PlayersManager.GetCharacter(true).DeckHandler;
    }
}