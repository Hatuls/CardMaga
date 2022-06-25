
using Battles;
using Battles.UI;
using System.Collections.Generic;
using UI;
using UI.Meta.Laboratory;
using UnityEngine;
using UnityEngine.EventSystems;
namespace CardMaga.UI
{

    public class RemoveCardPanelScreen : MonoBehaviour
    {
        [SerializeField]
        GameObject _cardUIPrefab;      

        [SerializeField]
        List<MetaCardUIHandler> _deckCardsUI;
        CardUI _holdingCard;

        [SerializeField] RestAreaUI _restAreaUI;

        [SerializeField]
        PresentCardUIScreen _presentCardUIScreen;

        [SerializeField]
        Transform _container;
        public void OnDisable()
        {
            int length = _deckCardsUI.Count;
            for (int i = 0; i < length; i++)
                _deckCardsUI[i].OnCardUIClicked -= SelectedCard;

        }

        public void SetActivePanel(bool state) => gameObject.SetActive(state);

        internal void OpenRemoveCardScreen()
        {
            _holdingCard = null;

            if (_presentCardUIScreen.gameObject.activeSelf)
                _presentCardUIScreen.gameObject.SetActive(false);

            if (!gameObject.activeSelf)
                SetActivePanel(true);

            ShowAllCards();
           int length = _deckCardsUI.Count;
            for (int i = 0; i < length; i++)
            {
                _deckCardsUI[i].OnCardUIClicked += SelectedCard;
                _deckCardsUI[i].ToOnlyClickCardUIBehaviour = true;
            }

        }
        private void CreateCards()
        {
            //var deck = Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterDeck;
            //while (deck.Length > _deckCardsUI.Count)
            //{
            //    var card = Instantiate(_cardUIPrefab, _container ?? this.transform).GetComponent<MetaCardUIHandler>();
            //    _deckCardsUI.Add(card);
            //}

        }
        private void ShowAllCards()
        {
            //CreateCards();
            //int length = _deckCardsUI.Count;
            //var deck = Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterDeck;
        
            //for (int i = 0; i < length; i++)
            //{
            //    if (i < deck.Length)
            //    {
            //        if (_deckCardsUI[i].gameObject.activeSelf == false)
            //            _deckCardsUI[i].gameObject.SetActive(true);

           
            //        _deckCardsUI[i].CardUI.DisplayCard(deck[i]);
            //    }
            //    else
            //    {
            //        if (_deckCardsUI[i].gameObject.activeSelf == true)
            //            _deckCardsUI[i].gameObject.SetActive(false);
            //    }
            //}
        }

        private void SelectedCard(CardUI card)
        {
            _holdingCard = card;

            _presentCardUIScreen.OpenCardUIInfo(card);

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

        public void ReturnToRemovalSelection()
        {
            _holdingCard = null;
            if (_presentCardUIScreen.gameObject.activeSelf)
                _presentCardUIScreen.gameObject.SetActive(false);
        }
    }
}