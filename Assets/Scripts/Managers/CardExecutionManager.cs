using Battles.Deck;
using Battles.UI;
using Characters.Stats;
using System.Collections.Generic;
using UnityEngine;

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
            _playersKeyword = new Queue<Keywords.KeywordData>();
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


            DeckManager.Instance.TransferCard(DeckEnum.Selected, card.GetSetCard.ToExhaust ?DeckEnum.Exhaust : DeckEnum.Disposal, card);

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
                    switch (_currentCard.GetSetCard.GetCardType._cardType)
                    {
                        case Cards.CardTypeEnum.Utility:
                        case Cards.CardTypeEnum.Defend:
                            switch (_currentCard.GetSetCard.GetCardsKeywords[0].GetKeywordSO.GetKeywordType)
                            {
                                case Keywords.KeywordTypeEnum.Defense:

                                 VFXManager.Instance.PlayParticle(
                                 isPlayer,
                                 BodyPartEnum.Chest,
                                 VFXManager.KeywordToParticle(_currentCard.GetSetCard.GetCardsKeywords[0].GetKeywordSO.GetKeywordType));
                                    _playSound?.Raise(SoundsNameEnum.GainArmor);

                                 break;

                                case Keywords.KeywordTypeEnum.Strength:

                                    _playSound?.Raise(SoundsNameEnum.GainStrength);
                                    VFXManager.Instance.PlayParticle(
                                isPlayer,
                                BodyPartEnum.BottomBody,
                                VFXManager.KeywordToParticle(_currentCard.GetSetCard.GetCardsKeywords[0].GetKeywordSO.GetKeywordType));

                                    break;


                                case Keywords.KeywordTypeEnum.Heal:

                                    _playSound?.Raise(SoundsNameEnum.Healing);

                                VFXManager.Instance.PlayParticle(
                                isPlayer,
                                BodyPartEnum.BottomBody,
                                VFXManager.KeywordToParticle(_currentCard.GetSetCard.GetCardsKeywords[0].GetKeywordSO.GetKeywordType));

                                    break;


                                case Keywords.KeywordTypeEnum.Attack:
                                case Keywords.KeywordTypeEnum.Bleed:
                                case Keywords.KeywordTypeEnum.MaxHealth:
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



        static Queue<Keywords.KeywordData> _playersKeyword;
        static Queue<Keywords.KeywordData> _enemyKeyword;


        public void ExecuteCard()
        {
            if (_currentCard == null || _currentCard.GetCardKeywords == null || _currentCard.GetCardKeywords.Length == 0 || BattleManager.isGameEnded)
                return;

            for (int j = 0; j < _currentCard.GetCardKeywords.Length; j++)
                Keywords.KeywordManager.Instance.ActivateKeyword(_currentCard.GetCardKeywords[j]);
        }






        public void ExecuteCard(bool isPlayer)
        {






        }

        

    } 
}