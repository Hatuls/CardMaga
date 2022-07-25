using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;

public class StaminaTransition : MonoBehaviour
{
    [Header("RectTransforms")]
    //The objects that will be effected by the animations
    [SerializeField] public RectTransform MaxStaminaRectTransform;
    [SerializeField] public RectTransform CurrentStaminaRectTransform;
    [SerializeField] public RectTransform StaminaIconsRectTransform;

    [Title("TransitionPackSO","Texts: ")]
    [SerializeField] public  TransitionPackSO _gainStaminaTextTransition;
    [SerializeField] private TransitionPackSO _reduceStaminaTextTransition;


    [Title("Icon", "Current Stamina: ")]
    [SerializeField] public TransitionPackSO _gainStaminaIconTransition;
    [SerializeField] public TransitionPackSO _reduceStaminaIconTransition;
    [Title("Icon", "Max Stamina: ")]
    [SerializeField] public TransitionPackSO _gainMaxStaminaIconTransition;
    [SerializeField] public TransitionPackSO _reduceMaxStaminaIconTransition;


    /// <summary>
    /// Using the _reduceStamina TransitionPackSO. The only thing you need to choose is the RectTransform object- the object you want to be effected by the animation(TransitionPackSO)
    /// </summary>
    /// <param name="_staminaRectTransform"></param>
    public void ReduceCurrentStaminaAnimation()
    {
        CurrentStaminaRectTransform.Transition(_reduceStaminaTextTransition);
        StaminaIconsRectTransform.Transition(_reduceStaminaIconTransition);
    }

    internal void ReduceMaxStaminaAnimation()
    {
        MaxStaminaRectTransform.Transition(_reduceStaminaTextTransition);
        StaminaIconsRectTransform.Transition(_reduceStaminaIconTransition);
    }

    internal void GainMaxStaminaAnimation()
    {
        CurrentStaminaRectTransform.Transition(_gainStaminaTextTransition);
        StaminaIconsRectTransform.Transition(_gainMaxStaminaIconTransition);
    }


    /// <summary>
    /// Using the _gainStamina TransitionPackSO. The only thing you need to choose is the RectTransform object- the object you want to be effected by the animation(TransitionPackSO)"
    /// </summary>
    /// <param name="_staminaRectTransform"></param>
    public void GainCurrentStaminaAnimation()
    {
        CurrentStaminaRectTransform.Transition(_gainStaminaTextTransition);
        StaminaIconsRectTransform.Transition(_gainStaminaIconTransition);
    }

    /// <summary>
    /// Send the RectTransitionManager, RectTransform and TransitionPackSO for transforming the object you want
    /// </summary>
    /// <param name="_rectTransitionManager"></param>
    /// <param name="_staminaRectTransform"></param>
    /// <param name="_transitionPackSO"></param>
    public void Transition(RectTransform _rectTransitionManager, RectTransform _staminaRectTransform, TransitionPackSO _transitionPackSO)
    {
        _rectTransitionManager.Move(_staminaRectTransform, _transitionPackSO);
    }
}
