using Battle.Characters;
using Battle.Deck;
using Battle.Turns;
using CardMaga.AI;
using CardMaga.Card;
using Characters.Stats;
using Managers;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Battle
{
    public class EnemyManager : MonoSingleton<EnemyManager>, IPlayer
    {
        #region Fields
        //   [UnityEngine.SerializeField] Opponents EnemyAI;

        [Tooltip("Player Stats: ")]
        [SerializeField] AIBrain _brain;
        [SerializeField] private Character _myCharacter;
        [SerializeField] VisualCharacter _visualCharacter;
        [SerializeField] TextMeshProUGUI _enemyNameText;
        [Space]
        int _cardAction;
        private GameTurnHandler _turnHandler;
        private DeckHandler _deckHandler;


        private StaminaHandler _staminaHandler;
        private CharacterStatsHandler _statsHandler;
        private CardData[] _deck;
        private AIHand _aiHand;




        //   private bool _isStillThinking;
        private TokenMachine _aiTokenMachine;
        private IDisposable _turnFinished;

        [SerializeField, Sirenix.OdinInspector.MinMaxSlider(0, 10f, true)]
        private Vector2 _delayTime;
        #endregion

        #region Properties
        public bool IsLeft => false;
        public VisualCharacter VisualCharacter => _visualCharacter;
        public CardData[] StartingCards => _deck;
        public AIBrain Brain => _brain;
        public CharacterStatsHandler StatsHandler => _statsHandler;
        public DeckHandler DeckHandler => _deckHandler;
        public Battle.Combo.Combo[] Combos => _myCharacter.CharacterData.ComboRecipe;
        public AnimatorController AnimatorController => VisualCharacter.AnimatorController;

        public StaminaHandler StaminaHandler => _staminaHandler;
        #endregion
        #region Public Methods





        public void AssignCharacterData(BattleManager battleManager, Character character)
        {
            _myCharacter = character;
            var characterdata = character.CharacterData;

            int deckLength = characterdata.CharacterDeck.Length;
            _deck = new CardData[deckLength];
            Array.Copy(characterdata.CharacterDeck, _deck, deckLength);

            _deckHandler = new DeckHandler(this, battleManager);
            _statsHandler = new CharacterStatsHandler(IsLeft, ref characterdata.CharacterStats, StaminaHandler);
            _staminaHandler = new StaminaHandler(_statsHandler.GetStats(Keywords.KeywordTypeEnum.Stamina).Amount, _statsHandler.GetStats(Keywords.KeywordTypeEnum.StaminaShards).Amount);

            _aiHand = new AIHand(_brain, StatsHandler.GetStats(Keywords.KeywordTypeEnum.Draw).Amount);


            _turnHandler = battleManager.TurnHandler;
            GameTurn turn = _turnHandler.GetCharacterTurn(IsLeft);
            turn.StartTurnOperations.Register(StaminaHandler.StartTurn);
            turn.StartTurnOperations.Register(DrawHands);
            turn.OnTurnActive += PlayEnemyTurn;
            turn.EndTurnOperations.Register(StaminaHandler.EndTurn);
  
            _aiTokenMachine = new TokenMachine(CalculateEnemyMoves, FinishTurn);
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
                _deckHandler.TransferCard(DeckEnum.Hand, DeckEnum.Selected, card.Card);
                CardExecutionManager.Instance.TryExecuteCard(IsLeft, card.Card);
                yield return new WaitForSeconds(UnityEngine.Random.Range(_delayTime.x, _delayTime.y));
                CalculateEnemyMoves();
            }
            else
                _turnFinished?.Dispose();
        }

        public void DoAction()
        {
            StartCoroutine(PlayTurnDelay());
        }
        public void EnemyWon()
        {
            VisualCharacter.AnimatorController.CharacterWon();
            _myCharacter.CharacterData.CharacterSO.VictorySound.PlaySound();
        }

        public void OnEndTurn()
        {
            VisualCharacter.AnimatorController.ResetLayerWeight();

        }


        public void PlayEnemyTurn()
        {
            Debug.Log("Enemy Attack!");

            _turnFinished = _aiTokenMachine.GetToken();
        }

        #endregion
        #region Private 
        private void DrawHands(ITokenReciever tokenMachine)
    => DeckHandler.DrawHand(StatsHandler.GetStats(Keywords.KeywordTypeEnum.Draw).Amount);
        private void FinishTurn()
        {
            _turnHandler.MoveToNextTurn();
            AnimatorController.ResetToStartingPosition();
        }

        private void OnDestroy()
        {
            _turnHandler.GetCharacterTurn(IsLeft).OnTurnActive -= CalculateEnemyMoves;

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
            _tree = new AITree(false, _brain);
        }

        public void CalculateMove()
        {
            for (int i = 0; i < _card.Count; i++)
            {
                if (_card[i].Card != null && !BattleManager.isGameEnded)
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
                if (_card[i].Card == null)
                {
                    _card[i].AssignCard(cardData);
                    return;
                }
            }
            AICard aicard = new AICard();
            aicard.AssignCard(cardData);
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
