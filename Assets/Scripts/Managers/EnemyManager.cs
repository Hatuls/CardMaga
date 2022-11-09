using Battle.Characters;
using Battle.Deck;
using Battle.Turns;
using CardMaga.AI;
using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Battle.Visual;
using CardMaga.Card;
using CardMaga.Commands;
using CardMaga.Keywords;
using Characters.Stats;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Battle
{
    public class EnemyManager : MonoSingleton<EnemyManager>, IPlayer
    {
        #region Fields
        //   [UnityEngine.SerializeField] Opponents EnemyAI;

        [Tooltip("LeftPlayer Stats: ")]
        [SerializeField] AIBrain _brain;
        [FormerlySerializedAs("_myCharacter")] [SerializeField] private BattleCharacter _character;
        [SerializeField] VisualCharacter _visualCharacter;
        [SerializeField] TextMeshProUGUI _enemyNameText;
        [Space]
        int _cardAction;
        private TurnHandler _turnHandler;
        private DeckHandler _deckHandler;
        private CraftingHandler _craftingHandler;
        private GameTurn _myTurn;
        private StaminaHandler _staminaHandler;
        private CharacterStatsHandler _statsHandler;
        private BattleCardData[] _deck;
        private AIHand _aiHand;
        private EndTurnHandler _endTurnHandler;
        private GameCommands _gameCommands;
        private PlayerComboContainer _comboContainer;
        private CardExecutionManager _cardExecutionManager;
        private TokenMachine _aiTokenMachine;
        private IDisposable _turnFinished;

        [SerializeField, Sirenix.OdinInspector.MinMaxSlider(0, 10f, true)]
        private Vector2 _delayTime;
        #endregion

        #region Properties
        public bool IsLeft => false;
        public VisualCharacter VisualCharacter => _visualCharacter;
        public BattleCardData[] StartingCards => _deck;
        public AIBrain Brain => _brain;
        public CharacterSO CharacterSO => _character.CharacterData.CharacterSO;
        public CharacterStatsHandler StatsHandler => _statsHandler;
        public DeckHandler DeckHandler => _deckHandler;
        //    public Battle.Combo.ComboData[] Combos => _character.CharacterData.ComboRecipe;

        public GameTurn MyTurn => _myTurn;
        public StaminaHandler StaminaHandler => _staminaHandler;
        public EndTurnHandler EndTurnHandler => _endTurnHandler;
        public CraftingHandler CraftingHandler => _craftingHandler;

        public PlayerComboContainer Combos => _comboContainer;

        public IReadOnlyList<PlayerTagSO> PlayerTags => _character.PlayerTags;

        #endregion

        #region Public Methods

        public void AssignCharacterData(IBattleManager battleManager, BattleCharacter character)
        {
            battleManager.OnBattleManagerDestroyed += BeforeGameExit;
            _character = character;

            //data from battledata
            CharacterBattleData characterdata = character.CharacterData;
            //Deck
            int deckLength = characterdata.CharacterDeck.Length;
            _deck = new BattleCardData[deckLength];
            Array.Copy(characterdata.CharacterDeck, _deck, deckLength);
            _deckHandler = new DeckHandler(this, battleManager);


            _comboContainer = new PlayerComboContainer(_character.CharacterData.ComboRecipe);
            //CraftingSlots
            _craftingHandler = new CraftingHandler();

            //Stats
            _statsHandler = new CharacterStatsHandler(IsLeft, ref characterdata.CharacterStats, StaminaHandler);

            //Stamina
            int additionStamina = !battleManager.TurnHandler.IsLeftPlayerStart ? -1 : 0;
            _staminaHandler = new StaminaHandler(_statsHandler.GetStat(KeywordType.Stamina).Amount, _statsHandler.GetStat(KeywordType.StaminaShards).Amount, additionStamina);
            //Game Commands
            _gameCommands = battleManager.GameCommands;

            //Execution
            _cardExecutionManager = battleManager.CardExecutionManager;
            //Turn
            _turnHandler = battleManager.TurnHandler;
            _myTurn = _turnHandler.GetCharacterTurn(IsLeft);
            _myTurn.StartTurnOperations.Register(StaminaHandler.StartTurn);
            _myTurn.StartTurnOperations.Register(DrawHands);
            _myTurn.OnTurnActive += PlayEnemyTurn;
            _myTurn.EndTurnOperations.Register(StaminaHandler.EndTurn);
            //AI 
            _aiHand = new AIHand(_brain, StatsHandler.GetStat(KeywordType.Draw));
            _aiTokenMachine = new TokenMachine(CalculateEnemyMoves, FinishTurn);

            _endTurnHandler = new EndTurnHandler(this, battleManager);
        }

        private void BeforeGameExit(IBattleManager obj)
        {
            obj.OnBattleManagerDestroyed -= BeforeGameExit;
            _endTurnHandler.Dispose();

            _myTurn.OnTurnActive -= CalculateEnemyMoves;
            _myTurn = null;
        }

        public void OnEndBattle()
        {
            FinishTurn();
        }

        public void CalculateEnemyMoves()
        {
            _aiHand.ResetData();
            var handCards = DeckHandler.GetCardsFromDeck(DeckEnum.Hand);
            _aiHand.AddCard(handCards);
            try
            {
                ThreadsHandler.ThreadHandler.StartThread(new ThreadsHandler.ThreadList(ThreadsHandler.ThreadHandler.GetNewID, _aiHand.CalculateMove, DoAction));
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                _turnFinished?.Dispose();
            }
        }

        public IEnumerator PlayTurnDelay()
        {
            const int NO_MORE_ACTION_TO_DO = -1;

            if (_aiHand.TryGetHighestWeight(out AICard card) > NO_MORE_ACTION_TO_DO && !BattleManager.isGameEnded)
            {
                _gameCommands.GameDataCommands.DataCommands.AddCommand(new TransferSingleCardCommand(DeckHandler, DeckEnum.Hand, DeckEnum.Selected, card.BattleCard));
                //_deckHandler.TransferCard(DeckEnum.Hand, DeckEnum.Selected, battleCard.BattleCard);
                _cardExecutionManager.TryExecuteCard(IsLeft, card.BattleCard);
                yield return new WaitForSeconds(UnityEngine.Random.Range(_delayTime.x, _delayTime.y));
                CalculateEnemyMoves();
            }
            else
                _turnFinished?.Dispose();
        }

        public void DoAction()
        {
            if(gameObject.activeSelf)
            StartCoroutine(PlayTurnDelay());
        }
        public void EnemyWon()
        {
            VisualCharacter.AnimatorController.CharacterWon();
            _character.CharacterData.CharacterSO.VictorySound.PlaySound();
        }

        public void OnEndTurn()
        {
            VisualCharacter.AnimatorController.ResetLayerWeight();
        }


        public void PlayEnemyTurn()
        {
            Debug.Log("RightPlayer Attack!");

            _turnFinished = _aiTokenMachine.GetToken();
        }

        #endregion

        #region Private 
        private void DrawHands(ITokenReciever tokenMachine)
    => DeckHandler.DrawHand(StatsHandler.GetStat(KeywordType.Draw).Amount);
        private void FinishTurn()
        {
            _endTurnHandler.EndTurnPressed();
            VisualCharacter.AnimatorController.ResetToStartingPosition();
        }


        #endregion
    }

    public class AIHand
    {
        public NodeState Result;
        private List<AICard> _card;
        private AITree _tree;
        private BaseStat _drawStat;

        public AIHand(AIBrain _brain, BaseStat drawStat)
        {
            _drawStat = drawStat;
            int drawAmount = _drawStat.Amount;
            _card = new List<AICard>(drawAmount);
            for (int i = 0; i < drawAmount; i++)
                _card.Add(new AICard());
            _tree = new AITree(false, _brain);
        }

        public void CalculateMove()
        {
            for (int i = 0; i < _card.Count; i++)
            {
                if (_card[i].BattleCard != null && !BattleManager.isGameEnded)
                    Result |= _tree.Evaluate(_card[i]);
            }
        }

        public void AddCard(BattleCardData[] cardData)
        {
            for (int i = 0; i < cardData.Length; i++)
                AddCard(cardData[i]);
        }

        public void AddCard(BattleCardData battleCardData)
        {
            for (int i = 0; i < _card.Count; i++)
            {
                if (_card[i].BattleCard == null)
                {
                    _card[i].AssignCard(battleCardData);
                    return;
                }
            }
            AICard aicard = new AICard();
            aicard.AssignCard(battleCardData);
            _card.Add(aicard);
        }

        public void ResetData() => _card.ForEach(x => x.Reset());

        public int TryGetHighestWeight(out AICard highestCard)
        {
            highestCard = null;
            int highestWeight = -1;
            for (int i = 0; i < _card.Count; i++)
            {
                AICard current = _card[i];
                if (current.BattleCard != null && highestWeight < current.Weight)
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
