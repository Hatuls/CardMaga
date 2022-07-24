using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class StaminaTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentStamima;
    [SerializeField] private TextMeshProUGUI _maxStamima;

    public void UpdateCurrentStamina(int stamina)
    {
        _currentStamima.text = (stamina).ToString();
    }

    public void UpdateMaxStamina(int stamina)
    {
        _maxStamima.text = (stamina).ToString();
    }
}
