
namespace CardMaga.ValidatorSystem.ValidatorConditions.MetaDeckData
{
    public class IsDeckFull : BaseValidatorCondition<MetaData.AccoutData.MetaDeckData>
    {
        public override int ID => 1;
        public override string Message => "Deck is not full";

        public IsDeckFull(ValidationLevel level) : base(level)
        {

        }

        public override bool Valid(MetaData.AccoutData.MetaDeckData obj, out IValidFailedInfo validFailedInfo,params ValidationTag[] validationTag)
        {
            if (obj.Cards.Count == 8)
            {
                validFailedInfo = null;
                return true;
            }

            validFailedInfo = this;
            return false;
        }
    }
}