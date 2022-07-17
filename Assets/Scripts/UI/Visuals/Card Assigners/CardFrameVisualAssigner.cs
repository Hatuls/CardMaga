using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Visuals
{
    [System.Serializable]
    public class CardFrameVisualAssigner : BaseVisualAssigner
    {
        [SerializeField] FrameCardVisualSO _frameCardVisualSO;
        [SerializeField] Image _frame;
        public override void Init()
        {
            if (_frameCardVisualSO.Frames.Length == 0)
                throw new System.Exception("FrameCardVisualSO has no Frames");
        }
        public void SetFrame(int frameTypeNum)
        {
            var frameType = frameTypeNum - 1;

            var sprite = GetSpriteToAssign(frameType, frameType, _frameCardVisualSO.Frames);
            AssignSprite(_frame, sprite);
        }
    }
}
