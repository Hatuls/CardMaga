using CardMaga.UI.Visuals;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class EXPBarTextAssigner : BaseTextAssigner<AccountBarVisualData>
    {
        [SerializeField] TextMeshProUGUI _maxExpText;
        [SerializeField] TextMeshProUGUI _currentExpText;
        [SerializeField] TextMeshProUGUI _accountLevelText;
        public override void CheckValidation()
        {
            //if (_maxExpText == null)
            //    throw new System.Exception("EXPBarTextAssigner has no Max EXP text");
            //if (_currentExpText == null)
            //    throw new System.Exception("EXPBarTextAssigner has no current EXP text");
            if (_accountLevelText == null)
                throw new System.Exception("EXPBarTextAssigner has no account level text");
        }
        public override void Dispose()
        {
            _maxExpText.text = EMPTY_TEXT;
            _currentExpText.text = EMPTY_TEXT;
            _accountLevelText.text = EMPTY_TEXT;
        }
        public override void Init(AccountBarVisualData comboData)
        {
            //_maxExpText.AssignText(comboData.MaxExpAmount.ToString());
            //_currentExpText.AssignText(comboData.CurrentExpAmount.ToString());
            _accountLevelText.AssignText(comboData.AccountLevel.ToString());
        }
    }
}