using CardMaga.UI;
using CardMaga.UI.Card;
using UnityEngine;

namespace CardMaga.Input
{
    public class CardUIInputHandler : TouchableItem<BattleCardUI>
    {
        [SerializeField] 
        private RectTransform _rectTransform;
        [SerializeField]
        private CardZoomHandler _cardZoomHandler;
        [SerializeField]
        private StrechBattleInput _zoomInScale;
        ICardUISize[] _cardUISize;

        public ICardUISize CardUIInputSize => _cardUISize[0];
        protected override void Awake()
        {
            base.Awake();
            _zoomInScale.Init(_rectTransform.rect.width,_rectTransform.rect.height);
            _cardZoomHandler.OnZoomInCompleted += _zoomInScale.SetResolution;
            _cardZoomHandler.OnZoomOutCompleted += _zoomInScale.Reset;
        }
        private void Start()
        {
            _cardUISize = new ICardUISize[]
            {
                _zoomInScale
            };
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _cardZoomHandler.OnZoomInCompleted  -= _zoomInScale.SetResolution;
            _cardZoomHandler.OnZoomOutCompleted -= _zoomInScale.Reset;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _zoomInScale.DrawGizmos();
        }
#endif
    }


    public interface ICardUISize
    {
        void SetHeight(float height);
        void SetWidth(float width);
        void SetResolution();
        void Reset();

#if UNITY_EDITOR
        Color GizmosColor { get; }
#endif
    }
    [System.Serializable]
    public class StrechBattleInput : ICardUISize
    {
        [SerializeField]
        private RectTransform _rectTransform;
        [SerializeField]
        private RectTransform _imageRectTransform;
        [SerializeField]
        private Vector3 _offsetPosition;

        private float _startWidth;
        private float _startHeight;
        
#if UNITY_EDITOR
        [SerializeField]
        Color _gizmosColor;

   
        
        public Color GizmosColor =>_gizmosColor;
        public void DrawGizmos()
        {
            Gizmos.color = _gizmosColor;

            //Gizmos.DrawWireCube(
            //    _rectTransform.position + _offsetPosition,
            //    new Vector3(_width, _height)
            //    );
        }
#endif
        [SerializeField,Tooltip("The Specified Width Required To Change To")]
        private float _width;
        [SerializeField, Tooltip("The Specified Height Required To Change To")]
        private float _height;

        public void Init(float startWidth, float startHeight)
        {
            _startHeight = startHeight;
            _startWidth = startWidth;
        }
    

        public void Reset()
        {
            //_rectTransform.localPosition = Vector3.zero;
            SetResolution(_startWidth, _startHeight);
        }

        public void SetHeight(float height)
        {
            SetResolution(_rectTransform.sizeDelta.x, _height);
        }

        public void SetOffsetPosition()
            => _rectTransform.localPosition += _offsetPosition;
        public void SetResolution()
        { 
            SetResolution(_width, _height);
            SetOffsetPosition();
        }
        private void SetResolution(float width, float height)
        {
            _rectTransform.sizeDelta = new Vector2(width, height);
            _imageRectTransform.sizeDelta = _rectTransform.sizeDelta;
        }

        public void SetWidth(float width)
        {
            SetResolution(_width, _rectTransform.sizeDelta.y);
        }
    }
}