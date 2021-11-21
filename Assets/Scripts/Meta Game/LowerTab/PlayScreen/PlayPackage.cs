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
            Debug.Log($"Sending Package of Character {CharacterData.CharacterEnum} with the Chosen Deck {Deck.DeckName}");
            Battles.BattleData.Player = Factory.GameFactory.Instance.CharacterFactoryHandler.CreateCharacter(_characterData, _deck);
            Battles.BattleData.PlayerWon = false;
            Battles.BattleData.IsFinishedPlaying = false;
            Battles.BattleData.MapRewards = new Battles.MapRewards();
            if (PlayerPrefs.HasKey("Map"))
                PlayerPrefs.DeleteKey("Map");
        }
    }
}
