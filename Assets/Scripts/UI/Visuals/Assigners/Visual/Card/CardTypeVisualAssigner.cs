using Account.GeneralData;
using CardMaga.Card;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [Serializable]
    public class CardTypeVisualAssigner : BaseVisualAssigner<CardCore>
    {

        [SerializeField] TypeCardVisualSO _typeCardVisualSO;
        [SerializeField] Image _frame;
        [SerializeField] Image _innerFrame;

        public override void CheckValidation()
        {
            if (_typeCardVisualSO.Frames.Length == 0)
                throw new Exception("TypeCardVisualSO has no frames");

            if (_typeCardVisualSO.InnerFrames.Length == 0)
                throw new Exception("TypeCardVisualSO has no inner frames");
        }
        public override void Init(CardCore comboData)
        {
            int cardType = (int)comboData.CardSO.CardTypeData.CardType - 1;
            var sprite = BaseVisualSO.GetSpriteToAssign(cardType, cardType, comboData.CardSO.IsCombo? _typeCardVisualSO.GoldFrame : _typeCardVisualSO.Frames);
            _frame.AssignSprite(sprite);

            sprite = BaseVisualSO.GetSpriteToAssign(cardType, cardType, _typeCardVisualSO.InnerFrames);
            _innerFrame.AssignSprite(sprite);
        }

        public override void Dispose()
        {
        }

    }
}
