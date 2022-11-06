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
        [SerializeField] KeywordType _currentkeywordType = KeywordType.None;
        [SerializeField] BuffCollectionVisualSO _buffCollectionVisualSO;

        const int BUFF_INDEX = 1;
        const int DEBUFF_INDEX = 2;

        [SerializeField] GameObject _largeBuff;
        [SerializeField] GameObject _smallBuff;

        //[Tooltip("0 = None, 1 = Large, 2 = Small")]
        //[SerializeField] Image[] _buffHolder;

        [Tooltip("0 = None, 1 = Buff, 2 = Debuff")]
        [SerializeField] Image[] _largebuffHolderBG;
        [SerializeField] Image[] _smallbuffHolderBG;

        [SerializeField] BuffTypeVisualSO _buffTypeSO;
        public override void CheckValidation()
        {
            if (_largeBuff == null)
                throw new Exception("BuffTypeVisualAssigner has no Large Buff GO");
            if (_smallBuff == null)
                throw new Exception("BuffTypeVisualAssigner has no Small Buff GO");

            //if (_buffHolder.Length == 0)
            //    throw new Exception("BuffTypeVisualAssigner has no buffHolder");
            //foreach (var image in _buffHolder)
            //{
            //    if (image == null)
            //        throw new Exception($"BuffTypeVisualAssigner has no buffHolder at image:{image}");
            //}

            if (_largebuffHolderBG.Length == 0)
                throw new Exception("BuffTypeVisualAssigner has no largebuffHolderBG");
            foreach (var image in _largebuffHolderBG)
            {
                if (image == null)
                    throw new Exception($"BuffTypeVisualAssigner has no largebuffHolderBG at image:{image}");
            }
            if (_smallbuffHolderBG.Length == 0)
                throw new Exception("BuffTypeVisualAssigner has no smallbuffHolderBG");
            foreach (var image in _smallbuffHolderBG)
            {
                if (image == null)
                    throw new Exception($"BuffTypeVisualAssigner has no smallbuffHolderBG at image:{image}");
            }
            _buffTypeSO.CheckValidation();
        }

        public override void Dispose()
        {
            _currentBuffVisualSO = null;
            _currentkeywordType = KeywordType.None;
        }

        public override void Init(BuffVisualData buffData)
        {
            if (_currentkeywordType != buffData.KeywordType)
            {
                //If is not the same KeywordType Assign A new Keyword Type
                _currentkeywordType = buffData.KeywordType;

                _currentBuffVisualSO = _buffCollectionVisualSO.GetBuffSO(_currentkeywordType);
            }
            if (_currentBuffVisualSO.BuffType == Buff.BuffTypeEnum.None)
            {
                throw new Exception("BuffTypeVisualAssigner Recived BuffEnumType None");
            }
            bool isBuff = _currentBuffVisualSO.BuffType == Buff.BuffTypeEnum.Buff;
            bool toShowText = _currentBuffVisualSO.ToShowText;
            Image holderBG = toShowText ? _largebuffHolderBG[BUFF_INDEX] : _smallbuffHolderBG[BUFF_INDEX];
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
    }
}
