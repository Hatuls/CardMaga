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
        public ref Characters.Stats.CharacterStats GetCharacterStats => ref _characterStats;
        public static AnimatorController EnemyAnimatorController => Instance._enemyAnimatorController;
        #region Public Methods
        public override void Init()
        {
       
        }
        //public void SetEnemy(CharacterSO _character)
        //{
        //    if (EnemyAI != null && _character != null)
        //        EnemyAI.AssignData(_character);
        //}

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

        //public Opponents GetEnemyAction {
        //    get
        //    {
        //        if (EnemyAI == null)
        //        {
        //            Debug.LogError("Trying To Get Enemy Before Initializing the Enemy Manager First!");
        //        }

        //        return  EnemyAI;
        //    }
        //}


        public System.Collections.IEnumerator AssignNextCard()
        {
            enemyAction = DeckManager.Instance.GetCardFromDeck(false,0, DeckEnum.Hand);
            Debug.Log("<a>Enemy Next Move</a>: Is going to be: " + enemyAction.CardSO.CardName.ToString() +
            "\n This attack is going to use " + enemyAction.CardSO.BodyPartEnum.ToString() + "\n" +
            "And Do " + enemyAction.CardSO.CardTypeEnum.ToString() + " with the amount of " + enemyAction.CardSO.CardSOKeywords[0].GetAmountToApply);

            yield return new WaitForSeconds(.1f);
        }

        public System.Collections.IEnumerator PlayEnemyTurn()
        {

            yield return AssignNextCard();
            if (enemyAction == null)
                yield break;

            Debug.Log("Enemy Attack!");
            
           // DeckManager.AddToCraftingSlot(false, enemyAction);
            CardExecutionManager.Instance.RegisterCard(enemyAction, false);
            DeckManager.Instance.TransferCard(false, DeckEnum.Hand, DeckEnum.Disposal, enemyAction);

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

    //[System.Serializable]
    //public class Opponents
    //{
    //    #region Fields
     
    //    [SerializeField] CharacterDifficultyEnum _difficultyLevel;
    //    [SerializeField] CharacterTypeEnum _name;
    //    [SerializeField] Cards.Card[] _deck;
    //    [SerializeField] Characters.Stats.CharacterStats _enemyStats;
    //    [SerializeField] Cards.Card enemyAction;
    //    int _cardAction;
   
    //    #endregion

    //    public Opponents()
    //    {

    //    }
    //    public Cards.Card GetEnemyAction => enemyAction;

    //    #region Public Methods
    //    public System.Collections.IEnumerator AssignNextCard()
    //    {
    //        enemyAction = GetOpponentCard;
    //        Debug.Log("<a>Enemy Next Move</a>: Is going to be: " + enemyAction.CardSO.CardName.ToString() +
    //        "\n This attack is going to use " + enemyAction.CardSO.BodyPartEnum.ToString() + "\n" +
    //        "And Do " + enemyAction.CardSO.CardTypeEnum.ToString() + " with the amount of " + enemyAction.CardSO.CardSOKeywords[0].GetAmountToApply);

    //        yield return new WaitForSeconds(.1f);
    //    }

    //    public System.Collections.IEnumerator PlayEnemyTurn()
    //    {
    //        if (enemyAction == null)
    //            yield break;

    //        Debug.Log("Enemy Attack!");
    //        Deck.DeckManager.AddToCraftingSlot(false, enemyAction);
    //        CardExecutionManager.Instance.RegisterCard(enemyAction, false);
   

    //        yield return new WaitUntil(() => EnemyManager.EnemyAnimatorController.GetIsAnimationCurrentlyActive == false);
    //        EnemyManager.EnemyAnimatorController.ResetToStartingPosition();
    //    }
    //    public void AssignData(CharacterSO characterAbstSO)// enemySO. get struct stats from scriptable object
    //    {
    //        _cards = characterAbstSO.GetCharacterCards;
    //        _enemyStats = characterAbstSO.CharacterStats;
    //        _name = characterAbstSO.CharacterType;
    //        _difficultyLevel = characterAbstSO.CharacterDiffciulty;
    //        _cardAction = -1;
    //        enemyAction = null;


            


    //        UI.StatsUIManager.GetInstance.UpdateMaxHealthBar(false,_enemyStats.MaxHealth);
    //        UI.StatsUIManager.GetInstance.InitHealthBar(false,_enemyStats.Health);

    //        UI.StatsUIManager.GetInstance.UpdateShieldBar(false,_enemyStats.Shield);
            
    //    }
    //    #endregion

    //    #region Properties
        
    //    public ref Cards.Card[] GetCards => ref _cards;
    //    public CharacterDifficultyEnum GetDifficulty => _difficultyLevel;
    //    public CharacterTypeEnum GetOpponentName => _name;
    //    public ref  Characters.Stats.CharacterStats GetCharacterStats
    //    {
    //        get { 
    //            return ref _enemyStats;
    //        }
    //    }


    //    public Cards.Card GetOpponentCard
    //    {
    //        get
    //        {
    //                _cardAction++;

    //            return _cards[_cardAction % _cards.Length];
    //        }
    //    }
    //    #endregion
  //  }
}
