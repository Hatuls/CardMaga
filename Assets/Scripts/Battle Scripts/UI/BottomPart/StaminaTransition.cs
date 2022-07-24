using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class StaminaTransition : MonoBehaviour
{

    [Header("TransitionPackSO")]
    [SerializeField] public TransitionPackSO _gainStamina;
    [SerializeField] private TransitionPackSO _reduceStamina;

    [Header("RectTransforms")]
    //The objects that will be effected by the animations
    [SerializeField] public RectTransform MaxStaminaRectTransform;
    [SerializeField] public RectTransform CurrentStaminaRectTransform;
    [SerializeField] public RectTransform StaminaIconsRectTransform;

    [Header("RectTransitionManager")]
    private RectTransitionManager _reduceStaminaTransitionManager;
    private RectTransitionManager _gainStaminaTransitionManager;

    void Start()
    {
        _reduceStaminaTransitionManager = new RectTransitionManager(CurrentStaminaRectTransform);
        _gainStaminaTransitionManager = new RectTransitionManager(CurrentStaminaRectTransform);

        if (_gainStamina != null)
        {
            _gainStaminaTransitionManager = new RectTransitionManager(CurrentStaminaRectTransform);
        }

        if (_reduceStamina != null)
        {
            _reduceStaminaTransitionManager = new RectTransitionManager(CurrentStaminaRectTransform);
        }
    }

    /// <summary>
    /// Using the _reduceStamina TransitionPackSO. The only thing you need to choose is the RectTransform object- the object you want to be effected by the animation(TransitionPackSO)
    /// </summary>
    /// <param name="_staminaRectTransform"></param>
    public void ReduceAnimation(RectTransform _staminaRectTransform)
    {
        _staminaRectTransform.Scale(_reduceStamina.ScaleMultiplier, _reduceStamina.Scale);
    }


    /// <summary>
    /// Using the _gainStamina TransitionPackSO. The only thing you need to choose is the RectTransform object- the object you want to be effected by the animation(TransitionPackSO)"
    /// </summary>
    /// <param name="_staminaRectTransform"></param>
    public void GainAnimation(RectTransform _staminaRectTransform)
    {
        _staminaRectTransform.Scale(_gainStamina.ScaleMultiplier, _gainStamina.Scale);
    }

    /// <summary>
    /// Send the RectTransitionManager, RectTransform and TransitionPackSO for transforming the object you want
    /// </summary>
    /// <param name="_rectTransitionManager"></param>
    /// <param name="_staminaRectTransform"></param>
    /// <param name="_transitionPackSO"></param>
    public void Transition(RectTransitionManager _rectTransitionManager, RectTransform _staminaRectTransform, TransitionPackSO _transitionPackSO)
    {
        _rectTransitionManager.Move(_staminaRectTransform, _transitionPackSO);
    }
}
