﻿using Managers;
using UnityEngine;
using Battles.Deck;
using Characters.Stats;

namespace Battles
{
    public class EnemyManager : MonoSingleton<EnemyManager> , IBattleHandler
    {
        #region Fields
     //   [UnityEngine.SerializeField] Opponents EnemyAI;

        [Tooltip("Player Stats: ")]

        [SerializeField] private  Character _myCharacter;
        [Space]

        int _cardAction;
        [SerializeField] Cards.Card enemyAction;
        [SerializeField]  AnimatorController _enemyAnimatorController;
        #endregion
         public Combo.Combo[] Recipes => _myCharacter.ComboRecipe;
        private Cards.Card[] _deck;
        public Cards.Card[] Deck => _deck;
        public ref CharacterStats GetCharacterStats => ref _myCharacter.CharacterStats;
        public static AnimatorController EnemyAnimatorController => Instance._enemyAnimatorController;
        #region Public Methods
        public override void Init()
        {
           
        }


        public void RestartBattle()
        {

        }

        public void AssignCharacterData(CharacterSO characterSO)
      => AssignCharacterData(new Character(characterSO));
        public void AssignCharacterData(Character character)
        { 
            _myCharacter = character;
            int deckLength = character.CharacterDeck.Length;
            _deck = new Cards.Card[deckLength];
            System.Array.Copy(character.CharacterDeck, _deck, deckLength);

            CharacterStatsManager.RegisterCharacterStats(false, ref character.CharacterStats);
            DeckManager.Instance.InitDeck(false, _deck);

            EnemyAnimatorController.ResetAnimator();
        }
        public void UpdateStatsUI()
        {
            UI.StatsUIManager.GetInstance.UpdateMaxHealthBar(false, GetCharacterStats.MaxHealth);
            UI.StatsUIManager.GetInstance.InitHealthBar(false, GetCharacterStats.Health);
            UI.StatsUIManager.GetInstance.UpdateShieldBar(false, GetCharacterStats.Shield);
        }

        public void OnEndBattle()
        {

        }


        public void CalculateEnemyMoves()
        {
            //ThreadsHandler.ThreadHandler.StartThread(new ThreadsHandler.ThreadList(ThreadsHandler.ThreadHandler.GetNewID,))

        }


        public void OnEndTurn() => _enemyAnimatorController.ResetLayerWeight();
        public System.Collections.IEnumerator PlayEnemyTurn()
        {
            Debug.Log("Enemy Attack!");

            var staminaHandler = Characters.Stats.StaminaHandler.Instance;

           var handCards = DeckManager.Instance.GetCardsFromDeck(false, DeckEnum.Hand);


            int indexCheck = -1;
            bool noMoreCardsAvailable = false;
            do
            {
                do
                {
                    yield return null;
                    indexCheck++;   
                    if (indexCheck >= handCards.Length)
                    {
                        noMoreCardsAvailable = true;
                        break;
                    }

                    enemyAction = handCards[indexCheck];

                    if (staminaHandler.IsEnoughStamina(false, enemyAction))
                        DeckManager.Instance.TransferCard(false, DeckEnum.Hand, DeckEnum.Selected, enemyAction);

                

                } while (!CardExecutionManager.Instance.TryExecuteCard(false, enemyAction));

                if (noMoreCardsAvailable == false)
                {
                    if (enemyAction.CardSO.CardTypeEnum == Cards.CardTypeEnum.Attack)
                        yield return new WaitForSeconds(.3f);
                    else
                        yield return Turns.Turn.WaitOneSecond;
                }

                indexCheck = -1;
            } while (staminaHandler.HasStamina(false) && noMoreCardsAvailable == false);



            yield return new WaitUntil(() => EnemyAnimatorController.GetIsAnimationCurrentlyActive == false);
            yield return Turns.Turn.WaitOneSecond;
            EnemyAnimatorController.ResetToStartingPosition();
        }


        #endregion

    }

 
}
