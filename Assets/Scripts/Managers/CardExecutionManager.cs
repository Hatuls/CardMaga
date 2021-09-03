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
        Cards.Card _currentCard;
        [SerializeField] Unity.Events.SoundsEvent _playSound;


        public void ResetExecution()
        {
            StopAllCoroutines();
        }
    
        public override void Init()
        {
            _currentCard = null;
            _keywordsQueue = new Queue<KeywordData>();
        }


        public bool TryExecuteCard(CardUI cardUI)
        {
            Cards.Card card = cardUI.GFX.GetCardReference;


            if (StaminaHandler.IsEnoughStamina(card) == false)
            {
                // not enough stamina 

                _playSound?.Raise(SoundsNameEnum.Reject);
                CardUIManager.Instance.SelectCardUI(null);
                return false;
            }

            // execute card

            CardUIManager.Instance.LockHandCards(false);


            DeckManager.Instance.TransferCard(DeckEnum.Selected, card.CardSO.ToExhaust ?DeckEnum.Exhaust : DeckEnum.Disposal, card);

            DeckManager.AddToCraftingSlot(true,card);
            RegisterCard(card);
            StaminaHandler.ReduceStamina(card);


            // reset the holding card
            CardUIManager.Instance.ExecuteCardUI();
            return true;
        }


        public void RemoveCard() => _currentCard = null;
        public void RegisterCard(Cards.Card card, bool isPlayer = true)
        {

            if (BattleManager.isGameEnded)
                return;



            _currentCard = card;
            if (isPlayer)
            {

                if (_currentCard != null)
                {
                    switch (_currentCard.CardSO.GetCardType._cardType)
                    {
                        case Cards.CardTypeEnum.Utility:
                        case Cards.CardTypeEnum.Defend:
                            switch (_currentCard.CardSO.GetCardsKeywords[0].GetKeywordSO.GetKeywordType)
                            {
                                case KeywordTypeEnum.Defense:

                                 VFXManager.Instance.PlayParticle(
                                 isPlayer,
                                 BodyPartEnum.Chest,
                                 VFXManager.KeywordToParticle(_currentCard.CardSO.GetCardsKeywords[0].GetKeywordSO.GetKeywordType));
                                    _playSound?.Raise(SoundsNameEnum.GainArmor);

                                 break;

                                case KeywordTypeEnum.Strength:

                                    _playSound?.Raise(SoundsNameEnum.GainStrength);
                                    VFXManager.Instance.PlayParticle(
                                isPlayer,
                                BodyPartEnum.BottomBody,
                                VFXManager.KeywordToParticle(_currentCard.CardSO.GetCardsKeywords[0].GetKeywordSO.GetKeywordType));

                                    break;


                                case KeywordTypeEnum.Heal:

                                    _playSound?.Raise(SoundsNameEnum.Healing);

                                VFXManager.Instance.PlayParticle(
                                isPlayer,
                                BodyPartEnum.BottomBody,
                                VFXManager.KeywordToParticle(_currentCard.CardSO.GetCardsKeywords[0].GetKeywordSO.GetKeywordType));

                                    break;


                                case KeywordTypeEnum.Attack:
                                case KeywordTypeEnum.Bleed:
                                case KeywordTypeEnum.MaxHealth:
                                default:
                                    break;
                            }
                           
                            ExecuteCard();
                            break;
                        case Cards.CardTypeEnum.Attack:
                            _playerAnimator.SetAnimationQueue(card);

                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                _enemyAnimator.SetAnimationQueue(card);
            }
        }



        static Queue<KeywordData> _keywordsQueue;
    


        public void ExecuteCard()
        {
            if (_currentCard == null || _currentCard.CardKeywords == null || _currentCard.CardKeywords.Length == 0 || BattleManager.isGameEnded)
                return;

            for (int j = 0; j < _currentCard.CardKeywords.Length; j++)
                KeywordManager.Instance.ActivateKeyword(_currentCard.CardKeywords[j]);
        }






        public void ExecuteCard(bool isPlayer)
        {






        }

        

    } 
}