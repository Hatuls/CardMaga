using System;
using CardMaga.MetaData.Collection;

namespace CardMaga.ValidatorSystem.ValidatorConditions
{
    public class IsEnoughInstance : BaseValidatorCondition<MetaCollectionCardData>
    {
        public override int ID => 2;
        public override string Message => "Not Enough Instance";

        public override bool Valid(MetaCollectionCardData obj, out IValidFailedInfo validFailedInfo,params ValidationTag[] validationTag)
        {
            if (obj.NumberOfCurrentInstance > 0)
            {
                validFailedInfo = null;
                return true;
            }

            validFailedInfo = this;
            return false;
        }
    }
}