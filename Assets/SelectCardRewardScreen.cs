using Battles.UI;
using Cards;
using Rewards.Battles;
using TMPro;
using UnityEngine;

public class SelectCardRewardScreen : MonoBehaviour
{
    [SerializeField]
    CardUI[] _cards;
    [SerializeField]
    BattleRewardHandler _battleRewardHandler;
    [SerializeField]
    BattleUIRewardHandler _battleUIRewardHandler;
    [SerializeField]
    TextMeshProUGUI _moneyText;
    ushort _money;
    [SerializeField]
    PresentCardUIScreen _presentCardUIScreen;
    public void OnOpenCardUI(Card[] cards, ushort money)
    {
        if (_cards.Length != cards.Length)
            throw new System.Exception($"SelectCardRewardScreen: Rewarded Cards Is bigger Than Given Option\nBattle Reward Cards: {cards.Length}\nCard UI Length: {_cards.Length}");

        var artBoard = Factory.GameFactory.Instance.ArtBlackBoard;
        for (int i = 0; i < cards.Length; i++)
            _cards[i].GFX.SetCardReference(cards[i], artBoard);

        _money = money; 
        _moneyText.text = money.ToString();
        gameObject.SetActive(true);

    }


    private void AssignInfoEvent(bool toAssign)
    {
        _presentCardUIScreen.SubScribe(toAssign,  _cards);
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
        _battleRewardHandler.AddCard(_cards[i].GFX.GetCardReference);
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
