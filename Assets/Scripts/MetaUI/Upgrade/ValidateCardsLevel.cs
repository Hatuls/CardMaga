namespace CardMaga.Meta.Upgrade
{
    public class ValidateCardsLevel : IValidateOperation<UpgradeInfo>
    {
        public bool Validate(UpgradeInfo validatedObject)
        => validatedObject.MetaCardData.CardsAtMaxLevel == false;
    }

    public class ValidateUserCurrency : IValidateOperation<UpgradeInfo>
    {
        public bool Validate(UpgradeInfo validatedObject)
        {
            bool hasEnough = validatedObject.MetaCardData.CardsAtMaxLevel == false; // there is no cost for max level card

            if (hasEnough)
            {
                Account.AccountData userData = Account.AccountManager.Instance.Data;
                foreach (var resource in validatedObject.Costs)
                {
                    hasEnough &= userData.AccountResources.HasEnoughAmount(resource);

                    // doesnt has enough
                    if (!hasEnough)
                        break;
                }
            }

            return hasEnough;
        }
    }



    public interface IValidateOperation<T>
    {
        bool Validate(T validatedObject);
    }
}