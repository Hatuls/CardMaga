using CardMaga.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class StaminaTransition : MonoBehaviour
{
    [SerializeField] HandUI _handUI;

    [Header("RectTransforms")]
    //The objects that will be effected by the animations
    [SerializeField] public RectTransform MaxStaminaRectTransform;
    [SerializeField] public RectTransform CurrentStaminaRectTransform;
    [SerializeField] public RectTransform StaminaIconsRectTransform;

    [Header("Text Components")]
    [SerializeField] TextMeshProUGUI _currentStaminaText;
    [SerializeField] TextMeshProUGUI _maxStaminaText;

    [Title("TransitionPackSO", "Texts: ")]
    [SerializeField] public TransitionPackSO _gainStaminaTextTransition;
    [SerializeField] private TransitionPackSO _reduceStaminaTextTransition;

    [Title("Icon", "Current Stamina: ")]
    [SerializeField] public TransitionPackSO _gainStaminaIconTransition;
    [SerializeField] public TransitionPackSO _reduceStaminaIconTransition;

    [Title("Icon", "Max Stamina: ")]
    [SerializeField] public TransitionPackSO _gainMaxStaminaIconTransition;
    [SerializeField] public TransitionPackSO _reduceMaxStaminaIconTransition;

    [Title("Icon", "No Stamina: ")]
    [SerializeField] private TransitionPackSO _noStaminaIconTransition;

    [Title("Text", "No Stamina: ")]
    [SerializeField] private TransitionPackSO _noStaminaTextTransition;
    [SerializeField] Color _noStaminaColor;
    [SerializeField] Color _hasStaminaColor;

    private float _startStaminaTextScale;
    private float _startMaxStaminaText;
    private float _startIconStaminaIcon;

    Sequence _noStaminaSequence;
    Sequence _reduceAndGainStaminaSequence;
    Sequence _reduceAndGainMaxStaminaSequence;

    public void Awake()
    {
        _handUI.OnCardExecutionFailed += NoStaminaAnimation;
        _startStaminaTextScale = CurrentStaminaRectTransform.localScale.x;
        _startMaxStaminaText = MaxStaminaRectTransform.localScale.x;
        _startIconStaminaIcon = StaminaIconsRectTransform.localScale.x;
        _currentStaminaText.color = _hasStaminaColor;
        _maxStaminaText.color = _hasStaminaColor;
    }

    /// <summary>
    /// Using the _gainStamina TransitionPackSO. The only thing you need to choose is the RectTransform object- the object you want to be effected by the animation(TransitionPackSO)"
    /// </summary>
    /// <param name="_staminaRectTransform"></param>
    public void GainCurrentStaminaAnimation()
    {
        if (_reduceAndGainStaminaSequence != null)
            _reduceAndGainStaminaSequence.Kill(true);

        _reduceAndGainStaminaSequence = CurrentStaminaRectTransform.Transition(_gainStaminaTextTransition).Join(
            StaminaIconsRectTransform.Transition(_gainStaminaIconTransition)).OnComplete(ResetScales);
    }

    /// <summary>
    /// Using the _reduceStamina TransitionPackSO. The only thing you need to choose is the RectTransform object- the object you want to be effected by the animation(TransitionPackSO)
    /// </summary>
    /// <param name="_staminaRectTransform"></param>
    public void ReduceCurrentStaminaAnimation()
    {
        if (_reduceAndGainStaminaSequence != null)
            _reduceAndGainStaminaSequence.Kill(true);

        _reduceAndGainStaminaSequence = CurrentStaminaRectTransform.Transition(_reduceStaminaTextTransition).Join(
            StaminaIconsRectTransform.Transition(_reduceStaminaIconTransition)).OnComplete(ResetScales);
    }

    internal void ReduceMaxStaminaAnimation()
    {
        if (_reduceAndGainMaxStaminaSequence != null)
            _reduceAndGainMaxStaminaSequence.Kill(true);

        _reduceAndGainMaxStaminaSequence = MaxStaminaRectTransform.Transition(_reduceStaminaTextTransition).Join(
            StaminaIconsRectTransform.Transition(_reduceStaminaIconTransition)).OnComplete(ResetScales);
    }

    internal void GainMaxStaminaAnimation()
    {
        if (_reduceAndGainMaxStaminaSequence != null)
            _reduceAndGainMaxStaminaSequence.Kill(true);

        _reduceAndGainMaxStaminaSequence = CurrentStaminaRectTransform.Transition(_gainStaminaTextTransition).Join(
            StaminaIconsRectTransform.Transition(_gainMaxStaminaIconTransition)).OnComplete(ResetScales);
    }

    internal void NoStaminaAnimation()
    {
        if (_noStaminaSequence != null && _noStaminaSequence.IsPlaying())
            return;
        
        _noStaminaSequence = CurrentStaminaRectTransform.Transition(_noStaminaTextTransition).Join(
        StaminaIconsRectTransform.Transition(_noStaminaIconTransition)).Join(_currentStaminaText.DOColor(_noStaminaColor,0)).OnComplete(ResetScales);
    }

    private void ResetScales()
    {
        MaxStaminaRectTransform.localScale = _startStaminaTextScale * Vector3.one;
        CurrentStaminaRectTransform.localScale = _startMaxStaminaText * Vector3.one;
        StaminaIconsRectTransform.localScale = _startIconStaminaIcon * Vector3.one;
        _currentStaminaText.color = _hasStaminaColor;
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
