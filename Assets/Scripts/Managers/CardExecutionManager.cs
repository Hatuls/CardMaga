using Battles.Deck;
using Battles.UI;
using Characters.Stats;
using System.Collections.Generic;
using UnityEngine;
using Keywords;

namespace Battles
{
    public class CardExecutionManager : MonoSingleton<CardExecutionManager>
    {
        [SerializeField]
        AnimatorController _playerAnimator;
        [SerializeField]
        AnimatorController _enemyAnimator;
        [SerializeField] VFXController __playerVFXHandler;
     
        [SerializeField] Unity.Events.SoundsEvent _playSound;
        static Queue<Cards.Card> _cardsQueue;
        static List<KeywordData> _keywordData;

        static int currentKeywordIndex;

        public void ResetExecution()
        {
            //_keywordData.Clear();
            //_cardsQueue.Clear();
            //currentKeywordIndex = 0;
            StopAllCoroutines();
        }
    
        public override void Init()
        {
            _cardsQueue= new Queue<Cards.Card>();
            _keywordData = new List<KeywordData>();
        }
        public bool TryExecuteCard(Cards.Card card)
        {

            if (StaminaHandler.IsEnoughStamina(card) == false)
            {
                // not enough stamina 

                _playSound?.Raise(SoundsNameEnum.Reject);
                CardUIManager.Instance.ZoomCard(null);
                CardUIManager.Instance.SelectCardUI(null);
                return false;
            }

            // execute card
            StaminaHandler.ReduceStamina(card);

            CardUIManager.Instance.LockHandCards(false);


            DeckManager.Instance.TransferCard(true, DeckEnum.Selected, card.CardSO.ToExhaust ? DeckEnum.Exhaust : DeckEnum.Disposal, card);


            RegisterCard(card);

            DeckManager.AddToCraftingSlot(true, card);

            
            return true;
        }

        public bool TryExecuteCard(CardUI cardUI)
        {
            Cards.Card card = cardUI.GFX.GetCardReference;
            bool isExecuted = TryExecuteCard(card);
            if (isExecuted)
            {
            // reset the holding card
            CardUIManager.Instance.ExecuteCardUI(cardUI);
            }
            return isExecuted;
        }

        public void ExecuteCraftedCard(bool isPlayer,Cards.Card card)
        {
            DeckManager.AddToCraftingSlot(isPlayer, card);
            RegisterCard(card);

        }

        public void RegisterCard(Cards.Card card, bool isPlayer = true)
        {

            if (BattleManager.isGameEnded)
                return;



            var currentCard = card;

            if (currentCard != null)
            {
                switch (currentCard.CardSO.CardType.CardType)
                {
                    case Cards.CardTypeEnum.Utility:
                    case Cards.CardTypeEnum.Defend:
                        switch (currentCard.CardSO.CardSOKeywords[0].KeywordSO.GetKeywordType)
                        {
                            case KeywordTypeEnum.Defense:

                                VFXManager.Instance.PlayParticle(
                                isPlayer,
                                BodyPartEnum.Chest,
                                VFXManager.KeywordToParticle(currentCard.CardSO.CardSOKeywords[0].KeywordSO.GetKeywordType)
                                );

                                _playSound?.Raise(SoundsNameEnum.GainArmor);

                                break;

                            case KeywordTypeEnum.Strength:

                                _playSound?.Raise(SoundsNameEnum.GainStrength);

                                VFXManager.Instance.PlayParticle(
                                isPlayer,
                                BodyPartEnum.BottomBody,
                                VFXManager.KeywordToParticle(currentCard.CardSO.CardSOKeywords[0].KeywordSO.GetKeywordType)
                                );

                                break;


                            case KeywordTypeEnum.Heal:

                                _playSound?.Raise(SoundsNameEnum.Healing);

                                VFXManager.Instance.PlayParticle(
                                isPlayer,
                                BodyPartEnum.BottomBody,
                                VFXManager.KeywordToParticle(currentCard.CardSO.CardSOKeywords[0].KeywordSO.GetKeywordType)
                                );

                                break;


                            case KeywordTypeEnum.Attack:
                            case KeywordTypeEnum.Bleed:
                            case KeywordTypeEnum.MaxHealth:
                            default:
                                break;
                        }

                        ExecuteCard(currentCard);
                        break;

                    case Cards.CardTypeEnum.Attack:
                        //    _playerAnimator.SetAnimationQueue(card);
                        AddToQueue(card);
                        break;


                    default:
                        Debug.LogError($" Card Type is Not Valid -  {currentCard.CardSO.CardType.CardType}");
                        break;
                }
            }

        }


        public void ExecuteCard(Cards.Card current)
        {
            if (current == null || current.CardKeywords == null || current.CardKeywords.Length == 0 || BattleManager.isGameEnded)
                return;
            bool currentTurn = (Turns.TurnHandler.CurrentState == Turns.TurnState.EndPlayerTurn || Turns.TurnHandler.CurrentState == Turns.TurnState.PlayerTurn || Turns.TurnHandler.CurrentState == Turns.TurnState.StartPlayerTurn);


            for (int j = 0; j < current.CardKeywords.Length; j++)
                KeywordManager.Instance.ActivateKeyword(currentTurn, current.CardKeywords[j]);
        }

        // need to register the players cards into a queue and tell the animation to play the animation
        // when this card is activate need to sort the cards keywords in a list by their indexes
        // when animation is finished to tell the queue to pop up the next card and play it
        // each animation has animation keys to notify which index is currently playing
        // when the animation event fire his index -> the list execute the keyword and move to the next index

        public void AddToQueue(Cards.Card card)
        {   
            bool firstCard = _cardsQueue.Count == 0;
            _cardsQueue.Enqueue(card);
            Debug.Log($"<a>Register card queue has {_cardsQueue.Count} cards in it\nIs First Card {firstCard}</a>");
            if (firstCard)
            {
                ActivateCard();
            }

        }
        void ActivateCard()
        {
            // play the card animation

            Debug.Log("<a>Activating Card</a>");
            // sort his keyowrds
        

            ThreadsHandler.ThreadHandler.StartThread(
                new ThreadsHandler.ThreadList(
                    ThreadsHandler.ThreadHandler.GetNewID,
                    () => SortKeywords(),
                   () =>
                   {  
                       if (_cardsQueue.Count == 0)
                        return;
                       if (Turns.TurnHandler.CurrentState == Turns.TurnState.PlayerTurn)
                           _playerAnimator.SetAnimationQueue(_cardsQueue.Peek());
                       else if (Turns.TurnHandler.CurrentState == Turns.TurnState.EnemyTurn)
                           _enemyAnimator.SetAnimationQueue(_cardsQueue.Peek());
                       else
                           Debug.LogError("Current turn is not a turn that a card could be played!");

                       // reset Index
                       currentKeywordIndex = 0;
                   }
                 )
                );
            //  SortKeywords();

        }
        public void SortKeywords()
        {
            //clearing the list
            // registering the keywords
            // sorting it by the animation index
            Debug.Log("<a>Keywords Cleared</a>");
           _keywordData.Clear();

            var currentCard = _cardsQueue.Peek().CardKeywords;
            Debug.Log($"<a>sorting keywords {_cardsQueue.Count} cards left to be executed </a>");

            for (int i = 0; i < currentCard.Length; i++)
                _keywordData.Add(currentCard[i]);
            
            _keywordData.Sort();
            Debug.Log($"<a>{_keywordData.Count} keys were added</a>");
        }

  
        public void CardFinishExecuting()
        {
                Debug.Log($"<a>Animation Finished {_cardsQueue.Count} left to be executed </a>");
            if (_cardsQueue.Count ==0)
                    return;
            
                Debug.Log("<a>Dequeueing to next card</a>");
            _cardsQueue.Dequeue();

            if (_cardsQueue.Count > 0)
            {
                Debug.Log("<a>Activating Next card</a>");
                ActivateCard();
            }
        }

        public void OnKeywordEvent()
        {
                Debug.Log($"<a>Executing Kewords with {_keywordData.Count} keywords to be executed</a>");
            bool currentTurn = (Turns.TurnHandler.CurrentState == Turns.TurnState.EndPlayerTurn || Turns.TurnHandler.CurrentState == Turns.TurnState.PlayerTurn || Turns.TurnHandler.CurrentState == Turns.TurnState.StartPlayerTurn);


            for (int i = 0; i < _keywordData.Count; i++)
            {
                if (currentKeywordIndex == _keywordData[i].AnimationIndex)
                {
                    // activate the keyword
                    KeywordManager.Instance.ActivateKeyword(currentTurn, _keywordData[i]);

                    //remove from the list
                    _keywordData.Remove(_keywordData[i]);
                    i--;
                }
            }
            currentKeywordIndex++;
        }
    } 
}