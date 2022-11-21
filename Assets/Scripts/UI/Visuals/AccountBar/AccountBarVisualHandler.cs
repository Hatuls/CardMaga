using CardMaga.UI.Text;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public class AccountBarVisualHandler : BaseAccountBarVisualHandler
    {
        [SerializeField] AccountBarVisualAssignerHandler _accountBarVisualAssignerHandler;
        [SerializeField] AccountBarTextAssignerHandler _accountBarTextAssignerHandler;
        public override BaseVisualAssignerHandler<AccountBarVisualData> AccountBarVisualAssignerHandler => _accountBarVisualAssignerHandler;
        public override BaseTextAssignerHandler<AccountBarVisualData> AccountBarTextAssignerHandler => _accountBarTextAssignerHandler;

        public override void CheckValidation()
        {
            base.CheckValidation();
        }
        public override void Init(AccountBarVisualData accountBarData)
        {
            base.Init(accountBarData);
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}