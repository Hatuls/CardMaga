using System;
using CardMaga.MetaData.Collection;

namespace CardMaga.ValidatorSystem.ValidatorConditions
{
    public class IsEnoughInstance : BaseValidatorCondition<MetaCollectionCardData>
    {
        public override string FailedMassage => "Not Enough Instance";

        public override bool Valid(MetaCollectionCardData obj, out string failedMassage,ValidationTag validationTag = default)
        {
            if (obj.NumberOfCurrentInstance > 0)
            {
                failedMassage = String.Empty;
                return true;
            }

            failedMassage = FailedMassage;
            return false;
        }
    }
}