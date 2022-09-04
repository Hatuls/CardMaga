
using Battle;
using Battle.Characters;
using Battle.Combo;
using Battle.Deck;
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
        CharacterStatsHandler StatsHandler { get; }
        CardData[] StartingCards { get; }
        DeckHandler DeckHandler { get; }
        Combo[] Combos { get; }
        VisualCharacter VisualCharacter { get; }
        void AssignCharacterData(BattleManager battleManager, Character characterData);
    }


    public class PlayerManager : MonoSingleton<PlayerManager>, IPlayer
    {
        #region Fields

        private DeckHandler _deckHandler;
        private Character _character;
        private CharacterStatsHandler _statsHandler;
        private CardData[] _playerDeck;
        [SerializeField] VisualCharacter _visualCharacter;

        #endregion
      

        public CardData[] StartingCards => _playerDeck;
        public Combo[] Combos => _character.CharacterData.ComboRecipe;
        public bool IsLeft => true;
        public AnimatorController AnimatorController => VisualCharacter.AnimatorController;



        public CharacterStatsHandler StatsHandler { get => _statsHandler; }

        public VisualCharacter VisualCharacter => _visualCharacter;

        public DeckHandler DeckHandler => _deckHandler;

        public void AssignCharacterData(BattleManager battleManager,Character characterData)
        {


            //     Debug.LogWarning("<a>Spawning " + Counter++ + " </a>");
            _character = characterData;
            var data = characterData.CharacterData;
            VisualCharacter.AnimationSound.CurrentCharacter = data.CharacterSO;

            int Length = data.CharacterDeck.Length;

            _playerDeck = new CardData[Length];
            Array.Copy(data.CharacterDeck, _playerDeck, Length);

          //  Battle.Deck.DeckManager.Instance.InitDeck(true, _playerDeck);
            _statsHandler = new CharacterStatsHandler(true, ref data.CharacterStats);
            _deckHandler = new DeckHandler(this, battleManager);
            //  CharacterStatsManager.RegisterCharacterStats(true, ref data.CharacterStats);
            battleManager.TurnHandler.GetCharacterTurn(true).StartTurnOperations.Register(DrawHands);
        }


        public void OnEndTurn()
            => VisualCharacter.AnimatorController.ResetLayerWeight();

        public void PlayerWin()
        {
            VisualCharacter.AnimatorController.CharacterWon();
            _character.CharacterData.CharacterSO.VictorySound.PlaySound();
        }



        private void DrawHands(ITokenReciever tokenMachine)
            => DeckHandler.DrawHand(StatsHandler.GetStats(Keywords.KeywordTypeEnum.Draw).Amount);    

    }

}
