using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace UI.Visuals
{
    [Serializable]
    public class CardTypeVisualAssigner : BaseVisualAssigner
    {

        [SerializeField] TypeCardVisualSO _typeCardVisualSO;
        [SerializeField] Image _frame;
        [SerializeField] Image _innerFrame;
        public override void Init()
        {
            if (_typeCardVisualSO.Frames.Length == 0)
                throw new Exception("TypeCardVisualSO has no frames");

            if (_typeCardVisualSO.InnerFrames.Length == 0)
                throw new Exception("TypeCardVisualSO has no inner frames");
        }

        public void SetType(int cardTypeNum)
        {
            var cardType = cardTypeNum - 1;
            var sprite = GetSpriteToAssign(cardType, cardType, _typeCardVisualSO.Frames);
            AssignSprite(_frame, sprite);

            sprite = GetSpriteToAssign(cardType, cardType, _typeCardVisualSO.InnerFrames);
            AssignSprite(_innerFrame, sprite);
        }
    }
}
