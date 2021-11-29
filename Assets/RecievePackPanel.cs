﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Battles.UI;
using Rewards;

public class RecievePackPanel : MonoBehaviour
{
    [SerializeField]
    Image _titleImage;
    [SerializeField]
    CardUI _currentCard;

    [SerializeField] 
    Image _chipImage;

    [SerializeField]
    TextMeshProUGUI _chipAmountText;


    [SerializeField]
    TextMeshProUGUI _openAgainCost;
    [SerializeField]
    TextMeshProUGUI _openAgainText;
    [SerializeField]
    TextMeshProUGUI _titleText;

    [SerializeField]
    GameObject _resourceHolder;
    [SerializeField]
    GameObject buyAgainContainer;


    PackReward _pack;
    public void Open(PackRewardSO pack )
    {
       
        var factory = Factory.GameFactory.Instance;
        _pack = pack.CreatePackReward();
        var card = factory.CardFactoryHandler.CreateCard(_pack.RewardCard);
        _titleText.text = card.CardSO.Rarity.ToString();
        _currentCard.GFX.SetCardReference(card, factory.ArtBlackBoard);
        SetChipValues();

        SetOpenCost(pack.PurchaseCosts[0].Price);

        gameObject.SetActive(true);
    }

    private void SetOpenCost(int price)
    {
        if (Account.AccountManager.Instance.AccountGeneralData.AccountResourcesData.Diamonds.Value >= price)
        {
       
           
            _openAgainCost.text = price.ToString();
            buyAgainContainer.SetActive(true);
        }
        else
            buyAgainContainer.SetActive(false);
    }

    private void SetChipValues()
    {
        if (_pack.Reward==null || _pack.Reward.Price == 0)
        {
            _resourceHolder.gameObject.SetActive(false);
        }
        else
        {

            _chipAmountText.text = string.Concat("X", _pack.Reward.Price);
            _resourceHolder.gameObject.SetActive(true);
        }
    }
    private void RecieveChip()
    {
        if (_pack.Reward != null && _pack.Reward.Price > 0)
        {
            Debug.Log("Adding " + _pack.Reward.Price);
        Account.AccountManager.Instance.AccountGeneralData.AccountResourcesData.Chips.AddValue(_pack.Reward.Price);
        }
    }
    private void RecieveCard()
    {
        Account.AccountManager.Instance.AccountCards.AddCard(_currentCard.GFX.GetCardReference.CardCoreInfo);
    }
    public void RecievePack()
    {
        RecieveChip();
        RecieveCard();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
   
}
