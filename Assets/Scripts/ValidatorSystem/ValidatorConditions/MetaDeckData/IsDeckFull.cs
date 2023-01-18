using System;

namespace CardMaga.ValidatorSystem.ValidatorConditions.MetaDeckData
{
    public class IsDeckFull : BaseValidatorCondition<MetaData.AccoutData.MetaDeckData>
    {
        public override int ID => 1;
        public override string Message => "Deck is not full";
        public override bool Valid(MetaData.AccoutData.MetaDeckData obj, out string failedMessage,params ValidationTag[] validationTag)
        {
            if (obj.Cards.Count == 8)
            {
                failedMessage = String.Empty;
                return true;
            }

            failedMessage = Message;
            return false;
        }
    }
}