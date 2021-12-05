using Battles.Deck;
using Battles.UI;
using Characters.Stats;
using Keywords;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battles
{
    public class CardExecutionManager : MonoSingleton<CardExecutionManager>
    {
        [SerializeField]
        AnimatorController _playerAnimatorController;
        [SerializeField]
        AnimatorController _enemyAnimatorController;
        [SerializeField] VFXController __playerVFXHandler;

        [SerializeField] Unity.Events.StringEvent _playSound;
        [Sirenix.OdinInspector.ShowInInspector]
        static Queue<Cards.Card> _cardsQueue = new Queue<Cards.Card>();
        public static Queue<Cards.Card> CardsQueue => _cardsQueue;
        static List<KeywordData> _keywordData = new List<KeywordData>();
        [SerializeField] StaminaUI _staminaBtn;
        static int currentKeywordIndex;
        public static bool FinishedAnimation;


        [SerializeField] UnityEvent OnSuccessfullExecution;
        [SerializeField] UnityEvent OnFailedToExecute;
        public void ResetExecution()
        {
            //_keywordData.Clear();
            //_cardsQueue.Clear();
            //currentKeywordIndex = 0;
            StopAllCoroutines();
        }

        public override void Init()
        {
            FinishedAnimation = true;
            _cardsQueue.Clear();
            _keywordData.Clear();
        }
        public bool TryExecuteCard(bool isPlayer, Cards.Card card)
        {
            if (card == null)
                throw new System.Exception("Card cannot be executed card is null\n Player " + isPlayer + " Tried to play a null Card");
            if (StaminaHandler.Instance.IsEnoughStamina(isPlayer, card) == false)
            {
                // not enough stamina 
                if (isPlayer)
                {
                    _staminaBtn.PlayRejectAnimation();
                    OnFailedToExecute?.Invoke();

                }

                return false;
            }

            // execute card
            StaminaHandler.Instance.ReduceStamina(isPlayer, card);
            OnSuccessfullExecution?.Invoke();
            if (isPlayer)
            {

                CardUIManager.Instance.LockHandCards(false);
            }
            else
                CardUIManager.Instance.PlayEnemyCard(card);

            DeckManager.Instance.TransferCard(isPlayer, DeckEnum.Selected, card.CardSO.ToExhaust ? DeckEnum.Exhaust : DeckEnum.Disposal, card);

            RegisterCard(card, isPlayer);

            DeckManager.AddToCraftingSlot(isPlayer, card);


            return true;
        }

        public bool TryExecuteCard(CardUI cardUI)
        {
            Cards.Card card = cardUI.GFX.GetCardReference;
            bool isExecuted = TryExecuteCard(true, card);
            if (isExecuted)
            {
                // reset the holding card
                CardUIManager.Instance.ExecuteCardUI(cardUI);
            }
            return isExecuted;
        }

        public void ExecuteCraftedCard(bool isPlayer, Cards.Card card)
        {
            DeckManager.AddToCraftingSlot(isPlayer, card);
            RegisterCard(card);

        }

        public void RegisterCard(Cards.Card card, bool isPlayer = true)
        {

            if (BattleManager.isGameEnded)
                return;



            var currentCard = card;

            if (currentCard == null)
                throw new System.Exception($"Cannot Execute Card that is null!!!!");
            switch (currentCard.CardSO.CardType.CardType)
            {
                case Cards.CardTypeEnum.Utility:
                case Cards.CardTypeEnum.Defend:
                    ExecuteUtilityAndDefendCard(isPlayer, currentCard);

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

        private void ExecuteUtilityAndDefendCard(bool isPlayer, Cards.Card currentCard)
        {
            switch (currentCard.CardSO.CardSOKeywords[0].KeywordSO.GetKeywordType)
            {
                case KeywordTypeEnum.Shield:

                    VFXManager.Instance.PlayParticle(
                    isPlayer,
                    BodyPartEnum.Chest,
                    VFXManager.KeywordToParticle(currentCard.CardSO.CardSOKeywords[0].KeywordSO.GetKeywordType)
                    );



                    break;

                case KeywordTypeEnum.Strength:


                    VFXManager.Instance.PlayParticle(
                    isPlayer,
                    BodyPartEnum.BottomBody,
                    VFXManager.KeywordToParticle(currentCard.CardSO.CardSOKeywords[0].KeywordSO.GetKeywordType)
                    );

                    break;


                case KeywordTypeEnum.Heal:
                    VFXManager.Instance.PlayParticle(
                                        isPlayer,
                                        BodyPartEnum.BottomBody,
                                        VFXManager.KeywordToParticle(currentCard.CardSO.CardSOKeywords[0].KeywordSO.GetKeywordType)
                                        );

                    break;


                case KeywordTypeEnum.Stamina:
                case KeywordTypeEnum.Attack:
                case KeywordTypeEnum.Bleed:
                case KeywordTypeEnum.MaxHealth:
                default:
                    break;
            }

            ExecuteCard(currentCard);
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
            if (BattleManager.isGameEnded)
                return;

            _cardsQueue.Enqueue(card);
            bool firstCard = _cardsQueue.Count == 1;
            Debug.Log($"<a>Register card queue has {_cardsQueue.Count} cards in it\nIs First Card {firstCard}</a>");
            if (firstCard)
            {
                ActivateCard();
            }

        }
        public void ActivateCard()
        {
            // play the card animation
            if (_cardsQueue.Count == 0 || BattleManager.isGameEnded)
                return;
            Debug.Log("<a>Activating Card</a>");
            // sort his keyowrds

            {
                //ThreadsHandler.ThreadHandler.StartThread(
                //    new ThreadsHandler.ThreadList(
                //        ThreadsHandler.ThreadHandler.GetNewID,
                //        () => SortKeywords(),
                //       () =>
                //       {
                //           if (_cardsQueue.Count == 0)
                //               return;
                //           var State = Turns.TurnHandler.CurrentState;
                //           if (State == Turns.TurnState.PlayerTurn || State == Turns.TurnState.StartPlayerTurn || State == Turns.TurnState.EndPlayerTurn)
                //               _playerAnimator.SetAnimationQueue(_cardsQueue.Peek());
                //           else if (State == Turns.TurnState.EnemyTurn || State == Turns.TurnState.StartEnemyTurn || State == Turns.TurnState.EndEnemyTurn)
                //               _enemyAnimator.SetAnimationQueue(_cardsQueue.Peek());
                //           else
                //               Debug.LogError("Current turn is not a turn that a card could be played!");

                //           // reset Index
                //           currentKeywordIndex = 0;
                //       }
                //     )
                //    );
            }

            SortKeywords();

            currentKeywordIndex = 0;

            if (Turns.TurnHandler.IsPlayerTurn)
                _playerAnimatorController.PlayCrossAnimation();
            else
                _enemyAnimatorController.PlayCrossAnimation();

            // reset Index


        }

        internal bool CanDefendIncomingAttack(bool Reciever)
        {
            for (int i = 0; i < _keywordData.Count; i++)
            {
                if (currentKeywordIndex == _keywordData[i].AnimationIndex)
                {
                    if (_keywordData[i].KeywordSO.GetKeywordType == KeywordTypeEnum.Attack)
                    {
                        return CharacterStatsManager.GetCharacterStatsHandler(Reciever).GetStats(KeywordTypeEnum.Shield).Amount >= _keywordData[i].GetAmountToApply;
                    }
                }
            }
            return false;
        }

        private void SortKeywords()
        {
            if (_cardsQueue.Count > 0)
            {
                //clearing the list
                // registering the keywords
                // sorting it by the animation index
                //  Debug.Log("<a>Keywords Cleared</a>");
                _keywordData.Clear();

                var currentCard = _cardsQueue.Peek().CardKeywords;
                //   Debug.Log($"<a>sorting keywords {_cardsQueue.Count} cards left to be executed </a>");

                for (int i = 0; i < currentCard.Length; i++)
                    _keywordData.Add(currentCard[i]);

                _keywordData.Sort();
                //    Debug.Log($"<a>{_keywordData.Count} keys were added</a>");
            }

        }

        public void CardFinishExecuting()
        {
            if (_cardsQueue.Count > 0)
            {
                Debug.Log("<a>Activating Next card</a>");
                ActivateCard();

            }
            FinishedAnimation = true;
        }

        public void OnKeywordEvent()
        {
            if (BattleManager.isGameEnded)
                return;


            Debug.Log($"<a>Executing Kewords with {_keywordData.Count} keywords to be executed</a>");
            bool currentTurn = (Turns.TurnHandler.CurrentState == Turns.TurnState.EndPlayerTurn || Turns.TurnHandler.CurrentState == Turns.TurnState.PlayerTurn || Turns.TurnHandler.CurrentState == Turns.TurnState.StartPlayerTurn);


            for (int i = 0; i < _keywordData.Count; i++)
            {
                if (currentKeywordIndex == _keywordData[i].AnimationIndex)
                {
                    var keyword = _keywordData[i];
                    //remove from the list
                    _keywordData.Remove(_keywordData[i]);
                    i--;
                    // activate the keyword
                    KeywordManager.Instance.ActivateKeyword(currentTurn, keyword);

                }
            }
            currentKeywordIndex++;
        }
    }
}