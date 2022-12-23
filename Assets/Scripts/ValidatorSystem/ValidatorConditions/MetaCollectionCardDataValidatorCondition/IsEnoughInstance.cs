using CardMaga.MetaData.Collection;

namespace CardMaga.ValidatorSystem.ValidatorConditions
{
    public class IsEnoughInstance : MetaCollectionCardDataValidatorCondition
    {
        public override bool Valid(MetaCollectionCardData obj)
        {
            return !obj.IsNotMoreInstants;
        }
    }
}