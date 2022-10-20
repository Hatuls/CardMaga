using System;
using System.Collections.Generic;
using Battle.Combo;
using CardMaga.Card;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class ComboAndDeckCollictonMainHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _deckCollection;
        [SerializeField] private GameObject _comboCollection;
        [SerializeField] private TMP_Text _comboAndDecksButtonText;
        [SerializeField] private ComboAndDeckButton _comboAndDeckButton;
        [SerializeField] private ComboUIScrollPanelManager _comboUIScroll;
        [SerializeField] private CardUIScrollPanelManager _cardUIScroll;
        [SerializeField] private CardDataFilterSystem _cardDataFilter;
        [SerializeField] private ComboDataFilterSystem _comboDataFilter;
        
        [SerializeField] private CardDataFilter _test;

        private CardDataSort _cardDataSort;
        private ComboDataSort _comboDataSort;
        
        private List<CardData> _cardDatas;
        private List<ComboData> _comboDatas;

        private void Awake()
        {
            _cardDataSort = new CardDataSort();
            _comboDataSort = new ComboDataSort();
        }

        private void Start()
        {
            _comboAndDeckButton.ComboState.OnClick += SetToDeckScreen;
            _comboAndDeckButton.DeckState.OnClick += SetToComboScreen;
            ShowObjects();
        }

        private void ShowObjects()
        {
            _cardUIScroll.AddObjectToPanel(_cardDataSort.SortCardData(_cardDataFilter.Filter(_cardDatas)));
            _comboUIScroll.AddObjectToPanel(_comboDataSort.SortComboData(_comboDataFilter.Filter(_comboDatas)));
        }

        private void OnDestroy()
        {
            _comboAndDeckButton.ComboState.OnClick -= SetToDeckScreen;
            _comboAndDeckButton.DeckState.OnClick -= SetToComboScreen;
        }

        public void Init(List<CardData> cardDatas, List<ComboData> comboDatas)
        {
            _cardDatas = cardDatas;
            _comboDatas = comboDatas;
            _cardUIScroll.Init();
            _comboUIScroll.Init();
        }

        private void SetToComboScreen(ButtonGenaric buttonGenaric)
        {
            _deckCollection.SetActive(false);
            _comboCollection.SetActive(true);
            _comboAndDecksButtonText.text = "Decks";
            _comboAndDeckButton.SetToComboState();
        }

        private void SetToDeckScreen(ButtonGenaric buttonGenaric)
        {
            _deckCollection.SetActive(true);
            _comboCollection.SetActive(false);
            _comboAndDecksButtonText.text = "Combo";
            _comboAndDeckButton.SetToDeckState();
        }
    }
}
