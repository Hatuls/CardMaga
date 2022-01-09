using Account.GeneralData;
using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public class PlayPackage
    {
        #region Fields
        CharacterData _characterData;
        AccountDeck _deck;
        #endregion
        #region Properties
        public CharacterData CharacterData { get => _characterData; set => _characterData = value; }
        public AccountDeck Deck { get => _deck; set => _deck = value; } 
        #endregion

        public void SendPackage()
        {
            var battleData = Account.AccountManager.Instance.BattleData;
            Debug.Log($"Sending Package of Character {CharacterData.CharacterEnum} with the Chosen Deck {Deck.DeckName}");
            battleData.Player = Factory.GameFactory.Instance.CharacterFactoryHandler.CreateCharacter(_characterData, _characterData.Decks[0]);
            battleData.PlayerWon = false;
            battleData.IsFinishedPlaying = false;
            battleData.ResetMapRewards();

            Map.MapManager.ResetSavedMap();
        }
    }
}
