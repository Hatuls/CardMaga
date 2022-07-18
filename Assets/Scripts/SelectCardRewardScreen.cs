using Battles.UI;
using Cards;
using Rewards.Battles;
using TMPro;
using UI.Meta.Laboratory;
using UnityEngine;
namespace UI
{

public class SelectCardRewardScreen : MonoBehaviour
{
    [SerializeField]
    MetaCardUIHandler[] _cards;
    [SerializeField]
    BattleRewardHandler _battleRewardHandler;
    [SerializeField]
    BattleUIRewardHandler _battleUIRewardHandler;
    [SerializeField]
    TextMeshProUGUI _moneyText;
    ushort _money;
    [SerializeField]
    PresentCardUIScreen _presentCardUIScreen;


    public void AssignRewardCardScreen(Card[] cards, ushort money)
    {
        if (_cards.Length != cards.Length)
            throw new System.Exception($"SelectCardRewardScreen: Rewarded Cards Is bigger Than Given Option\nBattle Reward Cards: {cards.Length}\nCard UI Length: {_cards.Length}");

       
        for (int i = 0; i < cards.Length; i++)
            _cards[i].CardUI.AssignCard(cards[i]);

        _money = money;
            _moneyText.text = string.Concat("Do you want to get ", money, " credits INSTEAD of choosing a card?");
        gameObject.SetActive(true);

    }
    public void CollectSelectedCardUI()
    {
        _battleRewardHandler.AddCard(_presentCardUIScreen.CardUI.CardData);
        gameObject.SetActive(false);

        _battleUIRewardHandler.ReturnFromCardsSelection();
    }

    private void AssignInfoEvent(bool toAssign)
    {
        _presentCardUIScreen?.SubScribe(toAssign,  _cards);
    }
    private void OnEnable()
    {
        AssignInfoEvent(true);
    }
    private void OnDisable()
    {
        AssignInfoEvent(false);
    }
    private void OnDestroy()
    {
        AssignInfoEvent(false);
    }
    public void SelectCardUI(int i)
    {
        _battleRewardHandler.AddCard(_cards[i].CardUI.CardData);
        gameObject.SetActive(false);

        _battleUIRewardHandler.ReturnFromCardsSelection();
    }    
    public void SelectMoney()
    {
        _battleRewardHandler.AddMoney(_money);

        gameObject.SetActive(false);
        _battleUIRewardHandler.ReturnFromCardsSelection();
    }
}

}