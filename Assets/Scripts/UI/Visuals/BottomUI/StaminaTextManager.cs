using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using Characters.Stats;

public class StaminaTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentStamima;
    [SerializeField] private TextMeshProUGUI _maxStamima;
    [SerializeField] private StaminaTransition _staminaTransition;

    private void Start()
    {
        StaminaHandler.StaminaTextManager = this;
    }

    public void UpdateCurrentStamina(int stamina)
    {
        _currentStamima.text = (stamina).ToString();
    }

    public void UpdateMaxStamina(int stamina)
    {
        _maxStamima.text = (stamina).ToString();
    }

#if UNITY_EDITOR
    /// <summary>
    /// Test for starting the animation if wanted (reduce) and changing the number
    /// </summary>
    /// <param name="num"></param>
    public void TestReduceStamina(int num)
    {
        _staminaTransition.ReduceAnimation(_staminaTransition.CurrentStaminaRectTransform);
        _currentStamima.text = (num).ToString();
    }
#endif
}
