using UnityEngine;
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

    #region Properties
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

