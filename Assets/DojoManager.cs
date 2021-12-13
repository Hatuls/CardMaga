using Battles;
using Battles.UI;
using Sirenix.OdinInspector;
using DesignPattern;
using UI;
using UI.Meta.Laboratory;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DojoManager : MonoBehaviour, IObserver
{
    private static DojoManager _instance;
    public static DojoManager Instance { get => _instance; }

    [TitleGroup("Dojo")]
    [TabGroup("Dojo/Component", "Card UI")]
    [SerializeField]
    MetaCardUIHandler[] _metaCardUIs;
    [TabGroup("Dojo/Component", "Combo UI")]
    [SerializeField]
    ComboRecipeUI[] _comboUI;


    [TabGroup("Dojo/Component", "Events")]
    [SerializeField]
    UnityEvent OnClosingUpgradePanel;
    [TabGroup("Dojo/Component", "Events")]
    [SerializeField]
    UnityEvent OnOpeningUpgradePanel;

    [TabGroup("Dojo/Component", "Panels")]
    [SerializeField]
    GameObject _dojoMainScreen;

    [TabGroup("Dojo/Component", "Panels")]
    [SerializeField]
    GameObject _upgradeScrollContainer;
 

    [TabGroup("Dojo/Component", "Panels")]
    [SerializeField]
    GameObject _presentCardPanel;
    [TabGroup("Dojo/Component", "Panels")]
    [SerializeField]
    GameObject _presentComboPanel;



    [TabGroup("Dojo/Component", "Others")]
    [SerializeField]
    ObserverSO _observer; 
    [TabGroup("Dojo/Component", "Others")]
    [SerializeField]
    string _soldtext = "Sold";
    [TabGroup("Dojo/Component", "Others")]
    [SerializeField]
    SoundEventSO SuccessfullPurchaseSound;
    [TabGroup("Dojo/Component", "Others")]
    [SerializeField]
    SoundEventSO UnSuccessfullPurchaseSound;
    [TabGroup("Dojo/Component", "Others")]
    [SerializeField]
    MapUpgradesCollection _upgradeHandler;  
    [SerializeField]
    [TabGroup("Dojo/Component", "Others")]
    MoneyIcon _moneyIcon;


    [SerializeField]
    TextMeshProUGUI[] _comboTexts;
 

    [SerializeField]
    GameObject[] _comboPurchaseBtns;
 
    private void Start()
    {
        _instance = this;
        ClosePanels();
    }
    public void InitDojo()
    {
        _observer.Notify(this);
        AssignDojosValues();
        //assign cards and combos Inputs
        OpenDojoPanel();
    }
    public void CloseUpgradeScreen()
    {
        OnClosingUpgradePanel?.Invoke();
        gameObject.SetActive(true);
        _upgradeHandler.Close();
        ClosePanels();
        OpenDojoPanel();
    }
    public void OpenUpgradeScreen()
    {
        ClosePanels();
        _dojoMainScreen.SetActive(false);
        OnOpeningUpgradePanel?.Invoke();
        _upgradeHandler.Open();
        _upgradeScrollContainer.SetActive(true);
      
    }

    #region Assign Values
    private void AssignCards(Rewards.BattleRewardCollectionSO battleReward)
    {
        int amountOfCards = _metaCardUIs.Length;
        Cards.Card[] cards = battleReward.GetRewardCards(Rewards.ActsEnum.ActOne, (byte)amountOfCards);

        for (byte i = 0; i < amountOfCards; i++)
        {
            var metaCard = _metaCardUIs[i].MetaCardUIInteraction;
            metaCard.ResetEnum();
            metaCard.ClosePanel();
            metaCard.SetClickFunctionality(MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Buy, BuyCard);
            metaCard.BuyBtn.SetText(cards[i].CardSO.PurchaseCost.ToString());
            metaCard.OpenInteractionPanel();
            _metaCardUIs[i].CardUI.DisplayCard(cards[i]);
        }
    }
    private void AssingCombos(Rewards.BattleRewardCollectionSO battleReward)
    {
        int amountOfCombos = _comboUI.Length;
        var combos = battleReward.GetRewardCombos(Rewards.ActsEnum.ActOne, (byte)amountOfCombos);
        var comboFactory = Factory.GameFactory.Instance.ComboFactoryHandler;
        for (byte i = 0; i < amountOfCombos; i++)
        {
            if (combos[i] != null)
            {
                _comboTexts[i].text = combos[i].ComboSO.Cost.ToString();
                _comboUI[i].gameObject.SetActive(true);
            _comboUI[i].InitRecipe(comboFactory.CreateCombo(combos[i].ComboSO, 0));
            }
            else
            {
                _comboUI[i].gameObject.SetActive(false);
               
            }
            _comboPurchaseBtns[i].gameObject.SetActive(combos[i] != null);
        }
    }
    private void AssignDojosValues()
    {
        isPurchaseable = new bool[2]
        {
            true,true
        };
        var battleRewards = Factory.GameFactory.Instance.RewardFactoryHandler.BattleRewardCollection;
        AssignCards(battleRewards);
        AssingCombos(battleRewards);
    }
    #endregion

    private void BuyCard(CardUI cardUI)
    {

        var card = cardUI.RecieveCardReference();
        if (BattleData.Player.CharacterData.CharacterStats.Gold >= card.CardSO.PurchaseCost)
        {
            //card added
            BattleData.Player.CharacterData.CharacterStats.Gold -= card.CardSO.PurchaseCost;
            _moneyIcon.SetMoneyText(BattleData.Player.CharacterData.CharacterStats.Gold);
            BattleData.Player.AddCardToDeck(card);
            for (int i = 0; i < _metaCardUIs.Length; i++)
            {
                if (_metaCardUIs[i].CardUI.RecieveCardReference().CardInstanceID == card.CardInstanceID)
                {
                    var metaCardInteraction = _metaCardUIs[i].MetaCardUIInteraction;
                    metaCardInteraction.ResetEnum();
                    metaCardInteraction.BuyBtn.SetText(_soldtext);
                    break;
                }
            }
            SuccessfullPurchaseSound.PlaySound();
        }
        else
        {
            // not enough gold
            UnSuccessfullPurchaseSound.PlaySound();
        }
    }
    private void BuyCombo(Combo.Combo combo)
    {
        int cost = combo.ComboSO.Cost;
        if (BattleData.Player.CharacterData.CharacterStats.Gold >= cost)
        {
            //card added
            BattleData.Player.CharacterData.CharacterStats.Gold -= cost;
            BattleData.Player.AddComboRecipe(combo);
            _moneyIcon.SetMoneyText(BattleData.Player.CharacterData.CharacterStats.Gold);
            SuccessfullPurchaseSound.PlaySound();
        }
        else
        {
            UnSuccessfullPurchaseSound.PlaySound();
            // not enough gold
        }
    }

    public void ExitDojo()
    {
        CloseUpgradeScreen();
        ClosePanels();
        _observer.Notify(null);
    }
    public void OpenDojoPanel()
    {
        _dojoMainScreen.SetActive(true);
    }
    private void ClosePanels()
    {
        _dojoMainScreen.SetActive(false);
        _presentComboPanel.SetActive(false);
        _presentCardPanel.SetActive(false);
    }
    
    public void OnNotify(IObserver Myself)
    {
      
    }
    bool[] isPurchaseable;
    public void TryBuyCombo(int index)
    {
        if (index == 0 )
        {
            if (BattleData.Player.CharacterData.CharacterStats.Gold >= _comboUI[index].ComboRecipe.Cost && isPurchaseable[index])
            {
                isPurchaseable[index] = false;
                //card added
                BattleData.Player.CharacterData.CharacterStats.Gold -= _comboUI[index].ComboRecipe.Cost;
                _moneyIcon.SetMoneyText(BattleData.Player.CharacterData.CharacterStats.Gold);
                BattleData.Player.AddComboRecipe(_comboUI[index].Combo);
                _comboTexts[index].text = "Sold";
                SuccessfullPurchaseSound.PlaySound();
            }
            else
            {
                // not enough gold
                UnSuccessfullPurchaseSound.PlaySound();
            }

        }
        else if (index == 1)
        {
            if (BattleData.Player.CharacterData.CharacterStats.Gold >= _comboUI[index].ComboRecipe.Cost && isPurchaseable[index])
            {
                isPurchaseable[index] = false;
                //card added
                BattleData.Player.CharacterData.CharacterStats.Gold -= _comboUI[index].ComboRecipe.Cost;
                _moneyIcon.SetMoneyText(BattleData.Player.CharacterData.CharacterStats.Gold);
                BattleData.Player.AddComboRecipe(_comboUI[index].Combo);
                _comboTexts[index].text = "Sold";
                SuccessfullPurchaseSound.PlaySound();
            }
            else
            {
                // not enough gold
                UnSuccessfullPurchaseSound.PlaySound();
            }
        }
    }
}
