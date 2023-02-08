using Account.GeneralData;
using CardMaga.Card;
using CardMaga.UI.Visuals;
using DG.Tweening;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI
{
    public class CardZoomHandler : MonoBehaviour
    {
        public event Action OnZoomInStarted;
        public event Action OnZoomOutStarted;
        public event Action OnZoomInCompleted;
        public event Action OnZoomOutCompleted;
        public static event Action OnZoomInLocation;

        [SerializeField] ZoomPositionsSO _zoomPositionsSO;
        [SerializeField] ZoomDoTweenSO _zoomDoTweenSO;

        [Tooltip("Attack, Defense, Utility")]
        [SerializeField] RectTransform _bottomPart;
        [SerializeField] Image _cardGlow;
        [SerializeField] Image _bottomPartBlackScreen;
        [SerializeField] RectTransform _cardVisualMainObject;
        [SerializeField] RectTransform _cardName;
        [SerializeField] CanvasGroup _description;

        DG.Tweening.Sequence _zoomSequence;
        int _cardType;

        TokenMachine _zoomTokenMachine;
        public ITokenReciever ZoomTokenMachine => _zoomTokenMachine;

        public float StartPos => _zoomPositionsSO.YStartPosition[_cardType];
        public float EndPos => _zoomPositionsSO.YEndPosition[_cardType];

        private void Awake()
        {
            if (_bottomPart == null)
                throw new Exception("CardZoomHandler has no bottom part");
            if (_cardVisualMainObject == null)
                throw new Exception("CardZoomHandler has no battleCard visual main object");
            if (_cardName == null)
                throw new Exception("CardZoomHandler has no battleCard Name");
            if (_description == null)
                throw new Exception("CardZoomHandler has no description");

            //Token
            _zoomTokenMachine = new TokenMachine(ZoomIn, ZoomOut);
        }
        private void OnDisable()
        {
            ForceReset();
        }
        [Button]
        public void ForceReset()
        {
            //Forcefully Kill the tween and reset all of the parts to it's place imideatly
            KillTween();
            //in parallel set text opacity to 0
            _description.alpha = 0;
            //in Parallel Set Name Scale to regular size
            _cardName.localScale = Vector3.one;
            //when completed move battleCard parts
            _bottomPart.localPosition = new Vector2(_bottomPart.localPosition.x, StartPos);
            //in parallel set alpha of bottompartBGscreen to 0
            Color color = _bottomPartBlackScreen.color;
            color.a = 0;
            _bottomPartBlackScreen.color = color;
            //set glow to 1
            color = _cardGlow.color;
            color.a = 1;
            _cardGlow.color = color;
            //scale battleCard
            _cardVisualMainObject.localScale = Vector3.one;
        }
        public void SetCardType(CardCore battleCardData)
        {
            _cardType = (int)battleCardData.CardSO.CardTypeData.CardType - 1;
   

        }
        public void ResetCardType()
        {
            _cardType = 0;
        }
        public void KillTween()
        {
            if (_zoomSequence != null)
                _zoomSequence.Kill();

        }
      [Button("HandZoom In")]
        private void ZoomIn()
        {
            KillTween();
            Debug.Log("Zooming In");
            _zoomSequence = DOTween.Sequence();
            //scale battleCard
            _zoomSequence.Append(_cardVisualMainObject.DOScale(_zoomDoTweenSO.ZoomScale, _zoomDoTweenSO.ZoomDuration).SetEase(_zoomDoTweenSO.CardCurveZoomIn));
            
            //set glow to 0
            _zoomSequence.Join(_cardGlow.DOFade(0, _zoomDoTweenSO.GlowFadeDuration));

            //when completed move battleCard parts
            _zoomSequence.Join(_bottomPart.DOLocalMoveY(EndPos, _zoomDoTweenSO.BottomPartMoveDuration).SetEase(_zoomDoTweenSO.BottomPartMoveCurve));

            //in parallel set alpha of bottompartBGscreen to 1
            _zoomSequence.Join(_bottomPartBlackScreen.DOFade(1, _zoomDoTweenSO.BlackScreenDuration));

            //in Parallel Set Name Scale to smaller
            _zoomSequence.Join(_cardName.DOScale(_zoomDoTweenSO.NameTextScaleZoomIn, _zoomDoTweenSO.NameScaleDurationZoomIn).SetEase(_zoomDoTweenSO.NameScaleCurveZoomIn));

            //in parallel set text opacity to visable
            _zoomSequence.Join(_description.DOFade(1, _zoomDoTweenSO.TextAlphaDuration));
            if(OnZoomInCompleted != null)
                _zoomSequence.AppendCallback(OnZoomInCompleted.Invoke);

            OnZoomInStarted?.Invoke();
        }
        [Button("HandZoom Out")]
        private void ZoomOut()
        {
            KillTween();
            Debug.Log("Zooming Out");
            _zoomSequence = DOTween.Sequence();
            //in parallel set text opacity to 0
            _zoomSequence.Append(_description.DOFade(0, _zoomDoTweenSO.TextAlphaDuration));
            //in Parallel Set Name Scale to regular size
            _zoomSequence.Join(_cardName.DOScale(Vector3.one, _zoomDoTweenSO.NameScaleDurationZoomOut).SetEase(_zoomDoTweenSO.NameScaleCurveZoomOut));
            //when completed move battleCard parts
            _zoomSequence.Join(_bottomPart.DOLocalMoveY(StartPos, _zoomDoTweenSO.BottomPartMoveDuration).SetEase(Ease.OutSine));
            //in parallel set alpha of bottompartBGscreen to 0
            _zoomSequence.Join(_bottomPartBlackScreen.DOFade(0, _zoomDoTweenSO.BottomPartMoveDuration));
            //set glow to 1
            _zoomSequence.Join(_cardGlow.DOFade(1, _zoomDoTweenSO.GlowFadeDuration));
            //scale battleCard
            _zoomSequence.Join(_cardVisualMainObject.DOScale(Vector3.one, _zoomDoTweenSO.ZoomDuration));

            if (OnZoomOutCompleted != null)
                _zoomSequence.AppendCallback(OnZoomOutCompleted.Invoke);

            OnZoomOutStarted?.Invoke();
        }
    }
}
