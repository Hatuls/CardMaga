using CardMaga.UI.Visuals;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class AccountNameTextAssigner : BaseTextAssigner<AccountBarVisualData>
    {
        [SerializeField] TextMeshProUGUI _accountNicknameText;
        public override void CheckValidation()
        {
            if (_accountNicknameText == null)
                throw new System.Exception("AccountNameTextAssigner has no account nickname Text");
        }

        public override void Dispose()
        {
            _accountNicknameText.text = EMPTY_TEXT;
        }

        public override void Init(AccountBarVisualData comboData)
        {
            _accountNicknameText.text = comboData.AccountNickname;
        }
    }
}