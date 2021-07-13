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
                                 true,
                                 BodyPartEnum.Chest,
                                 VFXManager.KeywordToParticle(_currentCard.GetSetCard.GetCardsKeywords[0].GetKeywordSO.GetKeywordType));
                                    _playSound?.Raise(SoundsNameEnum.GainArmor);
                                 break;

                                case Keywords.KeywordTypeEnum.Strength:
                                case Keywords.KeywordTypeEnum.Heal:
                                    _playSound?.Raise(SoundsNameEnum.Healing);

                                VFXManager.Instance.PlayParticle(
                                true,
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
     
        public void ExecuteCard()
        {


            if (_currentCard == null || _currentCard.GetCardKeywords == null || _currentCard.GetCardKeywords.Length == 0 || BattleManager.isGameEnded)
                return;

            for (int j = 0; j < _currentCard.GetCardKeywords.Length; j++)
                Keywords.KeywordManager.Instance.ActivateKeyword(_currentCard.GetCardKeywords[j]);


            //if (_comboArr[_currentCardIndex] == null)
            //{
            //    for (int i = _currentCardIndex; i < _comboArr.Length; i++)
            //    {
            //        if (_comboArr[i] != null)
            //            _currentCardIndex = i;
            //    }
            //}
            //var currentCard = _comboArr[_currentCardIndex];
            //if (currentCard == null | currentCard.GetCardKeywords == null || currentCard.GetCardKeywords.Length == 0)
            //    return;

            //for (int j = 0; j < currentCard.GetCardKeywords.Length; j++)
            //    Keywords.KeywordManager.Instance.ActivateKeyword(currentCard.GetCardKeywords[j]);

        }


        //private IEnumerator ExecuteCombo()
        //{




        //    // execute cards
        //    // execute combos
        //    // play animations
        //    // end player turn;


        //    // run for cards
        //    // each card execute his keywords if LCE execute also his LCE keywords
        //    // play animation
        //    // Execute Keywords  
        //    // wait for the animation to finish
        //    // play the other cards

        //    yield return RunOnCards();
        //    yield return null;
        //}
        //    private IEnumerator RunOnCards()
        //    {
        //        // get the animation length so each card will have a delay between them
        //        // check if the card is valid to execute
        //        // execute the keywords in order 
        //        if(_animatorController == null)
        //        {
        //            Debug.LogError("Error in RunOnCards");
        //            yield break;
        //        }


        //        float animationLength = 0.1f;
        //        var relicList = Relics.RelicManager.Instance.GetRelicFounds;
        //        bool animationRelic = false;

        //        for (int i = 0; i < _comboArr.Length; i++)
        //        {
        //            if (_comboArr[i] == null || _comboArr[i].GetCardKeywords == null || _comboArr[i].GetCardKeywords.Length == 0)
        //                continue;


        //            //detect combos on placementSlots
        //            if (relicList.Count == 0 || (relicList[0]._lastIndex <= i && relicList[0]._lastIndex - relicList[0]._slotLength >= i))
        //            {
        //                animationRelic = false;
        //                _animatorController.PlayAnimation(_comboArr[i].GetSetCard.GetCardName);
        //            }
        //            else
        //            {


        //                if (!animationRelic)
        //                {
        //                    animationRelic = true;
        //                    for (int k = 0; k < relicList.Count; k++)
        //                    {
        //                        _animatorController.PlayRelicAnimation(relicList[k]._relic.GetRelicName);

        //                        for (int j = 0; j < relicList[k]._relic.GetKeywordEffect.Length; j++)
        //                        {
        //                            Keywords.KeywordManager.Instance.ActivateKeyword(relicList[k]._relic.GetKeywordEffect[j]);
        //                            yield return null;
        //                        }
        //                    }
        //                }


        //            }

        //            for (int j = 0; j < _comboArr[i].GetCardKeywords.Length; j++)
        //            {
        //                Keywords.KeywordManager.Instance.ActivateKeyword(_comboArr[i].GetCardKeywords[j]);
        //                yield return null;
        //            }

        //            if (animationRelic&& i !=_comboArr.Length-1)
        //                continue;



        //            yield return new WaitUntil(() => false == _animatorController.GetIsAnimationCurrentlyActive );


        //                _animatorController.ResetToStartingPosition();
        //        }


        //    }

        //}
    } 
}