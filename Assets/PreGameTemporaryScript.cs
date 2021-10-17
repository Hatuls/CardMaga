using Battles;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Battles.UI;
using Art;

public class PreGameTemporaryScript : MonoBehaviour
{

    [SerializeField] ArtSO art;
    [SerializeField] SceneLoaderCallback _sceneloaderEvent;
    [SerializeField] BattleData _data;
    
    Character player;

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
        player = _data.PlayerCharacterData;
        if (player == null || _data.PlayerCharacterData.CharacterStats.Health <=0)
        {
            _battleBtnTxt.text = "Start!";
            _newRunBtn.gameObject.SetActive(false);
            _goldText.gameObject.SetActive(false);
            _healthText.gameObject.SetActive(false);
            _showDeckBtn.SetActive(false);
        }
        else
        {
            _goldText.text = string.Concat("Gold: ",player.CharacterStats.Gold );
            _goldText.gameObject.SetActive(true);
            _healthText.text = string.Concat("Health: ", player.CharacterStats.Health);
            _healthText.gameObject.SetActive(true);
            _battleBtnTxt.text = "Continue!";
            _newRunBtn.gameObject.SetActive(true);
            _showDeckBtn.SetActive(true);
        }
            _deckContainer.SetActive(false);
    }
    public void StartNewRun()
    {
        StartCoroutine(Try());
    }

    private IEnumerator Try()
    {
        player = Factory.GameFactory.Instance.CharacterFactoryHandler.CreateCharacter(CharacterTypeEnum.Player);
        yield return null;
        Debug.Log("Player created! " + player.CharacterStats.Health + " deck legth: " + player.CharacterDeck.Length);

        yield return   _data.Initbattle(CharacterTypeEnum.Basic_Enemy, player);

        _battleBtnTxt.text = "Start!";
        _newRunBtn.gameObject.SetActive(false);
        _goldText.gameObject.SetActive(false);
        _healthText.gameObject.SetActive(false);
        _showDeckBtn.SetActive(false);

   
    }
    public void Battle()
    {
        if (_data == null)
            throw new System.Exception("BattleSO data  Was not assigned!");

        if (_data.PlayerCharacterData == null || _data.PlayerCharacterData.CharacterStats.Health <= 0)
            StartNewRun();

     StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return null;
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

        var deck = player.CharacterDeck;
        _cardUIGOs = new GameObject[deck.Length];
        for (int i = 0; i < deck.Length; i++)
        {
            _cardUIGOs[i] = Instantiate(cardUIGO, _cardUIpanel);
            _cardUIGOs[i].GetComponent<CardUI>().GFX.SetCardReference(deck[i], art);
            _cardUIGOs[i].transform.localScale = Vector3.one * 0.5f;
        }
    }

}
