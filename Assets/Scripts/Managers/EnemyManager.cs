using Managers;
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
            CharacterStatsManager.RegisterCharacterStats(false,ref _characterStats);


            var factory = Factory.GameFactory.Instance.CardFactoryHandler;

            var CardInfo = characterSO.Deck;
            _deck = new Cards.Card[CardInfo.Length];
            for (int i = 0; i < CardInfo.Length; i++)
                _deck[i] = factory.CreateCard(CardInfo[i].Card, CardInfo[i].Level);

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
