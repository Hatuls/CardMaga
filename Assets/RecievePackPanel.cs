using UnityEngine;
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


    ushort _chipAmount;
    public void Open(PackRewardSO pack )
    {
        _chipAmount = 0;
        var factory = Factory.GameFactory.Instance;
        var rewards = pack.CreatePackReward();
        var card = factory.CardFactoryHandler.CreateCard(rewards.RewardCard);
        _titleText.text = card.CardSO.Rarity.ToString();
        _currentCard.GFX.SetCardReference(card, factory.ArtBlackBoard);
        SetChipValues(rewards);

        SetOpenCost(pack.PurchaseCosts[0].Price);

        gameObject.SetActive(true);
    }

    private void SetOpenCost(int price)
    {
        if (Account.AccountManager.Instance.AccountGeneralData.AccountResourcesData.Diamonds.Value >= price)
        {
            _chipAmount = (ushort)price;
            _openAgainCost.text = _chipAmount.ToString();
            buyAgainContainer.SetActive(true);
        }
        else
            buyAgainContainer.SetActive(false);
    }

    private void SetChipValues(PackReward pack)
    {
        if (pack.Reward==null)
        {
            _resourceHolder.gameObject.SetActive(false);
        }
        else
        {

            _chipAmountText.text = string.Concat("X",pack.Reward.Price);
            _resourceHolder.gameObject.SetActive(true);
        }
    }
    private void RecieveChip()
    {
        if(_chipAmount>0)
        Account.AccountManager.Instance.AccountGeneralData.AccountResourcesData.Chips.AddValue(_chipAmount);
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
