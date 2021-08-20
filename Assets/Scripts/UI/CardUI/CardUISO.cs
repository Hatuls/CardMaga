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

    [SerializeField] float _discardDefaultTime;

    [SerializeField] int _cardAmountOffset;
    [SerializeField] float _scaleFactorInSpaceInHand;

    [Range(0,0.1f)]
    [SerializeField] float _cardAlignmentInHandHeight;
    [Range(0,90f)]
    [SerializeField] float _degreePerCard;
    [SerializeField] float _yFactorPerOffsetCardInHand;
    #endregion


    #region  Translitions
    [TitleGroup("Card UI", "CardUI", TitleAlignments.Centered, BoldTitle = true)]

    #region Discard

    #region Scale

    [TabGroup("Card UI/CardUI", "Discard Cards")]
    [TabGroup("Card UI/CardUI/Discard Cards/Params", "Scale")]
    [Range(0f, 5f)]
    [SerializeField] float _discardEndScaleSize;

    [TabGroup("Card UI/CardUI/Discard Cards/Params", "Scale")]
    [Range(0f, 2f)]
    [SerializeField] float _scaleDiscardTime;

    [TabGroup("Card UI/CardUI/Discard Cards/Params", "Scale")]
    [SerializeField] LeanTweenType _scaleDiscardTweenType;
    #endregion


    #region Alpha   
    [TabGroup("Card UI/CardUI/Discard Cards/Params", "Alpha")]
    [Range(0, 1)]
    [SerializeField] float _endDiscardAlphaAmount;

    [TabGroup("Card UI/CardUI/Discard Cards/Params", "Alpha")]
    [SerializeField] float _alphaDiscardTime;

    [TabGroup("Card UI/CardUI/Discard Cards/Params", "Alpha")]
    [SerializeField] LeanTweenType _alphaDiscardTweenType;
    #endregion

    #region Draw On X Axis
    [TabGroup("Card UI/CardUI/Discard Cards/Params", "Move On X")]
    [Range(0, 2)]
    [SerializeField] float _discardTransitionXTime;

    [TabGroup("Card UI/CardUI/Discard Cards/Params", "Move On X")]
    [SerializeField] LeanTweenType _discardMoveOnXLeanTweenType;

    #endregion

    #region Draw On Y Axis
    [TabGroup("Card UI/CardUI/Discard Cards/Params", "Move On Y")]
    [Range(0, 2)]
    [SerializeField] float _discardTransitionYTime;

    [TabGroup("Card UI/CardUI/Discard Cards/Params", "Move On Y")]
    [SerializeField] LeanTweenType _discardMoveOnYLeanTweenType;

    [TabGroup("Card UI/CardUI", "Discard Cards")]
    [Range(0, 1)]
    [Tooltip("The Delay Between Each Card Discarded")]
    [SerializeField] float _delayBetweenCardsIsDiscarded; 
    
    [TabGroup("Card UI/CardUI", "Discard Cards")]
    [Range(0, 1)]
    [Tooltip("The Delay between alpha & scale to moving Each Card Discarded")]
    [SerializeField] float _delayDiscardBetweenVisualAndTranslation;

    #endregion
    public ref float DiscardEndScaleSize => ref _discardEndScaleSize;
    public ref float ScaleDiscardTime => ref _scaleDiscardTime;
    public ref float EndDiscardAlphaAmount => ref _endDiscardAlphaAmount;
    public ref LeanTweenType ScaleDiscardTweenType => ref _scaleDiscardTweenType;
    public ref float AlphaDiscardTime => ref _alphaDiscardTime;
    public ref float DiscardTransitionXTime => ref _discardTransitionXTime;
    public ref LeanTweenType AlphaDiscardTweenType => ref _alphaDiscardTweenType;
    public ref LeanTweenType DiscardMoveOnXLeanTweenType => ref _discardMoveOnXLeanTweenType;
    public ref LeanTweenType DiscardMoveOnYLeanTweenType => ref _discardMoveOnYLeanTweenType;
    public ref float DiscardTransitionYTime => ref _discardTransitionYTime;
    public ref float DelayBetweenCardsIsDiscarded => ref _delayBetweenCardsIsDiscarded;
    public ref float DelayBetweenVisualAndMoving => ref _delayDiscardBetweenVisualAndTranslation;

    #endregion

    #region Draw
    #region Scale

    [TabGroup("Card UI/CardUI", "Draw Cards")]
    [TabGroup("Card UI/CardUI/Draw Cards/Params", "Scale")]
    [Range(0f, 5f)]
    [SerializeField] float _startScaleSize;

   [TabGroup("Card UI/CardUI/Draw Cards/Params", "Scale")]
        [Range(0f, 2f)]
    [SerializeField] float _scaleDrawTime;

    [TabGroup("Card UI/CardUI/Draw Cards/Params", "Scale")]
    [SerializeField] LeanTweenType _scaleDrawTweenType;
    #endregion


    #region Alpha   
    [TabGroup("Card UI/CardUI/Draw Cards/Params", "Alpha")]
    [Range(0, 1)]
    [SerializeField] float _startAlphaAmount;

    [TabGroup("Card UI/CardUI/Draw Cards/Params", "Alpha")]
    [SerializeField] float _alphaDrawTime;

    [TabGroup("Card UI/CardUI/Draw Cards/Params", "Alpha")]
    [SerializeField] LeanTweenType _alphaDrawTweenType;
    #endregion

    #region Draw On X Axis
    [TabGroup("Card UI/CardUI/Draw Cards/Params", "Move On X")]
    [Range(0, 2)]
    [SerializeField] float _drawTransitionXTime;

    [TabGroup("Card UI/CardUI/Draw Cards/Params", "Move On X")]
    [SerializeField] LeanTweenType _drawMoveOnXLeanTweenType;

    #endregion

    #region Draw On Y Axis
   [TabGroup("Card UI/CardUI/Draw Cards/Params", "Move On Y")]
    [Range(0, 2)]
    [SerializeField] float _drawTransitionYTime;

    [TabGroup("Card UI/CardUI/Draw Cards/Params", "Move On Y")]
    [SerializeField] LeanTweenType _drawMoveOnYLeanTweenType;

    [TabGroup("Card UI/CardUI","Draw Cards")]
    [Range(0, 1)]
    [SerializeField] float _delayTillDrawNextCard;

    #endregion
    public ref float DelayTillDrawNextCard => ref _delayTillDrawNextCard;
    public ref float StartScaleSize => ref _startScaleSize;
    public ref float ScaleDrawTime => ref _scaleDrawTime;
    public ref LeanTweenType ScaleDrawTweenType => ref _scaleDrawTweenType;
    public ref float StartAlphaAmount => ref _startAlphaAmount;
    public ref float AlphaDrawTime => ref _alphaDrawTime;
    public ref LeanTweenType AlphaDrawTweenType => ref _alphaDrawTweenType;
    public ref LeanTweenType DrawMoveOnXLeanTweenType => ref _drawMoveOnXLeanTweenType;
    public ref LeanTweenType DrawMoveOnYLeanTweenType => ref _drawMoveOnYLeanTweenType;
    public ref float DrawTransitionXTime => ref _drawTransitionXTime;
    public ref float DrawTransitionYTime => ref  _drawTransitionYTime;

    #endregion

    #region Removal After Activation
    [TabGroup("Card UI/CardUI", "Remove After Use Card")]

    #region Removal Scale
    [TabGroup("Card UI/CardUI/Remove After Use Card/Params", "Scale")]
    [Range(0f, 5f)]
    [SerializeField] float _scaleSizeForRemoval;
    [TabGroup("Card UI/CardUI/Remove After Use Card/Params", "Scale")]
    [Range(0f,2f)]
        [SerializeField] float _removalTime;
    [TabGroup("Card UI/CardUI/Remove After Use Card/Params", "Scale")]
    [SerializeField] LeanTweenType _scaleRemovalTweenType;
    #endregion

    #region Alpha   
   [TabGroup("Card UI/CardUI/Remove After Use Card/Params", "Alpha")]
    [Range(0,1)]
        [SerializeField] float _alphaAmountForRemoval;

    [TabGroup("Card UI/CardUI/Remove After Use Card/Params", "Alpha")]
    [SerializeField] float _alphaRemovalTime;

    [TabGroup("Card UI/CardUI/Remove After Use Card/Params", "Alpha")]
    [SerializeField] LeanTweenType _alphaRemovalTweenType;
    #endregion

    #region Removal On X Axis
    [TabGroup("Card UI/CardUI/Remove After Use Card/Params", "Move On X")]
    [Range(0,2)]
    [SerializeField] float _removalTransitionXTime;

    [TabGroup("Card UI/CardUI/Remove After Use Card/Params", "Move On X")]
    [SerializeField] LeanTweenType _removalMoveOnXLeanTweenType;
    #endregion

    #region Removal On Y Axis
    [TabGroup("Card UI/CardUI/Remove After Use Card/Params", "Move On Y")]
    [Range(0,2)]
      [SerializeField] float _removalTransitionYTime;
    [TabGroup("Card UI/CardUI/Remove After Use Card/Params", "Move On Y")]
    [SerializeField] LeanTweenType _removalMoveOnYLeanTweenType;
    #endregion
    [TabGroup("Card UI/CardUI", "Remove After Use Card")]
    [Range(0, 1)]
    [SerializeField] float _delayTillStartMovement;
    public ref float GetDelayTillStartMovement => ref _delayTillStartMovement;
    public ref LeanTweenType GetMoveOnYLeanTween => ref _removalMoveOnYLeanTweenType;
    public ref float GetRemovalTransitionYTime => ref _removalTransitionYTime;
    public ref LeanTweenType GetMoveOnXLeanTween => ref _removalMoveOnXLeanTweenType;
    public ref float GetRemovalTransitionXTime => ref _removalTransitionXTime;
    public ref float GetAlphaRemovalAmount => ref _alphaAmountForRemoval;
    public ref float GetAlphaRemovalTime => ref _alphaRemovalTime;
    public ref LeanTweenType GetAlphaLeanTween => ref _alphaRemovalTweenType;
    public ref float GetRemovalTimeForRemoval => ref _removalTime;
    public ref LeanTweenType GetScaleRemovalLeanTweenType => ref _scaleRemovalTweenType;
    public ref float GetScaleSizeForRemoval => ref _scaleSizeForRemoval;

    #endregion

    #endregion
    #region Properties
    public ref float YFactorPerOffsetCardInHand => ref _yFactorPerOffsetCardInHand;
        public ref int CardAmountOffset => ref _cardAmountOffset;
        public ref float GetDelayBetweenRemovalOfEachCard => ref _delayBetweenCardsIsDiscarded;
        public ref float GetCardReturnSpeedDelay => ref _returnSpeedDelay;
        public ref float GetSpaceBetweenCards => ref _amountOfSpaceBetweenCards;
        public ref int GetAmountOfCardsUIInHand => ref _amountOfCardsUIInHand;
        public ref float GetCardFollowDelay => ref _cardFollowTheTouchDelay;
        public ref float GetCardMoveToDeckDelay => ref _cardGoToDeckDelay;
        public ref float GetCardScaleDelay => ref _cardScalingDelay;
        public Vector3 GetCardUIZoomedScale => Vector3.one * _zoomedScale;
        public Vector3 GetCardDefaultScale => Vector3.one * _cardScale;
    public ref float CardAlignmentInHandHeight => ref _cardAlignmentInHandHeight;
    public ref float DegreePerCard => ref _degreePerCard;

    public ref float ScaleFactorInSpaceInHand => ref _scaleFactorInSpaceInHand;
    #endregion

}

