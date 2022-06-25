using Managers;
using UnityEngine;
using Battles.Deck;
using Characters.Stats;
using Characters;
using TMPro;
using ReiTools.TokenMachine;

namespace Battles
{
    public class EnemyManager : MonoSingleton<EnemyManager> , IBattleHandler
    {
        #region Fields
     //   [UnityEngine.SerializeField] Opponents EnemyAI;

        [Tooltip("Player Stats: ")]

        [SerializeField] private  Character _myCharacter;
        [SerializeField] AnimationBodyPartSoundsHandler _animationSoundHandler;
        [Space]

        int _cardAction;
        [SerializeField] Cards.Card enemyAction;
        [SerializeField]  AnimatorController _enemyAnimatorController;
        [SerializeField] TextMeshProUGUI _enemyNameText;
        #endregion
         public Combo.Combo[] Recipes => _myCharacter.CharacterData.ComboRecipe;
        private Cards.Card[] _deck;
        public Cards.Card[] Deck => _deck;
        public ref CharacterStats GetCharacterStats => ref _myCharacter.CharacterData.CharacterStats;
        public static AnimatorController EnemyAnimatorController => Instance._enemyAnimatorController;

        [SerializeField,Sirenix.OdinInspector.MinMaxSlider(0,10f)]
        private Vector2 _delayTime;
        #region Public Methods
        public override void Init(ITokenReciever token)
        {
        
        }


        public void RestartBattle()
        {

        }

 
        public void AssignCharacterData(Character character)
        {
            Instantiate(character.CharacterData.CharacterSO.CharacterAvatar, _enemyAnimatorController.transform);
            _myCharacter = character;
            var characterdata = character.CharacterData;
            _animationSoundHandler.CurrentCharacter = characterdata.CharacterSO;
     
            int deckLength = characterdata.CharacterDeck.Length;
            _deck = new Cards.Card[deckLength];
            System.Array.Copy(characterdata.CharacterDeck, _deck, deckLength);

            CharacterStatsManager.RegisterCharacterStats(false, ref characterdata.CharacterStats);
            DeckManager.Instance.InitDeck(false, _deck);

            EnemyAnimatorController.ResetAnimator();


#if UNITY_EDITOR
            _enemyNameText.gameObject.SetActive(true);
            _enemyNameText.text = characterdata.CharacterSO.CharacterName;
#endif

        }
        public void UpdateStatsUI()
        {
            UI.StatsUIManager.Instance.UpdateMaxHealthBar(false, GetCharacterStats.MaxHealth);
            UI.StatsUIManager.Instance.InitHealthBar(false, GetCharacterStats.Health);
            UI.StatsUIManager.Instance.UpdateShieldBar(false, GetCharacterStats.Shield);
        }

        public void OnEndBattle()
        {

        }


        public void CalculateEnemyMoves()
        {
            //ThreadsHandler.ThreadHandler.StartThread(new ThreadsHandler.ThreadList(ThreadsHandler.ThreadHandler.GetNewID,))

        }
        public void EnemyWon()
        {
            _enemyAnimatorController.CharacterWon();
            _myCharacter.CharacterData.CharacterSO.VictorySound.PlaySound();
        }

        public void OnEndTurn()
        {
            _enemyAnimatorController.ResetLayerWeight();
 
        }
        public System.Collections.IEnumerator PlayEnemyTurn()
        {
            Debug.Log("Enemy Attack!");

            var staminaHandler = Characters.Stats.StaminaHandler.Instance;



            int indexCheck = -1;
            bool noMoreCardsAvailable = false;
            do
            {
                var handCards = DeckManager.Instance.GetCardsFromDeck(false, DeckEnum.Hand);
                bool isCardExecuted = false;
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
            
                    if (enemyAction!= null && staminaHandler.IsEnoughStamina(false, enemyAction))
                        DeckManager.Instance.TransferCard(false, DeckEnum.Hand, DeckEnum.Selected, enemyAction);
                    isCardExecuted = CardExecutionManager.Instance.CanPlayCard(false, enemyAction);
         
                } while (enemyAction == null || !isCardExecuted);

                if (isCardExecuted)
                {
                    yield return new WaitForSeconds(Random.Range(_delayTime.x,_delayTime.y));
                    CardExecutionManager.Instance.TryExecuteCard(false, enemyAction);
                }

                if (noMoreCardsAvailable == false)
                {
                    //if (enemyAction.CardSO.CardTypeEnum == Cards.CardTypeEnum.Attack)
                    //    yield return new WaitForSeconds(.3f);
                    //else
                 
                      yield return Turns.Turn.WaitOneSecond;
                }

                indexCheck = -1;
            } while (staminaHandler.HasStamina(false) && noMoreCardsAvailable == false);



            yield return new WaitUntil(() => EnemyAnimatorController.GetIsAnimationCurrentlyActive == false && CardExecutionManager.CardsQueue.Count ==0);
            UI.CardUIManager.Instance.ActivateEnemyCardUI(false);
            yield return Turns.Turn.WaitOneSecond;
            EnemyAnimatorController.ResetToStartingPosition();
        }


        #endregion


        #region Monobehaviour Callbacks 
        public override void Awake()
        {
            base.Awake();
            SceneHandler.OnBeforeSceneShown += Init;
        }
        public void OnDestroy()
        {
            SceneHandler.OnBeforeSceneShown -= Init;
        }
        #endregion
    }


}
