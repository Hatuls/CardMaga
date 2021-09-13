using UnityEngine;
namespace Battles
{
    public class EnemyManager : MonoSingleton<EnemyManager> 
    {
        #region Fields
        [UnityEngine.SerializeField] Opponents _opponent;
     
         [SerializeField]  AnimatorController _enemyAnimatorController;
        #endregion

        public static AnimatorController EnemyAnimatorController => Instance._enemyAnimatorController;
        #region Public Methods
        public override void Init()
        {
            if (_opponent == null)
            _opponent = new Opponents();
        }
        public void SetEnemy(CharacterSO _character)
        {
            if (_opponent != null && _character != null)
                _opponent.AssignData(_character);
        }

    

        public Opponents GetEnemy {
            get
            {
                if (_opponent == null)
                {
                    _opponent = new Opponents();
                    _opponent.AssignData(BattleManager.GetDictionary(typeof(EnemyManager))
                            .GetRandomOpponent());
                }

                return  _opponent;
            }
        }
        #endregion

    }

    [System.Serializable]
    public class Opponents
    {
        #region Fields
     
        [SerializeField] CharacterDifficultyEnum _difficultyLevel;
        [SerializeField] CharacterTypeEnum _name;
        [SerializeField] Cards.Card[] _cards;
        [SerializeField] Characters.Stats.CharacterStats _enemyStats;
        [SerializeField] Cards.Card enemyAction;
        int _cardAction;

        #endregion

        public Opponents()
        {

        }
        public Cards.Card GetEnemyAction => enemyAction;

        #region Public Methods
        public System.Collections.IEnumerator AssignNextCard()
        {
            enemyAction = GetOpponentCard;
            Debug.Log("<a>Enemy Next Move</a>: Is going to be: " + enemyAction.CardSO.CardName.ToString() +
            "\n This attack is going to use " + enemyAction.CardSO.BodyPartEnum.ToString() + "\n" +
            "And Do " + enemyAction.CardSO.CardTypeEnum.ToString() + " with the amount of " + enemyAction.CardSO.CardSOKeywords[0].GetAmountToApply);

            yield return new WaitForSeconds(.1f);
        }

        public System.Collections.IEnumerator PlayEnemyTurn()
        {
            if (enemyAction == null)
                yield break;

            Debug.Log("Enemy Attack!");
            Deck.DeckManager.AddToCraftingSlot(false, enemyAction);
            CardExecutionManager.Instance.RegisterCard(enemyAction, false);
   

            yield return new WaitUntil(() => EnemyManager.EnemyAnimatorController.GetIsAnimationCurrentlyActive == false);
            EnemyManager.EnemyAnimatorController.ResetToStartingPosition();
        }
        public void AssignData(CharacterSO characterAbstSO)// enemySO. get struct stats from scriptable object
        {
            _cards = characterAbstSO.GetCharacterCards;
            _enemyStats = characterAbstSO.CharacterStats;
            _name = characterAbstSO.CharacterType;
            _difficultyLevel = characterAbstSO.CharacterDiffciulty;
            _cardAction = -1;
            enemyAction = null;


            


            UI.StatsUIManager.GetInstance.UpdateMaxHealthBar(false,_enemyStats.MaxHealth);
            UI.StatsUIManager.GetInstance.InitHealthBar(false,_enemyStats.Health);

            UI.StatsUIManager.GetInstance.UpdateShieldBar(false,_enemyStats.Shield);
            
        }
        #endregion

        #region Properties
        
        public ref Cards.Card[] GetCards => ref _cards;
        public CharacterDifficultyEnum GetDifficulty => _difficultyLevel;
        public CharacterTypeEnum GetOpponentName => _name;
        public ref  Characters.Stats.CharacterStats GetCharacterStats
        {
            get { 
                return ref _enemyStats;
            }
        }


        public ref Cards.Card GetOpponentCard
        {
            get
            {
                    _cardAction++;

                return ref _cards[_cardAction % _cards.Length];
            }
        }
        #endregion
    }
}
