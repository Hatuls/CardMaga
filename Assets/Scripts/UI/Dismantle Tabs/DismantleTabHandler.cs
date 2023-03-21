using Account.GeneralData;
using CardMaga.Card;
using CardMaga.MetaData.Dismantle;
using CardMaga.UI;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardMaga.MetaData.Collection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DismantleTabHandler : BaseUIElement
{

    [SerializeField]
    private DismantelUIManager _dismantelUIManager;

    [SerializeField]
    private TextMeshProUGUI _text;


    [SerializeField]
    private RarityTab[] _rarityTabs;

    [SerializeField,EventsGroup]
    private UnityEvent OnShowEvent, OnHideEvent;
    private DismantleHandler _dismantleHandler; // got the cards
    private DismantleCurrencyHandler _dismantleCurrencyHandler; // gots the amount

    private void Awake()
    {
        var data = _dismantelUIManager.DismantleDataManager;
        _dismantleHandler = data.DismantleHandler;
        _dismantleCurrencyHandler = data.DismantleCurrencyHandler;
    }

    public override void Show()
    {
        if (_dismantleHandler.DismantleCards != null && _dismantleHandler.DismantleCards.Count > 0)
        {
            OnShowEvent?.Invoke();
            base.Show();
            AssignTexts();
            DeactivateTabs();
            ActivateRarityTabs();
        }
       
    }
    public override void Hide()
    {
        OnHideEvent?.Invoke();
        base.Hide();
    }

    private void DeactivateTabs()
    {
        for (int i = 0; i < _rarityTabs.Length; i++)
            _rarityTabs[i].gameObject.SetActive(false);
    }

    private void ActivateRarityTabs()
    {
        IReadOnlyList<MetaCardInstanceInfo> cards = _dismantleHandler.DismantleCards;

        IEnumerable<RarityEnum> rarities =cards.Select(x => x.CardInstance.CardSO.Rarity).Distinct();

        foreach (var rarity in rarities)
        {
            InitRarityTab(rarity, cards.Count(x=>x.CardInstance.CardSO.Rarity == rarity));
        }

        void InitRarityTab(RarityEnum rarity , int amount)
        {
            var tab = _rarityTabs.First(x => x.gameObject.activeSelf == false);
            tab.Init(new RarityTextData() 
            { 
                RarityAmount = amount.ToString(),
                RarityType = rarity
            }
            );

            tab.gameObject.SetActive(true);
        }
    }

    private void AssignTexts()
    {
        const string ARE_YOU_SURE_TEXT = "Are you sure you want to dismantle ";
        const string CARDS = " Cards for:\n";


        const string AND = "    And      ";
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append(ARE_YOU_SURE_TEXT);
        stringBuilder.Append(_dismantleHandler.DismantleCards.Count);
        stringBuilder.Append(CARDS);
        stringBuilder.Append(_dismantleCurrencyHandler.ChipsCurrency.ToString().AddImageInFrontOfText(1));
        stringBuilder.Append(AND);
        stringBuilder.Append(_dismantleCurrencyHandler.GoldCurrency.ToString().AddImageInFrontOfText(0));
        _text.text = stringBuilder.ToString();

        stringBuilder.Clear();
    }
}
