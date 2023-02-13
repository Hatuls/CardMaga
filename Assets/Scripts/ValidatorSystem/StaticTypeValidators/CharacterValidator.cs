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
        
        public bool Valid(out IValidFailedInfo validFailedInfo, params ValidationTag[] validationTag)
        {
            if (!Validator.Valid(_characterData, out var info, validationTag))
            {
                validFailedInfo = info;
                return false;
            }
            
            validFailedInfo = null;
            return true;
        }

        public void ExecuteTask(ITokenReceiver tokenMachine, MetaDataManager data)
        {
            _characterData = data.MetaAccountData.CharacterDatas.MainCharacterData;
        }

    }
}