using Battles;
using Battles.UI;

using UnityEngine;

public class UpperInfoUIHandler : MonoBehaviour
{
    [SerializeField] MoneyIcon _moneyHandler;
    [SerializeField] UIBar _hpBar;



    // Deck Show
    [SerializeField] Art.ArtSO _art;
    [SerializeField] GameObject _deckContainer;
    [SerializeField] GameObject[] _cardUIGOs;
    [SerializeField] GameObject cardUIGO;
    [SerializeField] RectTransform _cardUIpanel;
    public static UpperInfoUIHandler Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUpperInfoHandler(ref BattleData.Player.CharacterData.CharacterStats);
    }
    public void UpdateUpperInfoHandler(ref Characters.Stats.CharacterStats stats)
    {
        _hpBar.SetMaxValue(stats.MaxHealth);
        _hpBar.SetValueBar(stats.Health);
        _moneyHandler.SetMoneyText( stats.Gold);
    }

    public void OpenDeck()
    {
        if (_cardUIGOs != null && _cardUIGOs.Length > 0)
        {
            for (int i = 0; i < _cardUIGOs.Length; i++)
            {
                Destroy(_cardUIGOs[i]);
            }
        }

        var deck = BattleData.Player.CharacterData.CharacterDeck;
        _cardUIGOs = new GameObject[deck.Length];
        for (int i = 0; i < deck.Length; i++)
        {
            _cardUIGOs[i] = Instantiate(cardUIGO, _cardUIpanel);
            _cardUIGOs[i].GetComponent<CardUI>().GFX.SetCardReference(deck[i], _art);
            _cardUIGOs[i].transform.localScale = Vector3.one * 0.5f;
        }
    }


}
