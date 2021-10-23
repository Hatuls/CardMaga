using Battles;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Battles.UI;
using Art;

public class PreGameTemporaryScript : MonoBehaviour
{

    [SerializeField] ArtSO art;
    [SerializeField] SceneLoaderCallback _sceneloaderEvent;

    
 

    [SerializeField] RectTransform _cardUIpanel;
    [SerializeField] GameObject _showDeckBtn;
    [SerializeField] GameObject _deckContainer;
    [SerializeField] GameObject[] _cardUIGOs;
    [SerializeField] GameObject cardUIGO;

    

    [SerializeField] TextMeshProUGUI _battleBtnTxt;
    [SerializeField] TextMeshProUGUI _goldText;
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] Button _newRunBtn;
    [SerializeField] Button _BattleBtn;


    
    private void Start()
    {
      var  player = BattleData.Player;
        if (player == null || player.CharacterData.CharacterStats.Health <=0)
        {
            _battleBtnTxt.text = "Start!";
            _newRunBtn.gameObject.SetActive(false);
            _goldText.gameObject.SetActive(false);
            _healthText.gameObject.SetActive(false);
            _showDeckBtn.SetActive(false);
        }
        else
        {
            _goldText.text = string.Concat("Gold: ",player.CharacterData.CharacterStats.Gold );
            _goldText.gameObject.SetActive(true);
            _healthText.text = string.Concat("Health: ", player.CharacterData.CharacterStats.Health);
            _healthText.gameObject.SetActive(true);
            _battleBtnTxt.text = string.Concat("Continue!");
            _newRunBtn.gameObject.SetActive(true);
            _showDeckBtn.SetActive(true);
        }
            _deckContainer.SetActive(false);
    }
    public void StartNewRun()
    {
        var characterHandler = Factory.GameFactory.Instance.CharacterFactoryHandler;
        var player = characterHandler.CreateCharacter(CharacterTypeEnum.Player);

        Debug.Log("Player created! " + player.CharacterData.CharacterStats.Health + " deck legth: " + player.CharacterData.CharacterDeck.Length);

        BattleData.Opponent = characterHandler.CreateCharacter(CharacterTypeEnum.Elite_Enemy);

        _battleBtnTxt.text = "Start!";
        _newRunBtn.gameObject.SetActive(false);
        _goldText.gameObject.SetActive(false);
        _healthText.gameObject.SetActive(false);
        _showDeckBtn.SetActive(false);

        BattleData.Player = player;
    }

    public void Battle()
    {

        if (BattleData.Player == null || BattleData.Player.CharacterData.CharacterStats.Health <= 0)
            StartNewRun();

        
        
        _sceneloaderEvent.LoadScene(3);
    }

    public void OpenDeck()
    {
        if (_cardUIGOs != null && _cardUIGOs.Length >0)
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
            _cardUIGOs[i].GetComponent<CardUI>().GFX.SetCardReference(deck[i], art);
            _cardUIGOs[i].transform.localScale = Vector3.one * 0.5f;
        }
    }

}
