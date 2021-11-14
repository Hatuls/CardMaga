
using Battles;
using Battles.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Map.UI
{

    public class RemoveCardPanelScreen : MonoBehaviour
    {
        [SerializeField]
        GameObject _cardUIPrefab;
        [SerializeField]
        List<CardUI> _deckCardsUI;
        CardUI _holdingCard;

        [SerializeField] RestAreaUI _restAreaUI;

        public void OnDisable()
        {
            int length = _deckCardsUI.Count;
            for (int i = 0; i < length; i++)
                _deckCardsUI[i].Inputs.OnPointerClickEvent -= SelectedCard;

        }

        public void SetActivePanel(bool state) => gameObject.SetActive(state);

        internal void OpenRemoveCardScreen()
        {
            _holdingCard = null;

            if (!gameObject.activeSelf)
                SetActivePanel(true);

           int length = _deckCardsUI.Count;
            for (int i = 0; i < length; i++)
                _deckCardsUI[i].Inputs.OnPointerClickEvent += SelectedCard;

            ShowAllCards();
        }
        private void CreateCards()
        {
            var deck = BattleData.Player.CharacterData.CharacterDeck;
            while (deck.Length > _deckCardsUI.Count)
            {
                var card = Instantiate(_cardUIPrefab, this.transform).GetComponent<CardUI>();
                _deckCardsUI.Add(card);
            }

        }
        private void ShowAllCards()
        {
            CreateCards();
            int length = _deckCardsUI.Count;
            var deck = BattleData.Player.CharacterData.CharacterDeck;
            var artSO = Factory.GameFactory.Instance.ArtBlackBoard;
            for (int i = 0; i < length; i++)
            {
                if (i < deck.Length)
                {
                    if (_deckCardsUI[i].gameObject.activeSelf == false)
                        _deckCardsUI[i].gameObject.SetActive(true);

           
                    _deckCardsUI[i].GFX.SetCardReference(deck[i], artSO);
                }
                else
                {
                    if (_deckCardsUI[i].gameObject.activeSelf == true)
                        _deckCardsUI[i].gameObject.SetActive(false);
                }


            }


        }

        private void SelectedCard(CardUI card, PointerEventData data)
        {
            _holdingCard = card;
            card.GFX.GlowCard(true);
            card.CardAnimator.PlayNoticeAnimation();
        }

        public void Confirm()
        {


            if (_holdingCard == null)
                Cancel();
            else
                _restAreaUI.RemoveCardUI(_holdingCard);

            if (gameObject.activeSelf)
                SetActivePanel(false);
        }

        public void Cancel()
        {
            if (gameObject.activeSelf)
                SetActivePanel(false);

            _restAreaUI.CancelRemoveCardUI();
        }
    }
}