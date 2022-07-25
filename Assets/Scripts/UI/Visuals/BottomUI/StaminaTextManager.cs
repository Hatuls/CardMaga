using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using Characters.Stats;

public class StaminaTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentStamimaText;
    [SerializeField] private TextMeshProUGUI _maxStamimaText;
    [SerializeField] private StaminaTransition _staminaTransition;

    private int _currentStamina;

    private void Start()
    {
        StaminaHandler.StaminaTextManager = this;
        ResetStamina();
    }

    public void UpdateCurrentStamina(int stamina)
    {
        CheckCurrentStaminaForAnimation(stamina);
        _currentStamimaText.text = (_currentStamina).ToString();
    }

    public void UpdateMaxStamina(int stamina)
    {
        CheckMaxStaminaForAnimation(stamina);
        _maxStamimaText.text = (stamina).ToString();
    }

    private void CheckCurrentStaminaForAnimation(int _newStamina)
    {
        if(_newStamina != _currentStamina)
        {
            if (_newStamina < _currentStamina)
                _staminaTransition.ReduceAnimation(_staminaTransition.CurrentStaminaRectTransform);

            else if (_newStamina > _currentStamina)
                _staminaTransition.GainAnimation(_staminaTransition.CurrentStaminaRectTransform);

            _currentStamina = _newStamina;
        }
    }

    private void CheckMaxStaminaForAnimation(int _newStamina)
    {
        if(_newStamina != _currentStamina)
        {
            if (_newStamina < _currentStamina)
                _staminaTransition.ReduceAnimation(_staminaTransition.MaxStaminaRectTransform);

            else if (_newStamina > _currentStamina)
                _staminaTransition.GainAnimation(_staminaTransition.MaxStaminaRectTransform);
        }

        _currentStamina = _newStamina;
    }

    private void ResetStamina()
    {
        _currentStamina = 0;
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

#if UNITY_EDITOR
    /// <summary>
    /// Test for starting the animation if wanted (reduce) and changing the number
    /// </summary>
    /// <param name="num"></param>
    //Not working yet!!
    public void TestReduceStamina(RectTransform rectTransform, float timing, TextMeshProUGUI _staminaText, Color _newColor, int _newStamina)
    {
        StartCoroutine(WaitForColorChangeTiming( rectTransform,  timing,  _staminaText,  _newColor,  _newStamina));
    }
#endif
}
