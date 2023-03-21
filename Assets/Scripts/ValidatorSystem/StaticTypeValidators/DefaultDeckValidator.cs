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
        public bool Valid(out IValidFailedInfo validFailedInfo, params ValidationTag[] validationTag)
        {
            if (!Validator.Valid(_defaultDeck, out var info, validationTag))
            {
                validFailedInfo = info;
                return false;
            }
            
            validFailedInfo = null;
            return true;
        }

        public void ExecuteTask(ITokenReceiver tokenMachine, MetaDataManager data)
        {
            _defaultDeck = data.MetaAccountData.CharacterDatas.MainCharacterData.Decks[0];
        }

    }
}