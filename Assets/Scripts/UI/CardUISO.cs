using UnityEngine;

using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "CardUI_Settings", menuName = "ScriptableObjects/UI Settings")]
public class CardUISO : ScriptableObject
{
    #region Fields
    [Tooltip("The delay of card following the touch")]
    [SerializeField] float _cardFollowTheTouchDelay;  
    
    [Tooltip("The time of card going/getting to deck")]
    [SerializeField] float _cardGoToDeckDelay;    
    
    [Tooltip("The delay of card changing scale")]
    [SerializeField] float _cardScalingDelay;

    [Tooltip("The scale of the card ui when Zoomed")]
    [SerializeField] float _zoomedScale;

    [Tooltip("The default scale of the card")]
    [SerializeField] float _cardScale;
    
    [Tooltip("The delay of the card ui when is released and need to return to his previous place")]
    [SerializeField] float _returnSpeedDelay;

    [Tooltip("The maximum amount of cards in hand")]
    [SerializeField] int _amountOfCardsUIInHand;
    
    [Tooltip("The amount of space between cards in hand")]
    [SerializeField] float _amountOfSpaceBetweenCards;

    [Tooltip("The Delay Between Each Card Discarded")]
    [SerializeField] float _delayBetweenCardsIsDiscarded;

    [Tooltip("The delay of the card ui when is discarded toward the discard button on the end of the turn")]
    [SerializeField] float _timerForCardGoingToDiscardPile;

    #endregion


    #region  Removal Card
    [TitleGroup("Card UI", "", TitleAlignments.Centered, BoldTitle = true)]

    #region Removal Scale
    [TabGroup("Card UI/Removal", "Removal")]
    [Range(0f,5f)]
    [SerializeField] float _scaleSizeForRemoval;
    [TabGroup("Card UI/Removal", "Removal")]
    [Range(0f,2f)]
    [SerializeField] float _removalTime;
    [TabGroup("Card UI/Removal", "Removal")]
    [SerializeField] LeanTweenType _scaleRemovalTweenType;
    #endregion

    #region Alpha
    [TabGroup("Card UI/Removal", "Alpha")]
    [Range(0,1)]
    [SerializeField] float _alphaAmountForRemoval;
    [TabGroup("Card UI/Removal", "Alpha")]
    [SerializeField] float _alphaRemovalTime;
    [TabGroup("Card UI/Removal", "Alpha")]
    [SerializeField] LeanTweenType _alphaTweenType;
    #endregion

    #region Removal On X Axis
    [TabGroup("Card UI/Removal", "Move On X")]
    [Range(0,2)]
    [SerializeField] float _removalTransitionXTime;
    [TabGroup("Card UI/Removal", "Move On X")]
    [SerializeField] LeanTweenType _removalMoveOnXLeanTweenType;
    #endregion

    #region Removal On Y Axis
    [TabGroup("Card UI/Removal", "Move On Y")]
    [Range(0,2)]
    [SerializeField] float _removalTransitionYTime;
    [TabGroup("Card UI/Removal", "Move On Y")]
    [SerializeField] LeanTweenType _removalMoveOnYLeanTweenType;
    #endregion
    [Range(0,1)]
    [SerializeField] float _delayTillStartMovement;
    #endregion
    #region Properties
    public ref float GetDelayTillStartMovement => ref _delayTillStartMovement;
    public ref LeanTweenType GetMoveOnYLeanTween => ref _removalMoveOnYLeanTweenType;
    public ref float GetRemovalTransitionYTime => ref _removalTransitionYTime;
    public ref LeanTweenType GetMoveOnXLeanTween => ref _removalMoveOnXLeanTweenType;
    public ref float GetRemovalTransitionXTime => ref _removalTransitionXTime;
    public ref float GetAlphaRemovalAmount => ref _alphaAmountForRemoval;
    public ref float GetAlphaRemovalTime => ref _alphaRemovalTime;
    public ref LeanTweenType GetAlphaLeanTween => ref _alphaTweenType;
    public ref float GetRemovalTimeForRemoval => ref _removalTime;
    public ref LeanTweenType GetScaleRemovalLeanTweenType => ref _scaleRemovalTweenType;
    public ref float GetScaleSizeForRemoval => ref _scaleSizeForRemoval;
    public ref float GetTimerForCardGoingToDiscardPile => ref _timerForCardGoingToDiscardPile;
    public ref float GetDelayBetweenRemovalOfEachCard => ref _delayBetweenCardsIsDiscarded;
    public ref float GetCardReturnSpeedDelay => ref _returnSpeedDelay;
    public ref float GetSpaceBetweenCards => ref _amountOfSpaceBetweenCards;
    public ref int GetAmountOfCardsUIInHand => ref _amountOfCardsUIInHand;
    public ref float GetCardFollowDelay => ref _cardFollowTheTouchDelay;
    public ref float GetCardMoveToDeckDelay => ref _cardGoToDeckDelay;
    public ref float GetCardScaleDelay => ref _cardScalingDelay;
    public Vector3 GetCardUIZoomedScale => Vector3.one * _zoomedScale;
    public Vector3 GetCardDefaultScale => Vector3.one * _cardScale;
    #endregion
}

