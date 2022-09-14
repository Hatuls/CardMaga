
using Battle;
using Battle.Characters;
using Battle.Combo;
using Battle.Deck;
using Battle.Turns;
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
        CharacterStatsHandler StatsHandler { get; }
        CardData[] StartingCards { get; }
        DeckHandler DeckHandler { get; }
        Combo[] Combos { get; }
        VisualCharacter VisualCharacter { get; }
        GameTurn MyTurn { get; }
        CraftingHandler CraftingHandler { get; }
        void AssignCharacterData(BattleManager battleManager, Character characterData);
    }


    public class PlayerManager : MonoSingleton<PlayerManager>, IPlayer
    {
        #region Fields
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
        public Combo[] Combos => _character.CharacterData.ComboRecipe;
        public bool IsLeft => true;
        public AnimatorController AnimatorController => VisualCharacter.AnimatorController;

        public CharacterStatsHandler StatsHandler { get => _statsHandler; }

        public VisualCharacter VisualCharacter => _visualCharacter;

        public DeckHandler DeckHandler => _deckHandler;

        public StaminaHandler StaminaHandler => _staminaHandler;

        public GameTurn MyTurn => _myTurn;

        public CraftingHandler CraftingHandler => _craftingHandler;

        public void AssignCharacterData(BattleManager battleManager,Character characterData)
        {
            battleManager.OnBattleManagerDestroyed += BeforeDestroy;
            _character = characterData;
            var data = characterData.CharacterData;
            VisualCharacter.AnimationSound.CurrentCharacter = data.CharacterSO;

            int Length = data.CharacterDeck.Length;

            _playerDeck = new CardData[Length];
            Array.Copy(data.CharacterDeck, _playerDeck, Length);
            _craftingHandler = new CraftingHandler();
            _statsHandler = new CharacterStatsHandler(IsLeft, ref data.CharacterStats, _staminaHandler);
            _staminaHandler = new StaminaHandler(_statsHandler.GetStats(Keywords.KeywordTypeEnum.Stamina).Amount, _statsHandler.GetStats(Keywords.KeywordTypeEnum.StaminaShards).Amount);
            _deckHandler = new DeckHandler(this, battleManager);

       
           GameTurnHandler turnHandler = battleManager.TurnHandler;
            _myTurn = turnHandler.GetCharacterTurn(IsLeft);
            _staminaHandler.OnStaminaDepleted += turnHandler.MoveToNextTurn;
            _myTurn.StartTurnOperations.Register(DrawHands);
            _myTurn.StartTurnOperations.Register(StaminaHandler.StartTurn);

            _myTurn.EndTurnOperations.Register(StaminaHandler.EndTurn);


        }


        public void OnEndTurn()
            => VisualCharacter.AnimatorController.ResetLayerWeight();

        public void PlayerWin()
        {
            VisualCharacter.AnimatorController.CharacterWon();
            _character.CharacterData.CharacterSO.VictorySound.PlaySound();
        }


        private void BeforeDestroy(BattleManager battleManager)
        {
            battleManager.OnBattleManagerDestroyed -= BeforeDestroy;
            
            _staminaHandler.OnStaminaDepleted -= battleManager.TurnHandler.MoveToNextTurn;
        }
        private void DrawHands(ITokenReciever tokenMachine)
            => DeckHandler.DrawHand(StatsHandler.GetStats(Keywords.KeywordTypeEnum.Draw).Amount);    

    }

}
