using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle.UI;
using CardMaga.Card;
using CardMaga.UI;
using CardMaga.UI.Card;
using UnityEngine;

public class ComboUIManager : MonoBehaviour
{
   public event Action<CardUI[]> OnCardComboDone; 

   [Header("Scripts Reference")]
   [SerializeField] private CardUIManager _cardUIManager;
   [SerializeField] private HandUI _handUI;
   [SerializeField] private ComboManager _comboManager;

   [Header("RectTransforms")] 
   [SerializeField] private RectTransform _drawPosition;
   [SerializeField] private RectTransform _destination;

   [Header("TransitionPackSOs")] 
   [SerializeField] private TransitionPackSO _drawTransitionPackSo;

   [Header("Draw Parameters")] 
   [SerializeField] private float _delaybetweenDrawCards;
   
   private WaitForSeconds _waitForDrawBetweenCards;

   private void Start()
   {
      _comboManager.OnCraftingComboToHand += CraftComboCards;
      _waitForDrawBetweenCards = new WaitForSeconds(_delaybetweenDrawCards);
   }

   private void OnDestroy()
   {
      _comboManager.OnCraftingComboToHand -= CraftComboCards;
   }

   private void CraftComboCards(params CardData[] cardDatas)
   {
      CardUI[] cardUis = _cardUIManager.GetCardsUI(cardDatas);
      
      SetCardUisAtPosition(_drawPosition,cardUis);
   }

   private void SetCardUisAtPosition(RectTransform destination ,params CardUI[] cardUis)
   {
      for (int i = 0; i < cardUis.Length; i++)
      {
         cardUis[i].RectTransform.SetPosition(destination);
         cardUis[i].RectTransform.SetScale(0.1f);
      }
      OnCardComboDone?.Invoke(cardUis);
   }

   private IEnumerator MoveCardsUiToPosition(RectTransform destination, params CardUI[] cardUis)
   {
      for (int i = 0; i < cardUis.Length; i++)
      {
         cardUis[i].Init();
         cardUis[i].RectTransform.Transition(destination, _drawTransitionPackSo);
         yield return _waitForDrawBetweenCards;
         
      }
   }
}
