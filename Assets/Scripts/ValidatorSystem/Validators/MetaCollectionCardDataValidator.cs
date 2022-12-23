using System;
using CardMaga.MetaData.Collection;
using UnityEngine;

namespace CardMaga.ValidatorSystem
{
    [Serializable]
    public class MetaCollectionCardDataValidator : BaseTypeValidator<MetaCollectionCardData>
    {
        [SerializeField] private MetaCollectionCardDataValidatorCondition[] _condition;

        public override BaseValidatorCondition<MetaCollectionCardData>[] ValidatorCondition => _condition;
    }
}