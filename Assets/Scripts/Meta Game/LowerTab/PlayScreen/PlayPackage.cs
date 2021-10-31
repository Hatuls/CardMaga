using Account.GeneralData;
using System.Collections;
using System.Collections.Generic;
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
        }
    }
}
