using Cards;
using System.Collections;
using UnityEngine; 

namespace Battles
{
    public class CardExecutionManager : MonoSingleton<CardExecutionManager>
    {
        Cards.Card[] _comboArr;
        [SerializeField]
        AnimatorController _animatorController;
        int _currentCardIndex;
        public void StopCoroutine()
        {
            StopAllCoroutines();
        }
        public override void Init()
        {
            ResetExecution();
        }

        public void AssignCombo(ref Card[] cards)
        {
            _comboArr = cards;
        }

        public void RegisterExecutions()
        {

            _comboArr = Deck.DeckManager.Instance.GetCardsFromDeck(Deck.DeckEnum.Placement);

            _animatorController.SetAnimationQueue(_comboArr);

            _currentCardIndex =0;
         

            // create function

            // cancel inputs 
            // detect LCE
            // detect Relics
            // run on the placement deck

            // ExecuteCombo();

            //    ResetExecution();
        }

        public void MoveToNextIndex() {
            if (_comboArr == null || _comboArr.Length == 0)
                return;
            if (_comboArr.Length - 1 <= _currentCardIndex)
                _currentCardIndex = 0;
            else
            {
                for (int i = _currentCardIndex; i < _comboArr.Length-1; i++)
                {
                    if (_comboArr[i+1] != null)
                    {
                        _currentCardIndex = i + 1;
                        return;
                    }
                }
            }

        }

        public void ExecuteCard()
        {

            if (_comboArr[_currentCardIndex] == null)
            {
                for (int i = _currentCardIndex; i < _comboArr.Length; i++)
                {
                    if (_comboArr[i] != null)
                        _currentCardIndex = i;
                }
            }
            var currentCard = _comboArr[_currentCardIndex];
            if (currentCard == null | currentCard.GetCardKeywords == null || currentCard.GetCardKeywords.Length == 0)
                return ;
            for (int j = 0; j < currentCard.GetCardKeywords.Length; j++)
                Keywords.KeywordManager.Instance.ActivateKeyword(currentCard.GetCardKeywords[j]);

            }
        public void ResetExecution()
        {
            _comboArr = null;
        }


        private IEnumerator ExecuteCombo()
        {


            
     
            // execute cards
            // execute combos
            // play animations
            // end player turn;


            // run for cards
            // each card execute his keywords if LCE execute also his LCE keywords
            // play animation
            // Execute Keywords  
            // wait for the animation to finish
            // play the other cards

            yield return RunOnCards();
            yield return null;
        }
        private IEnumerator RunOnCards()
        {
            // get the animation length so each card will have a delay between them
            // check if the card is valid to execute
            // execute the keywords in order 
            if(_animatorController == null)
            {
                Debug.LogError("Error in RunOnCards");
                yield break;
            }


            float animationLength = 0.1f;
            var relicList = Relics.RelicManager.Instance.GetRelicFounds;
            bool animationRelic = false;

            for (int i = 0; i < _comboArr.Length; i++)
            {
                if (_comboArr[i] == null || _comboArr[i].GetCardKeywords == null || _comboArr[i].GetCardKeywords.Length == 0)
                    continue;


                //detect combos on placementSlots
                if (relicList.Count == 0 || (relicList[0]._lastIndex <= i && relicList[0]._lastIndex - relicList[0]._slotLength >= i))
                {
                    animationRelic = false;
                    _animatorController.PlayAnimation(_comboArr[i].GetSetCard.GetCardName);
                }
                else
                {


                    if (!animationRelic)
                    {
                        animationRelic = true;
                        for (int k = 0; k < relicList.Count; k++)
                        {
                            _animatorController.PlayRelicAnimation(relicList[k]._relic.GetRelicName);

                            for (int j = 0; j < relicList[k]._relic.GetKeywordEffect.Length; j++)
                            {
                                Keywords.KeywordManager.Instance.ActivateKeyword(relicList[k]._relic.GetKeywordEffect[j]);
                                yield return null;
                            }
                        }
                    }


                }
                
                for (int j = 0; j < _comboArr[i].GetCardKeywords.Length; j++)
                {
                    Keywords.KeywordManager.Instance.ActivateKeyword(_comboArr[i].GetCardKeywords[j]);
                    yield return null;
                }

                if (animationRelic&& i !=_comboArr.Length-1)
                    continue;
                


                yield return new WaitUntil(() => false == _animatorController.GetIsAnimationCurrentlyActive );

    
                    _animatorController.ResetToStartingPosition();
            }

           
        }

    }
}
