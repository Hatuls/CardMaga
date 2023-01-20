using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.SequenceOperation;
using CardMaga.ValidatorSystem;
using MetaData;
using ReiTools.TokenMachine;

namespace ValidatorSystem.StaticTypeValidators
{
    public class CharacterValidator : IValid , ISequenceOperation<MetaDataManager>
    {
        private MetaCharacterData _characterData;
        
        public int Priority => 1;
        
        public bool Valid(out string failedMessage, params ValidationTag[] validationTag)
        {
            if (!Validator.Valid(_characterData, out var message, validationTag))
            {
                failedMessage = message;
                return false;
            }
            
            failedMessage = String.Empty;
            return true;
        }

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _characterData = data.MetaAccountData.CharacterDatas.CharacterData;
        }

    }
}