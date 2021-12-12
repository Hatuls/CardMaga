using Battles.UI;
using UI;
using UI.Meta.Laboratory;
using UnityEngine;

public class DojoManager : MonoBehaviour
{
    [SerializeField]
    MetaCardUIHandler[] _metaCardUIs;

    [SerializeField]
    ComboRecipeUI[] _comboUI;

    [SerializeField]
    GameObject _dojoMainScreen;

    private void Start()
    {
        ClosePanels();
    }
    public void InitDojo()
    {
        AssignDojosValues();
        //assign cards and combos Inputs
        OpenDojoPanel();
    }
    public void CloseUpgradeScreen()
    {

    }
    public void OpenUpgradeScreen()
    {

    }
 

    private void AssignCards(Rewards.BattleRewardCollectionSO battleReward)
    {
        int amountOfCards = _metaCardUIs.Length;
        Cards.Card[] cards = battleReward.GetRewardCards(Rewards.ActsEnum.ActOne, (byte)amountOfCards);

        for (byte i = 0; i < amountOfCards; i++)
        {
            _metaCardUIs[i].CardUI.DisplayCard(cards[i]);
        }
    }
    private void AssingCombos(Rewards.BattleRewardCollectionSO battleReward)
    {
        int amountOfCombos = _comboUI.Length;
        var combos = battleReward.GetRewardCombos(Rewards.ActsEnum.ActOne, (byte)amountOfCombos);
        for (byte i = 0; i < amountOfCombos; i++)
        {
            _comboUI[i].InitRecipe(combos[i]);
        }
    }
    private void AssignDojosValues()
    {
        var battleRewards = Factory.GameFactory.Instance.RewardFactoryHandler.BattleRewardCollection;
        AssignCards(battleRewards);
        AssingCombos(battleRewards);
    }
    public void BuyCard(CardUI cardUI)
    {
        var card = cardUI.RecieveCardReference();
        if (Battles.BattleData.Player.CharacterData.CharacterStats.Gold >= card.CardSO.PurchaseCost)
        {
            //card added
            Battles.BattleData.Player.CharacterData.CharacterStats.Gold -= card.CardSO.PurchaseCost;
            Battles.BattleData.Player.AddCardToDeck(card);

        }
        else
        {

        // not enough gold
        }

    }
    public void BuyCombo(Combo.Combo combo)
    {
        var card = combo.ComboSO.Cost.RecieveCardReference();
        int cost = combo.ComboSO.Cost;
        if (Battles.BattleData.Player.CharacterData.CharacterStats.Gold >= combo.ComboSO.Cost)
        {
            //card added
            Battles.BattleData.Player.CharacterData.CharacterStats.Gold -= combo.ComboSO.Cost;
            Battles.BattleData.Player.AddComboRecipe(combo);

        }
        else
        {

            // not enough gold
        }
    }

    public void ExitDojo()
    {
        CloseUpgradeScreen();
        ClosePanels();
    }
    private void OpenDojoPanel()
    {
        CloseUpgradeScreen();
        _dojoMainScreen.SetActive(true);
    }
    private void ClosePanels()
    {
        _dojoMainScreen.SetActive(false);

    }
}
