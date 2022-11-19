using CardMaga.UI.Visuals;
using TMPro;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class AccountNameTextAssigner : BaseTextAssigner<AccountBarVisualData>
    {
        TextMeshProUGUI _accountNicknameText;
        public override void CheckValidation()
        {
            if (_accountNicknameText)
                throw new System.Exception("AccountNameTextAssigner has not account nickname Text");
        }

        public override void Dispose()
        {
            _accountNicknameText.text = EMPTY_TEXT;
        }

        public override void Init(AccountBarVisualData accountBarData)
        {
            _accountNicknameText.text = accountBarData.AccountNickname;
        }
    }
}