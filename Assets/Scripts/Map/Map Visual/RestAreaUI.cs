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
      _topOptionLeftText.text = string.Concat(HealOptionText, _restArea.HPHeal);
        _topOptionRightText.text = string.Concat(_restArea.MaxHPAddition, MaxHpAdditonText);
        _topOptionLeftImage.color = _resetColor;
        _topOptionRightImage.color = _resetColor;

        if (_topOptionLeftText.gameObject.activeSelf == false)
            _topOptionLeftText.gameObject.SetActive(true);
        if (_topOptionRightText.gameObject.activeSelf == false)
            _topOptionRightText.gameObject.SetActive(true);
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
        _firstOptionFlag = true;
        _restArea.AddMaxHealth();
        _topOptionRightImage.color = _optionTakenColor;

        _topOptionLeftImage.color = _optionNotTakeColor;
        _topOptionLeftText.gameObject.SetActive(false);
    }
    public void Heal()
    {
        if (_firstOptionFlag == false)
            return;
        _firstOptionFlag = true;
        _restArea.AddHealth();

        _topOptionLeftImage.color = _optionTakenColor;

        _topOptionRightImage.color = _optionNotTakeColor;
        _topOptionRightImage.gameObject.SetActive(false);

    }
    public void StamiaShard() 
    {
        if (_secondOptionFlag == false)
            return;
        _secondOptionFlag = true;
    }
    public void RemoveCard() 
    {
        if (_secondOptionFlag == false)
            return;
        _secondOptionFlag = true;
    }
}
[System.Serializable]
public class RestArea
{
    [SerializeField] ushort _HPHeal;
    [SerializeField] ushort _MaxHPAddition;
    public ushort HPHeal => _HPHeal;
    public ushort MaxHPAddition => _MaxHPAddition;
    public void AddHealth()
    {
        var stats = Battles.BattleData.Player.CharacterData.CharacterStats;
        int currentHP = stats.Health;
        int maxHealth = stats.MaxHealth;

        if (currentHP == maxHealth)
            return;
        else if (currentHP + _HPHeal >= maxHealth)
            currentHP = maxHealth;
        else
         currentHP += _HPHeal;

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




}