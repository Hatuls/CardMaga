using UnityEngine;
namespace Battles
{
    public class EnemyManager : MonoSingleton<EnemyManager> 
    {
        #region Fields
        [UnityEngine.SerializeField] Opponents _opponent;
        [SerializeField] BuffIconsHandler _uiBuffIconHandler;
        #endregion


        #region Public Methods
        public override void Init()
        {
            if (_opponent == null)
            _opponent = new Opponents(_uiBuffIconHandler);
        }
        public void SetEnemy(CharacterAbstSO _character)
        {
            if (_opponent != null && _character != null)
                _opponent.AssignData(_character);
        }

  

        public Opponents GetEnemy {
            get
            {
                if (_opponent == null)
                {
                    _opponent = new Opponents(_uiBuffIconHandler);
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
        [SerializeField]   AnimatorController _enemyAnimatorController;
        [SerializeField] CharacterDifficulty _difficultyLevel;
        [SerializeField] CharactersEnum _name;
        [SerializeField] Cards.Card[] _cards;
        [SerializeField] Characters.Stats.CharacterStats _enemyStats;
        [SerializeField] Cards.Card enemyAction;
        int _cardAction;
    [SerializeField]    BuffIconsHandler _buffIconsHandler;
        #endregion

        public Opponents(BuffIconsHandler buffIconsHandler)
        {
            this._buffIconsHandler = buffIconsHandler;
        }
        public Cards.Card GetEnemyAction => enemyAction;

        #region Public Methods
        public System.Collections.IEnumerator AssignNextCard()
        {
            enemyAction = GetOpponentCard;
            Debug.Log("<a>Enemy Next Move</a>: Is going to be: " + enemyAction.GetSetCard.GetCardName.ToString() +
            "\n This attack is going to use " + enemyAction.GetSetCard.GetBodyPartEnum.ToString() + "\n" +
            "And Do " + enemyAction.GetSetCard.GetCardTypeEnum.ToString() + " with the amount of " + enemyAction.GetSetCard.GetCardsKeywords[0].GetAmountToApply);

            _buffIconsHandler?.SetOpponentActionUI(enemyAction);
            //Battles.UI.CardUIManager.Instance.AssignEnemyActionOnSlot(enemyAction);

            yield return new WaitForSeconds(.1f);
        }

        public System.Collections.IEnumerator PlayEnemyTurn()
        {
            if (enemyAction == null)
                yield break;

            Debug.Log("Enemy Attack!");
            _enemyAnimatorController.PlayAnimation(enemyAction.GetSetCard.GetCardName);
            for (int i = 0; i < enemyAction.GetCardKeywords.Length; i++)
            {
                Keywords.KeywordManager.Instance.ActivateKeyword(enemyAction.GetCardKeywords[i]);
                yield return null;
            }

            yield return new WaitUntil(() => _enemyAnimatorController.GetIsAnimationCurrentlyActive == false);
            _enemyAnimatorController.ResetToStartingPosition();
        }
        public void AssignData(CharacterAbstSO characterAbstSO)// enemySO. get struct stats from scriptable object
        {
            _cards = characterAbstSO.GetCharacterCards;
            _enemyStats = characterAbstSO.GetCharacterStats;
            _name = characterAbstSO.GetOpponent;
            _difficultyLevel = characterAbstSO.GetDifficulty;
            _cardAction = -1;
            enemyAction = null;


            


            UI.StatsUIManager.GetInstance.UpdateMaxHealthBar(false,_enemyStats.MaxHealth);
            UI.StatsUIManager.GetInstance.UpdateHealthBar(false,_enemyStats.Health);

            UI.StatsUIManager.GetInstance.UpdateMaxShieldBar(false,_enemyStats.MaxHealth/4);     
            UI.StatsUIManager.GetInstance.UpdateShieldBar(false,_enemyStats.Shield);
            
        }
        #endregion

        #region Properties
        public AnimatorController GetEnemyAnimatorController => _enemyAnimatorController;
        public ref Cards.Card[] GetCards => ref _cards;
        public CharacterDifficulty GetDifficulty => _difficultyLevel;
        public CharactersEnum GetOpponentName => _name;
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
