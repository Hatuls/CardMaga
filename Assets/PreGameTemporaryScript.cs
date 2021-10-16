using Battles;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class PreGameTemporaryScript : MonoBehaviour
{
    
    [SerializeField] SceneLoaderCallback _sceneloaderEvent;
    [SerializeField] BattleData _data;
    [SerializeField]
    Character player;


    
    [SerializeField] TextMeshProUGUI _battleBtnTxt;
    [SerializeField] TextMeshProUGUI _goldText;
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] Button _newRunBtn;
    [SerializeField] Button _BattleBtn;
    private void Start()
    {
        player = _data.PlayerCharacterData;
        if (player == null)
        {
            _battleBtnTxt.text = "Start!";
            _newRunBtn.gameObject.SetActive(false);
            _goldText.gameObject.SetActive(false);
            _healthText.gameObject.SetActive(false);
        }
        else
        {
            _goldText.text = string.Concat("Gold: ",player.CharacterStats.Gold );
            _goldText.gameObject.SetActive(true);
            _healthText.text = string.Concat("Health: ", player.CharacterStats.Health);
            _healthText.gameObject.SetActive(true);
            _battleBtnTxt.text = "Continue!";
            _newRunBtn.gameObject.SetActive(true);
        }
    }
    public void StartNewRun()
    {
        player = Factory.GameFactory.Instance.CharacterFactoryHandler.CreateCharacter( CharacterTypeEnum.Player);
        _data.Initbattle( CharacterTypeEnum.Basic_Enemy,player);
   
    }

    public void Battle()
    {
        if (_data == null)
            throw new System.Exception("BattleSO data  Was not assigned!");

        if (_data.PlayerCharacterData == null || _data.PlayerCharacterData.CharacterStats.Health <= 0)
            StartNewRun();

        _sceneloaderEvent.LoadScene(3);
    }
}
