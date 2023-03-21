using Battle;
using Battle.Characters;
using Battle.Deck;
using Battle.Turns;
using CardMaga.Card;
using CardMaga.Commands;
using Characters.Stats;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;

namespace CardMaga.Battle.Players
{
    public interface IPlayer
    {
        IReadOnlyList<TagSO> Tags { get; }
        bool IsLeft { get; }
        StaminaHandler StaminaHandler { get; }
        CharacterSO CharacterSO { get; }
        CharacterStatsHandler StatsHandler { get; }
        BattleCardData[] StartingCards { get; }
        DeckHandler DeckHandler { get; }
        PlayerComboContainer Combos { get; }
        EndTurnHandler EndTurnHandler { get; }
        GameTurn MyTurn { get; }
        CraftingHandler CraftingHandler { get; }
        void AssignCharacterData(IBattleManager battleManager, BattleCharacter characterData);
     
    }


    public class PlayerManager : IPlayer
    {
        #region Fields
        [Sirenix.OdinInspector.ReadOnly,Sirenix.OdinInspector.ShowInInspector]
        private CharacterStatsHandler _statsHandler;
        private EndTurnHandler _endTurnHandler;
        private CraftingHandler _craftingHandler;
        private GameTurn _myTurn;
        private DeckHandler _deckHandler;
        private BattleCharacter _character;
        private BattleCardData[] _playerDeck;
        private StaminaHandler _staminaHandler;
        private PlayerComboContainer _comboContainer;
        #endregion

        public BattleCardData[] StartingCards => _playerDeck;
        public bool IsLeft => true;
        public PlayerComboContainer Combos => _comboContainer;
        public CharacterSO CharacterSO => _character.CharacterData.CharacterSO;
        public CharacterStatsHandler StatsHandler => _statsHandler;
        public EndTurnHandler EndTurnHandler => _endTurnHandler;
        public DeckHandler DeckHandler => _deckHandler;
        public StaminaHandler StaminaHandler => _staminaHandler;
        public GameTurn MyTurn => _myTurn;
        public CraftingHandler CraftingHandler => _craftingHandler;
        public IReadOnlyList<TagSO> Tags => _character.Tags;

        public void AssignCharacterData(IBattleManager battleManager, BattleCharacter characterData)
        {
            battleManager.OnBattleManagerDestroyed += BeforeDestroy;
            _character = characterData;
            //data
            CharacterBattleData data = characterData.CharacterData;


            //Deck
            int Length = data.CharacterDeck.Length;
            _playerDeck = new BattleCardData[Length];
            Array.Copy(data.CharacterDeck, _playerDeck, Length);

            //CraftingSlots
            _craftingHandler = new CraftingHandler();

            //Stats
            _statsHandler = new CharacterStatsHandler(IsLeft, ref data.CharacterStats, _staminaHandler);

            //Stamina
            int stamina = _statsHandler.GetStat(Keywords.KeywordType.Stamina).Amount;
            int staminaShard = _statsHandler.GetStat(Keywords.KeywordType.StaminaShards).Amount;
            int startingStaminaAddition = battleManager.TurnHandler.IsLeftPlayerStart ? -1 : 0;
            _staminaHandler = new StaminaHandler(stamina, staminaShard, startingStaminaAddition);
  

            //Deck and Combos
            _deckHandler = new DeckHandler(this, battleManager);
            _comboContainer = new PlayerComboContainer(_character.CharacterData.ComboRecipe);

            TurnHandler turnHandler = battleManager.TurnHandler;
            _myTurn = turnHandler.GetCharacterTurn(IsLeft);


            _myTurn.StartTurnOperations.Register(StaminaHandler.StartTurn);
            _myTurn.EndTurnOperations.Register(StaminaHandler.EndTurn);

            _myTurn.OnTurnActive += DrawHands;


            //endturn
            _endTurnHandler = new EndTurnHandler(this, battleManager);
        }


    


        private void BeforeDestroy(IBattleManager battleManager)
        {
            _myTurn.OnTurnActive -= DrawHands;
            battleManager.OnBattleManagerDestroyed -= BeforeDestroy;
            _endTurnHandler.Dispose();
        }
        private void DrawHands()
            => DeckHandler.DrawHand(StatsHandler.GetStat(Keywords.KeywordType.Draw).Amount);

    }

}


