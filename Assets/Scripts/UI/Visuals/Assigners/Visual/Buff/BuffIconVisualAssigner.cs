using CardMaga.Keywords;
using CardMaga.UI.Visuals;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga
{
    [System.Serializable]
    public class BuffIconVisualAssigner : BaseVisualAssigner<BuffVisualData>
    {
        [SerializeField] BuffCollectionVisualSO _buffCollectionVisualSO;
        [SerializeField] Image _buffIcon;
        [SerializeField] KeywordType _currentKeywordType = KeywordType.None;
        public override void CheckValidation()
        {
            _buffCollectionVisualSO.CheckValidation();
            if (_buffIcon == null)
                throw new System.Exception("BuffIconVisualAssigner has no buff icon");
        }

        public override void Dispose()
        {
            _currentKeywordType = KeywordType.None;
        }

        public override void Init(BuffVisualData comboData)
        {
            if (_currentKeywordType != comboData.KeywordType || _currentKeywordType == KeywordType.None)
            {
                //If is not the same KeywordType Assign A new Keyword Type
                _currentKeywordType = comboData.KeywordType;

                _buffIcon.sprite = _buffCollectionVisualSO.GetBuffSO(_currentKeywordType).BuffIcon;
            }
            else
            {
                //_buffIcon holds the correct image so we just keep it
            }
        }
    }
}