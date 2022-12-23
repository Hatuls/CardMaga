using System;
using CardMaga.MetaData.AccoutData;
using UnityEngine;

namespace CardMaga.ValidatorSystem
{
    [Serializable]
    public class MetaCardDataValidator : BaseTypeValidator<MetaCardData>
    {
        [SerializeField] private MetaCardDataValidatorCondition[] _conditions;

        public override BaseValidatorCondition<MetaCardData>[] ValidatorCondition => _conditions;
    }
}