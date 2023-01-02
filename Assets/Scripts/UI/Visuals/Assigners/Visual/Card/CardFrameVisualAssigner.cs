using Account.GeneralData;
using CardMaga.Card;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class CardFrameVisualAssigner : BaseVisualAssigner<CardCore>
    {
        [SerializeField] FrameCardVisualSO _frameCardVisualSO;
        [SerializeField] Image _frame;

        public override void CheckValidation()
        {
            if (_frameCardVisualSO.Frames.Length == 0)
                throw new System.Exception("FrameCardVisualSO has no Frames");
        }
        public override void Init(CardCore battleCardData)
        {
            //hard Coded value
            var frameType = 0;

            var sprite = BaseVisualSO.GetSpriteToAssign(frameType, frameType, _frameCardVisualSO.Frames);
            _frame.AssignSprite(sprite);
        }
        public override void Dispose()
        {
        }
    }
}
