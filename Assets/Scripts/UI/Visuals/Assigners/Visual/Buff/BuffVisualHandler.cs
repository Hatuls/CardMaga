using CardMaga.Tools.Pools;
using CardMaga.UI.PopUp;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardMaga.UI.Buff
{

    public class BuffVisualHandler : BaseBuffVisualHandler, IPoolableMB<BuffVisualHandler>
    {
        public event Action<BuffVisualHandler> OnDisposed;
        public static event Action<BuffVisualData,RectTransform> OnBuffPointerDown;
        public static event Action<BuffVisualData, RectTransform> OnBuffPointerUp;


        [SerializeField] BuffVisualAssignerHandler _buffVisualAssignerHandler;
        [SerializeField] BuffTextAssignerHandler _buffTextAssignerHandler;
        private BuffVisualData _buffData;
        private RectTransform _rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = transform as RectTransform;
                return _rectTransform;
            }
        }
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
            _buffData = buffData;
            Init();
        }

        public event Action OnInitializable;
        public void Init() { gameObject.SetActive(true); }

        public override void Dispose()
        {
            base.Dispose();
            OnDisposed?.Invoke(this);
            gameObject.SetActive(false);
        }

        public void OpenPopUp() => OnBuffPointerDown?.Invoke(_buffData, RectTransform);
        public void ClosePopUp() => OnBuffPointerUp?.Invoke(_buffData, RectTransform);
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
    }

}