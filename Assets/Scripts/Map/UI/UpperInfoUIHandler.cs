using Battles;
using Battles.UI;
using DesignPattern;
using UnityEngine;
namespace Map.UI
{

public class UpperInfoUIHandler : MonoBehaviour 
{
    [SerializeField] MoneyIcon _moneyHandler;
    [SerializeField] UIBar _hpBar;
    [SerializeField]
    DeckAndCombosScreenUI _deckAndComboScreen;


    // Deck Show
    [SerializeField] Art.ArtSO _art;
    [SerializeField] GameObject _deckContainer;
    [SerializeField] GameObject[] _cardUIGOs;
    [SerializeField] GameObject cardUIGO;
    [SerializeField] RectTransform _cardUIpanel;
    [SerializeField] GameObject _dropList;
    public static UpperInfoUIHandler Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUpperInfoHandler(ref Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats);
        _dropList.SetActive(false);
    }
    public void UpdateUpperInfoHandler(ref Characters.Stats.CharacterStats stats)
    {
        _hpBar.SetMaxValue(stats.MaxHealth);
        _hpBar.SetValueBar(stats.Health);
        _moneyHandler.SetMoneyText( stats.Gold);
    }

    public void DropListChangeState()
    {
      _dropList.gameObject.SetActive(!_dropList.activeSelf);
    }
    public void OpenDeckAndComboScreen()
    {
        //DropListChangeState();
        _deckAndComboScreen.Open();
    }

   
}

}