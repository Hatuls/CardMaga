using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;
using CardMaga.Keywords;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class BuffTypeVisualAssigner : BaseVisualAssigner<BuffVisualData>
    {
        [ReadOnly] BuffVisualSO _currentBuffVisualSO;
        [SerializeField] KeywordType _currentKeywordType = KeywordType.None;
        [SerializeField] BuffCollectionVisualSO _buffCollectionVisualSO;

        const int BUFF_INDEX = 1;
        const int DEBUFF_INDEX = 2;

        const int LARGE_HOLDER_INDEX = 1;
        const int SMALL_HOLDER_INDEX = 2;

        [SerializeField] GameObject _largeBuff;
        [SerializeField] GameObject _smallBuff;

        [Tooltip("0 = None, 1 = Large, 2 = Small")]
        [SerializeField] Image[] _buffHolder;

        [Tooltip("0 = None, 1 = Large, 2 = Small")]
        [SerializeField] Image[] _buffHolderBG;

        [SerializeField] BuffTypeVisualSO _buffTypeSO;
        public override void CheckValidation()
        {
            if (_largeBuff == null)
                throw new Exception("BuffTypeVisualAssigner has no Large Buff GO");
            if (_smallBuff == null)
                throw new Exception("BuffTypeVisualAssigner has no Small Buff GO");

            if (_buffHolder.Length == 0)
                throw new Exception("BuffTypeVisualAssigner has no buffHolder");
            //foreach (var image in _buffHolder)
            //{
            //    if (image == null)
            //        throw new Exception($"BuffTypeVisualAssigner has no buffHolder at image:{image}");
            //}

            if (_buffHolderBG.Length == 0)
                throw new Exception("BuffTypeVisualAssigner has no buffHolderBG");
            //foreach (var image in _buffHolderBG)
            //{
            //    if (image == null)
            //        throw new Exception($"BuffTypeVisualAssigner has no buffHolderBG at image:{image}");
            //}
            _buffTypeSO.CheckValidation();
        }

        public override void Dispose()
        {
            _currentBuffVisualSO = null;
            _currentKeywordType = KeywordType.None;
        }

        public override void Init(BuffVisualData comboData)
        {
            if (_currentKeywordType != comboData.KeywordType)
            {
                //If is not the same KeywordType Assign A new Keyword Type
                _currentKeywordType = comboData.KeywordType;

                _currentBuffVisualSO = _buffCollectionVisualSO.GetBuffSO(_currentKeywordType);
            }
            if (_currentBuffVisualSO.BuffType == Buff.BuffTypeEnum.None)
            {
                throw new Exception("BuffTypeVisualAssigner Recived BuffEnumType None");
            }
            //assign holder sprites if did not assigned them before
            SetBuffHolders();

            bool toShowText = _currentBuffVisualSO.ToShowText;
            bool isBuff = _currentBuffVisualSO.BuffType == Buff.BuffTypeEnum.Buff;
            Image holderBG = toShowText ? _buffHolderBG[LARGE_HOLDER_INDEX] : _buffHolderBG[SMALL_HOLDER_INDEX];
            Sprite[] sprites = toShowText ? _buffTypeSO.LargeBG : _buffTypeSO.SmallBG;
            ActivateLargeBuff(toShowText);
            ActivateBuffBG(isBuff, holderBG, sprites);


        }
        private void ActivateLargeBuff(bool isTrue)
        {
            _largeBuff.SetActive(isTrue);
            _smallBuff.SetActive(!isTrue);
        }
        private void ActivateBuffBG(bool isGoodBuff, Image image, Sprite[] soImages)
        {
            image.sprite = soImages[isGoodBuff ? BUFF_INDEX: DEBUFF_INDEX];
        }
        private void SetBuffHolders()
        {
            if (_buffHolder[LARGE_HOLDER_INDEX].sprite != _buffTypeSO.Holders[LARGE_HOLDER_INDEX])
            {
                _buffHolder[LARGE_HOLDER_INDEX].sprite = _buffTypeSO.Holders[LARGE_HOLDER_INDEX];
            }
            if (_buffHolder[SMALL_HOLDER_INDEX].sprite != _buffTypeSO.Holders[SMALL_HOLDER_INDEX])
            {
                _buffHolder[SMALL_HOLDER_INDEX].sprite = _buffTypeSO.Holders[SMALL_HOLDER_INDEX];
            }
        }
    }
}
