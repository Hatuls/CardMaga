using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class StaminaTransition : MonoBehaviour
{

    [Header("Fields")]
    [SerializeField] private TransitionPackSO _gainStamina;
    [SerializeField] private TransitionPackSO _reduceStamina;

    //Transform of the UI.
    [SerializeField] private RectTransform _maxStaminaRectTransform;
    [SerializeField] private RectTransform _currentStaminaRectTransform;
    [SerializeField] private RectTransform _staminaIconsRectTransform;

    //Scrip of thransitions. Recive Transision pack SO
    private RectTransitionManager _reduceStaminaTransitionManager;
    private RectTransitionManager _gainStaminaTransitionManager;

    void Start()
    {
        _reduceStaminaTransitionManager = new RectTransitionManager(_currentStaminaRectTransform);
        _gainStaminaTransitionManager = new RectTransitionManager(_currentStaminaRectTransform);

        if (_gainStamina != null)
        {
            _gainStaminaTransitionManager = new RectTransitionManager(_currentStaminaRectTransform);
        }

        if (_reduceStamina != null)
        {
            _reduceStaminaTransitionManager = new RectTransitionManager(_currentStaminaRectTransform);
        }
    }

    [Button]
    public void ReduceAnimation(RectTransform _staminaRectTransform)
    {
        _staminaRectTransform.Scale(_reduceStamina.ScaleMultiplier, _reduceStamina);
    }

    [Button]
    public void GainAnimation(RectTransform _staminaRectTransform)
    {
        _staminaRectTransform.Scale(_gainStamina.ScaleMultiplier, _gainStamina);
    }

    [Button]
    public void Transition(RectTransform _rectTransitionManager, RectTransform _staminaRectTransform, TransitionPackSO _transitionPackSO)
    {
        _rectTransitionManager.Move(_staminaRectTransform, _transitionPackSO);
    }
}
