using Battle.Characters;
using Battle.Deck;
using CardMaga.AI;
using CardMaga.Card;
using Characters.Stats;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Battle
{
    public class EnemyManager : MonoSingleton<EnemyManager>, IBattleHandler
    {
        #region Fields
        //   [UnityEngine.SerializeField] Opponents EnemyAI;

        [Tooltip("Player Stats: ")]
        [SerializeField] AIBrain _brain;
        [SerializeField] private Character _myCharacter;
        [SerializeField] AnimationBodyPartSoundsHandler _animationSoundHandler;
        [Space]
        private AIHand _aiHand;
        int _cardAction;
     //   [SerializeField] CardData enemyAction;
        [SerializeField] AnimatorController _enemyAnimatorController;
        [SerializeField] TextMeshProUGUI _enemyNameText;
        #endregion
        public Battle.Combo.Combo[] Recipes => _myCharacter.CharacterData.ComboRecipe;
        private CardData[] _deck;
        public CardData[] Deck => _deck;
        public ref CharacterStats GetCharacterStats => ref _myCharacter.CharacterData.CharacterStats;
        public static AnimatorController EnemyAnimatorController => Instance._enemyAnimatorController;

        public AIBrain Brain { get => _brain;}
        private bool _isStillThinking;
        private TokenMachine _aiTokenMachine;
        private IDisposable _turnFinished;

        [SerializeField, Sirenix.OdinInspector.MinMaxSlider(0, 10f)]
        private Vector2 _delayTime;
        #region Public Methods
        public override void Init(ITokenReciever token)
        {
            using (token.GetToken())
            {
                _aiHand = new AIHand(_brain, _myCharacter.CharacterData.CharacterStats.DrawCardsAmount);
                _aiTokenMachine = new TokenMachine(CalculateEnemyMoves,NoMoreActionsFound);
            }
        }


        public void RestartBattle()
        {

        }


        public void AssignCharacterData(Character character)
        {
            SpawnModel(character);
            _myCharacter = character;
            var characterdata = character.CharacterData;
            _animationSoundHandler.CurrentCharacter = characterdata.CharacterSO;

            int deckLength = characterdata.CharacterDeck.Length;
            _deck = new CardData[deckLength];
            System.Array.Copy(characterdata.CharacterDeck, _deck, deckLength);

            CharacterStatsManager.RegisterCharacterStats(false, ref characterdata.CharacterStats);
            DeckManager.Instance.InitDeck(false, _deck);

            EnemyAnimatorController.ResetAnimator();


#if UNITY_EDITOR
            _enemyNameText.gameObject.SetActive(true);
            _enemyNameText.text = characterdata.CharacterSO.CharacterName;
#endif

        }

        private void SpawnModel(Character character)
        {
            Instantiate(character.CharacterData.CharacterSO.CharacterAvatar, _enemyAnimatorController.transform);
        }

        public void UpdateStatsUI()
        {
            UI.StatsUIManager.Instance.UpdateMaxHealthBar(false, GetCharacterStats.MaxHealth);
            UI.StatsUIManager.Instance.InitHealthBar(false, GetCharacterStats.Health);
            UI.StatsUIManager.Instance.UpdateShieldBar(false, GetCharacterStats.Shield);
        }

        public void OnEndBattle()
        {
            NoMoreActionsFound();
        }


        public void CalculateEnemyMoves()
        {
            _aiHand.ResetData();
            var handCards = DeckManager.Instance.GetCardsFromDeck(false, DeckEnum.Hand);
            _aiHand.AddCard(handCards);
            ThreadsHandler.ThreadHandler.StartThread(new ThreadsHandler.ThreadList(ThreadsHandler.ThreadHandler.GetNewID, _aiHand.CalculateMove, DoAction));
        }


        public IEnumerator PlayTurnDelay()
        {
            const int NO_MORE_ACTION_TO_DO = -1;
            if (_aiHand.TryGetHighestWeight(out AICard card) > NO_MORE_ACTION_TO_DO)
            {
                DeckManager.Instance.TransferCard(false, DeckEnum.Hand, DeckEnum.Selected, card.Card);
                CardExecutionManager.Instance.TryExecuteCard(false, card.Card);
                yield return new WaitForSeconds(UnityEngine.Random.Range(_delayTime.x, _delayTime.y));
                CalculateEnemyMoves();
            }else
            _turnFinished.Dispose();
        }

        public void DoAction()
        {
            StartCoroutine(PlayTurnDelay());
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
        private void NoMoreActionsFound() => _isStillThinking = false;
        public IEnumerator PlayEnemyTurn()
        {
            Debug.Log("Enemy Attack!");

            _turnFinished = _aiTokenMachine.GetToken();
            _isStillThinking = true;
            while (_isStillThinking)
                yield return null;


            //int indexCheck = -1;
            //bool noMoreCardsAvailable = false;
            //do
            //{
            //    var handCards = DeckManager.Instance.GetCardsFromDeck(false, DeckEnum.Hand);
            //    bool isCardExecuted = false;
            //    do
            //    {
            //        yield return null;
            //        indexCheck++;
            //        if (indexCheck >= handCards.Length)
            //        {
            //            noMoreCardsAvailable = true;
            //            break;
            //        }

            //        enemyAction = handCards[indexCheck];

            //        if (enemyAction != null && staminaHandler.IsEnoughStamina(false, enemyAction))
            //            DeckManager.Instance.TransferCard(false, DeckEnum.Hand, DeckEnum.Selected, enemyAction);
            //      isCardExecuted = CardExecutionManager.Instance.CanPlayCard(false, enemyAction);

            //    } while (enemyAction == null || !isCardExecuted);

            //    if (isCardExecuted)
            //    {
            //        yield return new WaitForSeconds(Random.Range(_delayTime.x, _delayTime.y));
            //        CardExecutionManager.Instance.TryExecuteCard(false, enemyAction);
            //    }

            //    if (noMoreCardsAvailable == false)
            //    {
            //        //if (enemyAction.CardSO.CardTypeEnum == Cards.CardTypeEnum.Attack)
            //        //    yield return new WaitForSeconds(.3f);
            //        //else

            //        yield return Turns.Turn.WaitOneSecond;
            //    }

            //    indexCheck = -1;
            //} while (staminaHandler.HasStamina(false) && noMoreCardsAvailable == false);

      //   ThreadsHandler.ThreadHandler.StartThread(new ThreadsHandler.ThreadList(AIHand)


            yield return new WaitUntil(() => EnemyAnimatorController.GetIsAnimationCurrentlyActive == false && CardExecutionManager.CardsQueue.Count == 0);
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


    public class AIHand
    {
        public NodeState Result;
        private List<AICard> _card;
        private AITree _tree;
        public AIHand(AIBrain _brain, int drawAmount)
        {
            _card = new List<AICard>(drawAmount);
            for (int i = 0; i < drawAmount; i++)
                _card.Add(new AICard());
            _tree = new AITree(false,_brain);
        }

        public void CalculateMove()
        {
            for (int i = 0; i < _card.Count; i++)
            {
                if (_card[i].Card != null)
                    Result |= _tree.Evaluate(_card[i]);
            }
        }
       

        public void AddCard(CardData[] cardData)
        {
            for (int i = 0; i < cardData.Length; i++)
                AddCard(cardData[i]);
        }
        public void AddCard(CardData cardData)
        {
            for (int i = 0; i < _card.Count; i++)
            {
                if(_card[i].Card== null)
                {
                    _card[i].AssignCard(cardData);
                    return;
                }    
            }
            AICard aicard = new AICard();
            aicard.AssignCard(cardData);
            _card.Add(aicard);
        }
        public void ResetData() => _card.ForEach(x=>x.Reset());
        public int TryGetHighestWeight(out AICard highestCard)
        {
            highestCard = null;
            int highestWeight = -1;
            for (int i = 0; i < _card.Count; i++)
            {
                AICard current = _card[i];
                if (current.Card != null && highestWeight < current.Weight)
                {
                    highestWeight = current.Weight;
                    highestCard = current;
                }

            }
            return highestWeight;
        }
        ~AIHand() 
        {
        _card.Clear();
            _card = null;
        } 
    }
}
