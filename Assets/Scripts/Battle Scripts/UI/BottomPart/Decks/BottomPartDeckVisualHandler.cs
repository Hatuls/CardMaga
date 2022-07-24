﻿using UnityEngine;
using CardMaga.UI.Text;
using Battle.Deck;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

namespace CardMaga.UI
{
    public class BottomPartDeckVisualHandler : MonoBehaviour
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
        }
        private void Start()
        {
            var deckManager = DeckManager.Instance;
            _drawDeckTextAssigner.Init(deckManager.GetBaseDeck(true,DeckEnum.PlayerDeck));
            _discardDeck = deckManager.GetBaseDeck(true, DeckEnum.Discard);
            _discardDeckTextAssigner.Init(_discardDeck);
            _discardDeck.OnResetDeck += MoveCardsToDrawPileAnim;
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
            _discardDeck.OnResetDeck -= MoveCardsToDrawPileAnim;
        }
    }
}
