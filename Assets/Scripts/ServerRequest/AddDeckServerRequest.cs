using System;
using System.Collections.Generic;
using Account;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using ReiTools.TokenMachine;

namespace CardMaga.Server.Request
{
    public class AddDeckServerRequest : BaseServerRequest
    {
        private AccountManager _accountManager;
        
        private readonly MetaDeckData _deckData;
        private readonly int _characterId;
        
        public AddDeckServerRequest(MetaDeckData metaDeckData,int characterId)
        {
            _deckData = metaDeckData;
            _characterId = characterId;
            _accountManager = AccountManager.Instance;
        }
        
        protected override void ServerLogic()
        {
            _accountManager.Data.CharactersData.Characters[GetCharacterIndexById(_characterId)].AddNewDeck(GetDeckData());
            
            TokenMachine tokenMachine = new TokenMachine(ReceiveResult);
            
            _accountManager.SendAccountData(tokenMachine);
        }

        protected override void ReceiveResult()
        {
            _deckData.RegisterDeck();
            base.ReceiveResult();
        }

        private int GetCharacterIndexById(int Id)
        {
            List<Character> characters = _accountManager.Data.CharactersData.Characters;

            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].ID == Id)
                {
                    return i;
                }
            }

            return -1;
        }

        private DeckData GetDeckData()
        {
            if (ReferenceEquals(_deckData,null))
                throw new Exception("Deck sent to server is null");
            
            int id;
            string name;
            CoreID[] cards;
            ComboCore[] combos;

            id = _deckData.DeckId;
            name = _deckData.DeckName;
            
            List<MetaCardInstanceInfo> cardDatas = _deckData.Cards;
            List<MetaComboInstanceInfo> comboDatas = _deckData.Combos;
            
            cards = new CoreID[cardDatas.Count];

            for (int i = 0; i < cardDatas.Count; i++)
            {
                cards[i] = new CoreID(cardDatas[i].CoreID);
            }

            combos = new ComboCore[comboDatas.Count];
            
            for (int i = 0; i < comboDatas.Count; i++)
            {
                combos[i] = new ComboCore(comboDatas[i].CoreID);
            }

            return new DeckData(id, name, cards, combos);
        }
    }
}