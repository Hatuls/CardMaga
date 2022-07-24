using UnityEngine;
using CardMaga.UI.Text;
using Battle.Deck;
using DG.Tweening;

namespace CardMaga.UI
{
    public class BottomPartDeckVisualHandler : MonoBehaviour
    {
        [Header("Draw Deck")]
        [SerializeField] DeckTextAssigner _drawDeckTextAssigner;
        [SerializeField] TransitionPackSO _drawDeckTransitionPackSO;

        [Header("Discard Deck")]
        [SerializeField] DeckTextAssigner _discardDeckTextAssigner;
        [SerializeField] TransitionPackSO _discardDeckTransitionPackSO;
        BaseDeck _discardDeck;
        //handles deck visuals
        //need to know how to add/remove cards from deck
        //need to know to init deck
        //need to know how to shuffle between decks

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
            //_drawDeckRectTransitionManager.Transition(_drawDeckTransitionPackSO).
              //  Join(_discardDeckRectTransitionManager.Transition(_discardDeckTransitionPackSO));
        }
        private void OnDestroy()
        {
            _discardDeck.OnResetDeck -= MoveCardsToDrawPileAnim;
        }
    }
}
