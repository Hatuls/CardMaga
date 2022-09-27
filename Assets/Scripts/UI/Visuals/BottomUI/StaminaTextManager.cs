
using Battle;
using Characters.Stats;
using CardMaga.Sequence;
using ReiTools.TokenMachine;
using System.Collections;
using TMPro;
using UnityEngine;

public class StaminaTextManager : MonoBehaviour, ISequenceOperation<IBattleManager>
{
    [SerializeField] private TextMeshProUGUI _currentStamimaText;
    [SerializeField] private TextMeshProUGUI _maxStamimaText;
    [SerializeField] private StaminaTransition _staminaTransition;
    private StaminaHandler _playerStaminaHandler;
    private int _currentStamina = -1;

    public int Priority => 0;

    private void Awake()
    {

        BattleManager.Register(this, OrderType.Before);

    }

    public void UpdateCurrentStamina(int stamina)
    {
        if (stamina != _currentStamina)
        {
            CheckCurrentStaminaForAnimation(stamina);
            _currentStamimaText.text = (stamina).ToString();
            _currentStamina = stamina;
        }

    }

    public void UpdateMaxStamina(int stamina)
    {
        CheckMaxStaminaForAnimation(stamina);
        _maxStamimaText.text = (stamina).ToString();
    }

    private void CheckCurrentStaminaForAnimation(int _newStamina)
    {
        if (_newStamina < _currentStamina)
            _staminaTransition.ReduceCurrentStaminaAnimation();

        else if (_newStamina > _currentStamina)
            _staminaTransition.GainCurrentStaminaAnimation();
    }

    private void CheckMaxStaminaForAnimation(int _newStamina)
    {
   
            if (_newStamina < _currentStamina)
                _staminaTransition.ReduceMaxStaminaAnimation();

            else if (_newStamina > _currentStamina)
                _staminaTransition.GainMaxStaminaAnimation();
 
    }

    private void ResetStamina()
    {
        UpdateCurrentStamina(0);
    }

    private void ChangeTextColor(TextMeshProUGUI _staminaText, Color _newColor)
    {
        _staminaText.color = _newColor;
    }

    //Not working yet!!
    IEnumerator WaitForColorChangeTiming(RectTransform rectTransform, float timing, TextMeshProUGUI _staminaText, Color _newColor, int _newStamina)
    {
        CheckCurrentStaminaForAnimation(_newStamina);
        while (rectTransform.localScale.x < timing)
        {
            Debug.Log(rectTransform.localScale.x);
            yield return null;
        }
        ChangeTextColor(_staminaText, _newColor);
        _currentStamimaText.text = (_currentStamina).ToString();
    }
    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
    {
        _playerStaminaHandler = data.PlayersManager.GetCharacter(true).StaminaHandler;
        _playerStaminaHandler.OnStaminaValueChanged += UpdateCurrentStamina;
        ResetStamina();
       // data.PlayersManager.GetCharacter(true).
    }

    private void OnDestroy()
    {
        if (_playerStaminaHandler != null)
            _playerStaminaHandler.OnStaminaValueChanged -= UpdateCurrentStamina;
    }
#if UNITY_EDITOR
    /// <summary>
    /// Test for starting the animation if wanted (reduce) and changing the number
    /// </summary>
    /// <param name="num"></param>
    //Not working yet!!
    public void TestReduceStamina(RectTransform rectTransform, float timing, TextMeshProUGUI _staminaText, Color _newColor, int _newStamina)
    {
        StartCoroutine(WaitForColorChangeTiming(rectTransform, timing, _staminaText, _newColor, _newStamina));
    }

 
#endif
}
