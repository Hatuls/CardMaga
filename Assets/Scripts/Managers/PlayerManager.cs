
using Battle;
using Battle.Characters;
using Battle.Combo;
using Battle.Deck;
using Battle.Turns;
using CardMaga.Battle.Visual;
using CardMaga.Card;
using Characters.Stats;
using ReiTools.TokenMachine;
using System;
using UnityEngine;

namespace Managers
{
    public interface IPlayer
    {
        bool IsLeft { get; }
        StaminaHandler StaminaHandler { get; }
        CharacterSO CharacterSO { get; }
        CharacterStatsHandler StatsHandler { get; }
        CardData[] StartingCards { get; }
        DeckHandler DeckHandler { get; }
        ComboData[] Combos { get; }
        
        EndTurnHandler EndTurnHandler { get; }
        VisualCharacter VisualCharacter { get; }
        GameTurn MyTurn { get; }
        CraftingHandler CraftingHandler { get; }
        void AssignCharacterData(IBattleManager battleManager, Character characterData);
    }


    public class PlayerManager : MonoSingleton<PlayerManager>, IPlayer
    {
        #region Fields

        private EndTurnHandler _endTurnHandler;
        [Sirenix.OdinInspector.ShowInInspector]
        private CraftingHandler _craftingHandler;
        private GameTurn _myTurn;
        private DeckHandler _deckHandler;
        private Character _character;
        private CharacterStatsHandler _statsHandler;
        private CardData[] _playerDeck;
        private StaminaHandler _staminaHandler;
        [SerializeField] VisualCharacter _visualCharacter;
        #endregion
        
        public CardData[] StartingCards => _playerDeck;
        public ComboData[] Combos => _character.CharacterData.ComboRecipe;
        public bool IsLeft => true;
        public AnimatorController AnimatorController => VisualCharacter.AnimatorController;

        public CharacterSO CharacterSO => _character.CharacterData.CharacterSO;
        public CharacterStatsHandler StatsHandler { get => _statsHandler; }
        public EndTurnHandler EndTurnHandler => _endTurnHandler;
        public VisualCharacter VisualCharacter => _visualCharacter;

        public DeckHandler DeckHandler => _deckHandler;

        public StaminaHandler StaminaHandler => _staminaHandler;

        public GameTurn MyTurn => _myTurn;

        public CraftingHandler CraftingHandler => _craftingHandler;
        public void AssignCharacterData(IBattleManager battleManager, Character characterData)
        {
            battleManager.OnBattleManagerDestroyed += BeforeDestroy;
            _character = characterData;
            //data
            CharacterBattleData data = characterData.CharacterData;

            //Visuals
            VisualCharacter.AnimationSound.CurrentCharacter = data.CharacterSO;
            //Deck
            int Length = data.CharacterDeck.Length;
            _playerDeck = new CardData[Length];
            Array.Copy(data.CharacterDeck, _playerDeck, Length);
            
            //CraftingSlots
            _craftingHandler = new CraftingHandler();

            //Stats
            _statsHandler = new CharacterStatsHandler(IsLeft, ref data.CharacterStats, _staminaHandler);

            //Stamina
            if (battleManager.TurnHandler.IsLeftPlayerStart)
                _staminaHandler = new StaminaHandler(_statsHandler.GetStat(Keywords.KeywordTypeEnum.Stamina).Amount, _statsHandler.GetStat(Keywords.KeywordTypeEnum.StaminaShards).Amount,-1);
            else
                _staminaHandler = new StaminaHandler(_statsHandler.GetStat(Keywords.KeywordTypeEnum.Stamina).Amount, _statsHandler.GetStat(Keywords.KeywordTypeEnum.StaminaShards).Amount);
            
            //Deck
            _deckHandler = new DeckHandler(this, battleManager);

            GameTurnHandler turnHandler = battleManager.TurnHandler;
            _myTurn = turnHandler.GetCharacterTurn(IsLeft);

            _myTurn.StartTurnOperations.Register(DrawHands);
            _myTurn.StartTurnOperations.Register(StaminaHandler.StartTurn);

            _myTurn.EndTurnOperations.Register(StaminaHandler.EndTurn);

            

            //endturn
            _endTurnHandler = new EndTurnHandler(this, battleManager);
        }


        public void OnEndTurn()
            => VisualCharacter.AnimatorController.ResetLayerWeight();

        public void PlayerWin()
        {
            VisualCharacter.AnimatorController.CharacterWon();
            _character.CharacterData.CharacterSO.VictorySound.PlaySound();
        }


        private void BeforeDestroy(IBattleManager battleManager)
        {
            battleManager.OnBattleManagerDestroyed -= BeforeDestroy;
            _endTurnHandler.Dispose();
        }
        private void DrawHands(ITokenReciever tokenMachine)
            => DeckHandler.DrawHand(StatsHandler.GetStat(Keywords.KeywordTypeEnum.Draw).Amount);

    }

}
