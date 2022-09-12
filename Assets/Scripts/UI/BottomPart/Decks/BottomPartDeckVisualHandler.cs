using UnityEngine;
using CardMaga.UI.Text;
using Battle.Deck;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using ReiTools.TokenMachine;
using Battle;
using Managers;

namespace CardMaga.UI
{
    public class BottomPartDeckVisualHandler : MonoBehaviour , ISequenceOperation<BattleManager>
    {
#if UNITY_EDITOR
        //[Header("Test")]
        [Button]
        public void TestAnimation()
        {
            MoveCardsToDrawPileAnim();
        }
        [Button]
        public void TestReset()
        {
            ResetDeckPosition();
        }
        [Button]
        public void TestForceReset()
        {
            ForceResetDecksPosition();
        }

#endif
        [Header("General")]
        [SerializeField] float _resetDuration = 0.3f;
  
        [Header("Draw Deck")]
        [SerializeField] DeckTextAssigner _drawDeckTextAssigner;
        [SerializeField] TransitionPackSO _drawDeckTransitionPackSO;
        [SerializeField] RectTransform _drawDeckRectTransform;
        Vector3 _drawDeckStartPos;


        [Header("Discard Deck")]
        [SerializeField] DeckTextAssigner _discardDeckTextAssigner;
        [SerializeField] TransitionPackSO _discardDeckTransitionPackSO;
        [SerializeField] RectTransform _discardDeckRectTransform;
        Vector3 _discardDeckStartPos;
        BaseDeck _discardDeck;

        public int Priority => 10;


        //handles deck visuals
        //need to know how to add/remove cards from deck
        //need to know to init deck
        //need to know how to shuffle between decks
        private void Awake()
        {
            if (_drawDeckRectTransform == null)
                throw new Exception("BottomPartDeckVisualHandler has no Draw Rect Transform");
            else
                _drawDeckStartPos = _drawDeckRectTransform.position;

            if (_discardDeckRectTransform == null)
                throw new Exception("BottomPartDeckVisualHandler has no discard Rect Transform");
            else
                _discardDeckStartPos = _discardDeckRectTransform.position;

            if (_drawDeckTransitionPackSO == null)
                throw new Exception("BottomPartDeckVisualHandler has no Draw Transition SO");
            if (_discardDeckTransitionPackSO == null)
                throw new Exception("BottomPartDeckVisualHandler has no Discard Transition SO");

            BattleManager.Register(this, OrderType.Before);
        }
  
        public void ExecuteTask(ITokenReciever tokenMachine,BattleManager battleManager)
        {
         
            using (tokenMachine.GetToken())
            {
                var deckHandler = battleManager.PlayersManager.GetCharacter(true).DeckHandler;
                _drawDeckTextAssigner.Init(deckHandler[DeckEnum.PlayerDeck]);
                _discardDeck = deckHandler[DeckEnum.Discard];
                _discardDeckTextAssigner.Init(_discardDeck);
                _discardDeck.OnResetDeck += MoveCardsToDrawPileAnim;
            }
        }
        void MoveCardsToDrawPileAnim()
        {
            _drawDeckRectTransform.Transition(_drawDeckTransitionPackSO).
                Join(_discardDeckRectTransform.Transition(_discardDeckTransitionPackSO));
        }
        public void ResetDeckPosition()
        {
            _drawDeckRectTransform.DOMove(_drawDeckStartPos, _resetDuration);
            _discardDeckRectTransform.DOMove(_discardDeckStartPos, _resetDuration);
        }
        public void ForceResetDecksPosition()
        {
            _drawDeckRectTransform.position = _drawDeckStartPos;
            _discardDeckRectTransform.position = _discardDeckStartPos;
        }
        private void OnDestroy()
        {
            if(_discardDeck != null)
            _discardDeck.OnResetDeck -= MoveCardsToDrawPileAnim;
     
            _drawDeckTextAssigner?.Dispose();
            _discardDeckTextAssigner?.Dispose();
                
        }
    }
}
