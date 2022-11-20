using CardMaga.UI.Visuals;
using TMPro;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class EXPBarTextAssigner : BaseTextAssigner<AccountBarVisualData>
    {
        TextMeshProUGUI _maxExpText;
        TextMeshProUGUI _currentExpText;
        TextMeshProUGUI _accountLevelText;
        public override void CheckValidation()
        {
            if (_maxExpText != null)
                throw new System.Exception("EXPBarTextAssigner has no Max EXP text");
            if (_currentExpText != null)
                throw new System.Exception("EXPBarTextAssigner has no current EXP text");
            if (_accountLevelText != null)
                throw new System.Exception("EXPBarTextAssigner has no account level text");
        }
        public override void Dispose()
        {
            _maxExpText.text = EMPTY_TEXT;
            _currentExpText.text = EMPTY_TEXT;
            _accountLevelText.text = EMPTY_TEXT;
        }
        public override void Init(AccountBarVisualData accountBarData)
        {
            _maxExpText.AssignText(accountBarData.MaxExpAmount.ToString());
            _currentExpText.AssignText(accountBarData.CurrentExpAmount.ToString());
            _accountLevelText.AssignText(accountBarData.AccountLevel.ToString());
        }
    }
}