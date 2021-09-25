using Managers;
using UnityEngine;
using Battles.Deck;
namespace Battles
{
    public class EnemyManager : MonoSingleton<EnemyManager> , IBattleHandler
    {
        #region Fields
     //   [UnityEngine.SerializeField] Opponents EnemyAI;

        [Tooltip("Player Stats: ")]
        [SerializeField] private Characters.Stats.CharacterStats _characterStats;
        [SerializeField] private  CharacterSO _myCharacter;
        [SerializeField] private Cards.Card[] _deck;
        [SerializeField] private CharacterSO.RecipeInfo[] _recipes;
        public Cards.Card[] Deck => _deck;

        [SerializeField] Cards.Card enemyAction;
        int _cardAction;
        [SerializeField]  AnimatorController _enemyAnimatorController;
        #endregion
         public CharacterSO.RecipeInfo[] Recipes => _recipes;
        public ref Characters.Stats.CharacterStats GetCharacterStats => ref _characterStats;
        public static AnimatorController EnemyAnimatorController => Instance._enemyAnimatorController;
        #region Public Methods
        public override void Init()
        {
           
        }


        public void RestartBattle()
        {

        }

        public void AssignCharacterData(CharacterSO characterSO)
        {

           // EnemyAI = new Opponents();
            _myCharacter = characterSO;
            _characterStats = characterSO.CharacterStats;


            var CardInfo = characterSO.Deck;
            _deck = new Cards.Card[CardInfo.Length];
            for (int i = 0; i < CardInfo.Length; i++)
                _deck[i] = CardManager.Instance.CreateCard(CardInfo[i].Card, CardInfo[i].Level);

            DeckManager.Instance.InitDeck(false, _deck);


            _recipes = characterSO.Combos;
            EnemyAnimatorController.ResetAnimator();
        }

        public void UpdateStats()
        {


            UI.StatsUIManager.GetInstance.UpdateMaxHealthBar(false, _characterStats.MaxHealth);
            UI.StatsUIManager.GetInstance.InitHealthBar(false, _characterStats.Health);
            UI.StatsUIManager.GetInstance.UpdateShieldBar(false, _characterStats.Shield);
        }

        public void OnEndBattle()
        {

        }


        public void CalculateEnemyMoves()
        {
            //ThreadsHandler.ThreadHandler.StartThread(new ThreadsHandler.ThreadList(ThreadsHandler.ThreadHandler.GetNewID,))

        }

        private void SortEnemyCards()
        {
            var EnemyHand = DeckManager.Instance.GetCardsFromDeck(false, DeckEnum.Hand);
            System.Array.Sort(EnemyHand);
            for (int i = 0; i < EnemyHand.Length; i++)
            {

            }
        }

        public System.Collections.IEnumerator PlayEnemyTurn()
        {
            Debug.Log("Enemy Attack!");

            var staminaHandler = Characters.Stats.StaminaHandler.Instance; 
            int indexCheck = -1;
            bool noMoreCardsAvailable;
            do
            {
                do
                {
                    indexCheck++;
                    yield return null;
                    enemyAction = DeckManager.Instance.GetCardFromDeck(false, indexCheck, DeckEnum.Hand);

                    noMoreCardsAvailable = DeckManager.Instance.GetCardsFromDeck(false, DeckEnum.Hand).Length - 1 <= indexCheck;


                } while (!CardExecutionManager.Instance.TryExecuteCard(false, enemyAction));

                if (enemyAction.CardSO.CardTypeEnum == Cards.CardTypeEnum.Attack)
                    yield return new WaitForSeconds(.3f);
                else
                    yield return new WaitForSeconds(1f);

                   indexCheck = -1;
            } while (staminaHandler.HasStamina(false) && noMoreCardsAvailable == true) ;

            

            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => EnemyManager.EnemyAnimatorController.GetIsAnimationCurrentlyActive == false);
            EnemyManager.EnemyAnimatorController.ResetToStartingPosition();
        }


        public Cards.Card GetOpponentCard
        {
            get
            {
                _cardAction++;

                return _deck[_cardAction % _deck.Length];
            }
        }
        #endregion

    }

 
}
