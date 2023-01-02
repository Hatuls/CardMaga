using CardMaga.Keywords;
using CardMaga.UI.Visuals;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class BuffAmountTextAssigner : BaseTextAssigner<BuffVisualData>
    {
        [SerializeField] TextMeshProUGUI _buffAmountText;
        [SerializeField] BuffCollectionVisualSO _buffCollectionVisualSO;
        [SerializeField] KeywordType _currentkeywordType = KeywordType.None;
        [ReadOnly] BuffVisualSO _currentBuffVisualSO;
        public override void CheckValidation()
        {
            _buffCollectionVisualSO.CheckValidation();
            if (_buffAmountText == null)
                throw new System.Exception("BuffAmountTextAssigner Has no buff amount text(TMP)");
        }

        public override void Dispose()
        {
            _currentkeywordType = KeywordType.None;
            _buffAmountText.gameObject.SetActive(false);
            _currentBuffVisualSO = null;
        }

        public override void Init(BuffVisualData comboData)
        {
            if (_currentkeywordType != comboData.KeywordType)
            {
                _currentkeywordType = comboData.KeywordType;

                _currentBuffVisualSO = _buffCollectionVisualSO.GetBuffSO(_currentkeywordType);
                if (!_currentBuffVisualSO.ToShowText)
                {
                    //no need to show text
                    _buffAmountText.gameObject.SetActive(false);

                    return;
                }
            }
            //we want to show text
            _buffAmountText.gameObject.SetActive(true);

            if (!_currentBuffVisualSO.IsShardText)
            {
                //Activate the text object and display the basic text
                _buffAmountText.text = comboData.BuffCurrentAmount.ToString();
            }
            else
            {
                //Display the current amount of the max amount
                _buffAmountText.text = $"{comboData.BuffCurrentAmount}/{_currentBuffVisualSO.MaxShards}";
            }
        }
    }
}