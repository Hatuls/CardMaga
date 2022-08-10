using Battle.Characters;
using Battle.Deck;
using CardMaga.AI;
using CardMaga.Battle.UI;
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
        [SerializeField] AnimationBodyPartSoundsHandler _animationSoundHandler;
        [Space]
        private AIHand _aiHand;
        int _cardAction;
        //   [SerializeField] CardData enemyAction;
        [SerializeField] AnimatorController _enemyAnimatorController;
        [SerializeField] TextMeshProUGUI _enemyNameText;
        #endregion
        public Battle.Combo.Combo[] Combos => _myCharacter.CharacterData.ComboRecipe;
        private CardData[] _deck;
        public CardData[] Deck => _deck;
        public ref CharacterStats GetCharacterStats => ref _myCharacter.CharacterData.CharacterStats;
        public AnimatorController EnemyAnimatorController => _enemyAnimatorController;

        public AIBrain Brain { get => _brain; }
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
                _aiTokenMachine = new TokenMachine(CalculateEnemyMoves, NoMoreActionsFound);
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
            //   _enemyNameText.gameObject.SetActive(true);
            //   _enemyNameText.text = characterdata.CharacterSO.CharacterName;
#endif

        }

        private void SpawnModel(Character character)
        {
            Instantiate(character.CharacterData.CharacterSO.CharacterAvatar, _enemyAnimatorController.transform);
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
                DeckManager.Instance.TransferCard(false, DeckEnum.Hand, DeckEnum.Selected, card.Card);
                CardExecutionManager.Instance.TryExecuteCard(false, card.Card);
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

            yield return new WaitUntil(() => EnemyAnimatorController.GetIsAnimationCurrentlyActive == false && CardExecutionManager.CardsQueue.Count == 0);
            CardUIManager.Instance.ActivateEnemyCardUI(false);
            yield return Turns.Turn.WaitOneSecond;
            EnemyAnimatorController.ResetToStartingPosition();
        }


        #endregion

        #region Monobehaviour Callbacks 
        public override void Awake()
        {
            base.Awake();
            const int order = 5;
            SceneStarter.Register(new OperationTask(Init, order));
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
