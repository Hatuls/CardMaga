using System;

namespace CardMaga.ValidatorSystem.ValidatorConditions.MetaDeckData
{
    public class IsFull : BaseValidatorCondition<MetaData.AccoutData.MetaDeckData>
    {
        public override string FailedMassage => "Deck is not full";
        public override bool Valid(MetaData.AccoutData.MetaDeckData obj, out string failedMassage, ValidationTag validationTag = default)
        {
            if (obj.Cards.Count == 8)
            {
                failedMassage = String.Empty;
                return true;
            }

            failedMassage = FailedMassage;
            return false;
        }
    }
}