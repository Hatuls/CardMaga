using System;
using Battle;
using CardMaga.UI;
using CardMaga.UI.Card;
using DG.Tweening;
using UnityEngine;

public class FollowCardUI : MonoBehaviour
{
    public event Action<CardUI> OnCardExecute;
    
    [SerializeField] private HandUI _handUI;
    [SerializeField] private ZoomCardUI _zoomCardUI;
    [SerializeField] private TransitionPackSO _followHand;
    [SerializeField] private RectTransform _executionBoundry;

    private Sequence _currentSequence;
    private float _executionBoundry_Y;

    private Vector2 _mousePosition;

    private CardUI _selectCardUI;
    private IDisposable _token;

    public void Start()
    {
        _executionBoundry_Y = _executionBoundry.position.y;
        InputReciever.OnTouchDetected += GetMousePos;
    }

    public void OnDestroy()
    {
        InputReciever.OnTouchDetected -= GetMousePos;
        KillTween();
    }


    public void SetSelectCardUI(CardUI cardUI)
    {
        if (_selectCardUI != null)
            return;

        _selectCardUI = cardUI;
        Debug.Log("Card Set To follow" + _selectCardUI);
        _selectCardUI.Inputs.OnClick -= _zoomCardUI.SetZoomCard;
        _selectCardUI.Inputs.OnHold += FollowHand;
        _selectCardUI.Inputs.OnPointUp += ReleaseCard;
    }

    private void ReleaseCard(CardUI cardUI)
    {
        if (!ReferenceEquals(cardUI, _selectCardUI))
            return;

        _selectCardUI.Inputs.OnHold -= FollowHand;
        _selectCardUI.Inputs.OnPointUp -= ReleaseCard;

        if (cardUI.transform.position.y > _executionBoundry_Y &&
            CardExecutionManager.Instance.TryExecuteCard(_selectCardUI))
        {
            _selectCardUI.Inputs.ForceChangeState(false);
            OnCardExecute?.Invoke(_selectCardUI);
        }
        else
            _handUI.ReturnCardUIToHand(_selectCardUI);

        _selectCardUI = null;
    }

    public void ForceReleaseCard()
    {
        if (_selectCardUI == null)
            return;

        _selectCardUI.Inputs.OnHold -= FollowHand;
        _selectCardUI.Inputs.OnPointUp -= ReleaseCard;
        _handUI.ForceReturnCardUIToHand(_selectCardUI);
        _selectCardUI = null;
    }


    private void FollowHand(CardUI cardUI)
    {
        KillTween();
        _currentSequence = cardUI.CardTransitionManager.Move(_mousePosition, _followHand);
    }

    private void GetMousePos(Vector2 mousePos)
    {
        _mousePosition = mousePos;
    }

    private void KillTween()
    {
        if (_currentSequence != null) _currentSequence.Kill();
    }
}