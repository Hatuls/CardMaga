using System;
using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.SequenceOperation;
using CardMaga.ValidatorSystem;
using MetaData;
using ReiTools.TokenMachine;
using ValidatorSystem.StaticTypeValidators;

namespace ValidatorSystem.ValidatorTerminals
{
    public class MetaValidatorTerminal : BaseValidatorTerminal , ISequenceOperation<MetaDataManager>
    {
        private SequenceHandler<MetaDataManager> _sequenceHandler;
        
        private DefaultDeckValidator _defaultDeckValidator;
        private CharacterValidator _characterValidator;

        public DefaultDeckValidator DefaultDeckValidator => _defaultDeckValidator;

        public CharacterValidator CharacterValidator => _characterValidator;

        public int Priority => 1;
        
        protected override Type[] TypeValidator { get; } =
        {
            typeof(TypeValidator<MetaCollectionCardData>),
            typeof(TypeValidator<MetaCollectionComboData>),
            typeof(TypeValidator<MetaDeckData>),
            typeof(TypeValidator<CardInstance>),
            typeof(TypeValidator<MetaCardInstanceInfo>),
            typeof(TypeValidator<MetaCharacterData>),
            typeof(TypeValidator<string>),
        };
        
        private IEnumerable<ISequenceOperation<MetaDataManager>> DataInitializers
        {
            get
            {
                yield return _defaultDeckValidator = new DefaultDeckValidator();
                yield return _characterValidator = new CharacterValidator();
            }
        }

        public void ExecuteTask(ITokenReceiver tokenMachine, MetaDataManager data)
        {
            _sequenceHandler = new SequenceHandler<MetaDataManager>();
            
            foreach (var operation in DataInitializers)
            {
                _sequenceHandler.Register(operation);
            }
            
            _sequenceHandler.StartAll(data);
        }
    }
}