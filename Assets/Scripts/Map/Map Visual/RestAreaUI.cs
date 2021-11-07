using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestAreaUI : MonoBehaviour
{

    public static RestAreaUI Instance;
    [SerializeField] GameObject _RestAreaContainer;
    [SerializeField]
    RestArea _restArea;

    [SerializeField] Image _topOptionLeftImage;
    [SerializeField] Image _topOptionRightImage;

    [SerializeField] TextMeshProUGUI _topOptionLeftText;
    [SerializeField] TextMeshProUGUI _topOptionRightText;



    [SerializeField] Image _bottomOptionLeftImage;
    [SerializeField] Image _bottomOptionRightImage;

    [SerializeField] TextMeshProUGUI _bottomOptionLeftText;
    [SerializeField] TextMeshProUGUI _bottomOptionRightText;

    const string HealOptionText = "Heal ";
    const string StaminaShardText = "Add StaminaShard";
    const string MaxHpAdditonText = " Max HP";

    [SerializeField] Color _optionTakenColor;
    [SerializeField] Color _optionNotTakeColor;
    [SerializeField] Color _resetColor;

    bool _firstOptionFlag;
    bool _secondOptionFlag;


    private void Start()
    {
        Instance = this;
        if (_restArea == null)
            throw new System.Exception("Rest Area Data is null!");


        ResetRestArea();

    }

    private void ResetRestArea()
    {
        ResetTopQuestions();
        ResetBottomQuestions();
    }

    private void ResetBottomQuestions()
    {
        if (_bottomOptionLeftText.gameObject.activeSelf == false)
            _bottomOptionLeftText.gameObject.SetActive(true);
        if (_bottomOptionRightText.gameObject.activeSelf == false)
            _bottomOptionRightText.gameObject.SetActive(true);




        _bottomOptionLeftText.text =string.Concat("WIP"); 
        _bottomOptionRightText.text = string.Concat(StaminaShardText);

        _bottomOptionLeftImage.color = _resetColor;
        _bottomOptionRightImage.color = _resetColor;
    }

    private void ResetTopQuestions()
    {

        if (_topOptionLeftText.gameObject.activeSelf == false)
            _topOptionLeftText.gameObject.SetActive(true);
        if (_topOptionRightText.gameObject.activeSelf == false)
            _topOptionRightText.gameObject.SetActive(true);

        _topOptionLeftText.text = string.Concat(HealOptionText, _restArea.HPHeal);
        _topOptionRightText.text = string.Concat(_restArea.MaxHPAddition, MaxHpAdditonText);
        _topOptionLeftImage.color = _resetColor;
        _topOptionRightImage.color = _resetColor;



    }

    private void SetActiveRestAreaContainer(bool state) => _RestAreaContainer.SetActive(state);

    public void EnterRestArea()
    {
        ResetRestArea();
        SetActiveRestAreaContainer(true);
        _secondOptionFlag = true;
        _firstOptionFlag = true;
    }

    public void ExitRestArea()
    {
        Map.MapView.Instance.SetAttainableNodes();
        SetActiveRestAreaContainer(false);
    }

    public void AddMaxHealth()
    {

        if (_firstOptionFlag == false)
            return;
        _firstOptionFlag = false;
        _restArea.AddMaxHealth();
        _topOptionRightImage.color = _optionTakenColor;

        _topOptionLeftImage.color = _optionNotTakeColor;
        _topOptionLeftText.gameObject.SetActive(false);
        CheckForBothDecisionsAssigned();
    }
    public void Heal()
    {
        if (_firstOptionFlag == false)
            return;
        _firstOptionFlag = false;
        _restArea.AddHealth();

        _topOptionLeftImage.color = _optionTakenColor;

        _topOptionRightImage.color = _optionNotTakeColor;
        _topOptionRightText.gameObject.SetActive(false);
        CheckForBothDecisionsAssigned();
    }
    public void StamiaShard() 
    {
        if (_secondOptionFlag == false)
            return;
        _secondOptionFlag = false;

        _restArea.AddStaminaShard();


        _bottomOptionRightImage.color = _optionTakenColor;

        _bottomOptionLeftImage.color = _optionNotTakeColor;
        _bottomOptionLeftText.gameObject.SetActive(false);

        CheckForBothDecisionsAssigned();
    }
    public void RemoveCard()
    {
        if (_secondOptionFlag == false)
            return;
        _secondOptionFlag = false;

        _bottomOptionLeftImage.color = _optionTakenColor;

        _bottomOptionRightImage.color = _optionNotTakeColor;
        _bottomOptionRightText.gameObject.SetActive(false);


       
        CheckForBothDecisionsAssigned();
    }

    private void CheckForBothDecisionsAssigned()
    {
        if (!_secondOptionFlag && !_firstOptionFlag)
            ExitRestArea();
    }

   
}
[System.Serializable]
public class RestArea
{
    [SerializeField] byte _staminaShardAmount = 1;
    [SerializeField] ushort HPPrecentage;
    [SerializeField] ushort _MaxHPAddition;

    public byte StaminaShard => _staminaShardAmount;
    public ushort HPHeal =>(ushort) (Battles.BattleData.Player.CharacterData.CharacterStats.MaxHealth * HPPrecentage / 100);
    public ushort MaxHPAddition => _MaxHPAddition;
    public void AddHealth()
    {
        var stats = Battles.BattleData.Player.CharacterData.CharacterStats;
        int currentHP = stats.Health;
        int maxHealth = stats.MaxHealth;
        int amount = HPHeal;
        if (currentHP == maxHealth)
            return;
        else if (currentHP + amount >= maxHealth)
            currentHP = maxHealth;
        else
         currentHP += amount;

        Battles.BattleData.Player.CharacterData.CharacterStats.Health = currentHP;
        UpperInfoUIHandler.Instance.UpdateUpperInfoHandler(ref Battles.BattleData.Player.CharacterData.CharacterStats);
    }

    public void AddMaxHealth()
    {
        var stats = Battles.BattleData.Player.CharacterData.CharacterStats;
        int maxHealth = stats.MaxHealth;
        int currentHP = stats.Health;

        maxHealth += _MaxHPAddition;
        currentHP += _MaxHPAddition;

        Battles.BattleData.Player.CharacterData.CharacterStats.MaxHealth = maxHealth;
        Battles.BattleData.Player.CharacterData.CharacterStats.Health = currentHP;
        UpperInfoUIHandler.Instance.UpdateUpperInfoHandler(ref Battles.BattleData.Player.CharacterData.CharacterStats);
    }

    internal void AddStaminaShard()
    {
      Battles.BattleData.Player.CharacterData.CharacterStats.StaminaShard += _staminaShardAmount;
        Debug.Log("Added Stamina Shard From Rest Area\nCurrent Amount: " + Battles.BattleData.Player.CharacterData.CharacterStats.StaminaShard);
    }
}