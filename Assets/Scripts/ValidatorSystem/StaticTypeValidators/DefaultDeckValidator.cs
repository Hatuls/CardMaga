using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.SequenceOperation;
using CardMaga.ValidatorSystem;
using MetaData;
using ReiTools.TokenMachine;

namespace ValidatorSystem.StaticTypeValidators
{
    public class DefaultDeckValidator : IValid , ISequenceOperation<MetaDataManager>
    {
        private MetaDeckData _defaultDeck;
        
        public int Priority => 1;
        public bool Valid(out string failedMessage, params ValidationTag[] validationTag)
        {
            if (!Validator.Valid(_defaultDeck, out var message, validationTag))
            {
                failedMessage = message;
                return false;
            }
            
            failedMessage = String.Empty;
            return true;
        }

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _defaultDeck = data.MetaAccountData.CharacterDatas.CharacterData.Decks[0];
        }

    }
}