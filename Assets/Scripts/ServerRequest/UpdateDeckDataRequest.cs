using System;
using System.Collections.Generic;
using Account;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using ReiTools.TokenMachine;

namespace CardMaga.Server.Request
{
    public class UpdateDeckDataRequest : BaseServerRequest
    {
        private readonly MetaDeckData _deckData;
        private readonly int _characterId;
        
        public UpdateDeckDataRequest(MetaDeckData metaDeckData,int characterId)
        {
            _deckData = metaDeckData;
            _characterId = characterId;
        }
        
        protected override void ServerLogic()
        {
            //server logic
            AccountManager accountManager = AccountManager.Instance;
            
            DeckData deckData = GetDeckData();

            List<DeckData> accountDeckDatas = accountManager.Data.CharactersData.Characters[_characterId].Deck;

            if (!TryGetDeckIndexInAccount(accountDeckDatas,deckData.Id,out int deckIndex))
                throw new Exception("Deck index not found in account");

            accountDeckDatas[deckIndex] = deckData;

            TokenMachine tokenMachine = new TokenMachine(ReceiveResult);
            
            accountManager.SendAccountData(tokenMachine);
        }

        private bool TryGetDeckIndexInAccount(List<DeckData> accountDeckDatas,int deckId, out int deckIndex)
        {
            for (int i = 0; i < accountDeckDatas.Count; i++)
            {
                if (accountDeckDatas[i].Id == deckId)
                {
                    deckIndex = i;
                    return true;
                }
            }

            deckIndex = -1;
            return false;
        }
        
        private DeckData GetDeckData()
        {
            if (ReferenceEquals(_deckData,null))
                throw new Exception("Deck sent to server is null");
            
            DeckData output;

            int id;
            string name;
            CoreID[] cards;
            ComboCore[] combos;

            id = _deckData.DeckId;
            name = _deckData.DeckName;
            List<MetaCardData> cardDatas = _deckData.Cards;
            List<MetaComboData> comboDatas = _deckData.Combos;

            cards = new CoreID[cardDatas.Count];
            combos = new ComboCore[comboDatas.Count];

            for (int i = 0; i < cardDatas.Count; i++)
            {
                cards[i] = new CoreID(cardDatas[i].CardInstance.ID);
            }

            for (int i = 0; i < comboDatas.Count; i++)
            {
                MetaComboData cache = comboDatas[i];
                combos[i] = new ComboCore(cache.ID, cache.Level);
            }

            return output = new DeckData(id, name, cards, combos);
        }
    }
}