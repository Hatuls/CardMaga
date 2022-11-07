using Battle.Combo;
using CardMaga.UI.PopUp;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardMaga.UI.Buff
{
   
    public class BuffVisualHandler : BaseBuffVisualHandler
    {

#if UNITY_EDITOR
        [FormerlySerializedAs("_testBuff")]
        [Header("Test")]
        [SerializeField] BuffVisualData testBuffData;

        [Button]
        public void Test()
        {
            CheckValidation();
            Init(testBuffData);
        }
#endif

        BuffDescriptionPopUp _buffDescriptionPopUp;

        [SerializeField] BuffVisualAssignerHandler _buffVisualAssignerHandler;
        [SerializeField] BuffTextAssignerHandler _buffTextAssignerHandler;
        public override BaseTextAssignerHandler<BuffVisualData> BuffTextAssignerHandler => _buffTextAssignerHandler;
        public override BaseVisualAssignerHandler<BuffVisualData> BuffVisualAssignerHandler => _buffVisualAssignerHandler;
        public override void CheckValidation()
        {
            if (_buffTextAssignerHandler == null)
                throw new System.Exception("BaseBuffVisualHandler Has no Text Assigner Handler");
            if (_buffVisualAssignerHandler == null)
                throw new System.Exception("BaseBuffVisualHandler Has no Visual Assigner Handler");

            base.CheckValidation();
        }
        public override void Init(BuffVisualData buffData)
        {
            base.Init(buffData);
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }

}