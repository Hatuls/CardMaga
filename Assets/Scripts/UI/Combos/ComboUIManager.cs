using CardMaga.Battle;
using CardMaga.Battle.UI;
using CardMaga.Card;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using CardMaga.UI.Card;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ComboUIManager : MonoBehaviour, ISequenceOperation<IBattleUIManager>
{
    public event Action<CardUI[]> OnCardComboDone;

    [Header("Scripts Reference")]
    [SerializeField] private CardUIManager _cardUIManager;
    [SerializeField] private HandUI _handUI;

    [Header("RectTransforms")]
    [SerializeField] private RectTransform _drawPosition;
    [SerializeField] private RectTransform _destination;

    [FormerlySerializedAs("_drawTransitionPackSo")]
    [Header("TransitionPackSOs")]
    [SerializeField] private TransitionPackSO _drawMoveTransitionPackSo;
    [SerializeField] private TransitionPackSO _drawScaleTransitionPackSo;

    [Header("Draw Parameters")]
    [SerializeField] private float _delaybetweenDrawCards;

    private WaitForSeconds _waitForDrawBetweenCards;

    public int Priority => 0;


    private void CraftComboCards(params CardData[] cardDatas)
    {
        CardUI[] cardUis = _cardUIManager.GetCardsUI(cardDatas);

        SetCardUisAtPosition(_drawPosition, cardUis);
    }

    private void SetCardUisAtPosition(RectTransform destination, params CardUI[] cardUis)
    {
        for (int i = 0; i < cardUis.Length; i++)
        {
            cardUis[i].RectTransform.SetPosition(destination);
            cardUis[i].VisualsRectTransform.SetScale(0.1f);
        }
        OnCardComboDone?.Invoke(cardUis);
    }


    public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager battleUIManager)
    {
        var data = battleUIManager.BattleDataManager;
        data.OnBattleManagerDestroyed += BeforeBattleFinished;
        data.ComboManager.OnCraftingComboToHand += CraftComboCards;
        _waitForDrawBetweenCards = new WaitForSeconds(_delaybetweenDrawCards);
    }

    private void BeforeBattleFinished(IBattleManager battleManager)
    {
        battleManager.OnBattleManagerDestroyed -= BeforeBattleFinished;
        battleManager.ComboManager.OnCraftingComboToHand -= CraftComboCards;
    }

}
