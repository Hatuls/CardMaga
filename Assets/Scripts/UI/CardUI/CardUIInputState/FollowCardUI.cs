﻿using CardMaga.UI;
using CardMaga.UI.Card;
using DG.Tweening;
using UnityEngine;

public class FollowCardUI : BaseHandUIState
{
    [Header("Scripts Reference")]
    [SerializeField] private HandUI _handUI;
    [Header("TransitionPackSO")]
    [SerializeField] private TransitionPackSO _followHand;
    [Header("RectTransforms")]
    [SerializeField] private RectTransform _executionBoundry;
    public int Priority => 0;
    private float _executionBoundry_Y;

    private Vector2 _mousePosition;

    private void Start()
    {
        _executionBoundry_Y = _executionBoundry.position.y;
        InputReciever.Instance.OnTouchDetectedLocation += GetMousePos;

        _inputBehaviour.OnHold += FollowHand;
        _inputBehaviour.OnPointUp += ReleaseCardUI;
    }

    public void OnDestroy()
    {
        _inputBehaviour.OnHold -= FollowHand;
        _inputBehaviour.OnPointUp -= ReleaseCardUI;
        InputReciever.Instance.OnTouchDetectedLocation -= GetMousePos;
    }
    public override void ExitState(BattleCardUI battleCardUI)
    {
        if (!ReferenceEquals(battleCardUI, SelectedBattleCardUI))
            return;

        base.ExitState(battleCardUI);
    }

    private void ReleaseCardUI(BattleCardUI battleCardUI)
    {
        if (battleCardUI.transform.position.y > _executionBoundry_Y)
        {
            ExitState(battleCardUI);
            _handUI.TryExecuteCard(battleCardUI);
            return;
        }

        _handUI.SetToHandState(battleCardUI);
    }

    private void FollowHand(BattleCardUI battleCardUI)
    {
        battleCardUI.KillTween(false);
        battleCardUI.CurrentSequence = battleCardUI.RectTransform.Move(_mousePosition, _followHand);
    }

    private void GetMousePos(Vector2 mousePos)
    {
        _mousePosition = mousePos;
    }




}