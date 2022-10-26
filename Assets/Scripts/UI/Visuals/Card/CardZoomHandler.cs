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
        float _startPos;
        float _endPos;

        TokenMachine _zoomTokenMachine;
        public ITokenReciever ZoomTokenMachine => _zoomTokenMachine;
        private void Start()
        {
            if (_bottomPart == null)
                throw new Exception("CardZoomHandler has no bottom part");
            if (_cardVisualMainObject == null)
                throw new Exception("CardZoomHandler has no card visual main object");
            if (_cardName == null)
                throw new Exception("CardZoomHandler has no card Name");
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
            //when completed move card parts
            _bottomPart.localPosition = new Vector2(_bottomPart.localPosition.x, _startPos);
            //in parallel set alpha of bottompartBGscreen to 0
            Color color = _bottomPartBlackScreen.color;
            color.a = 0;
            _bottomPartBlackScreen.color = color;
            //set glow to 1
            color = _cardGlow.color;
            color.a = 1;
            _cardGlow.color = color;
            //scale card
            _cardVisualMainObject.localScale = Vector3.one;
        }
        public void SetCardType(CardData cardData)
        {
            int cardType = (int)cardData.CardTypeData.CardType - 1;
            _startPos = _zoomPositionsSO.YStartPosition[cardType];
            _endPos = _zoomPositionsSO.YEndPosition[cardType];
            Debug.Log("Recived ZoomData");
        }
        public void ResetCardType()
        {
            int cardType = 0;
            _startPos = _zoomPositionsSO.YStartPosition[cardType];
            _endPos = _zoomPositionsSO.YEndPosition[cardType];
	        //Debug.Log("Recived ZoomData");
        }
        public void KillTween()
        {
            if (_zoomSequence != null)
                _zoomSequence.Kill();

        }
   //     [Button("HandZoom In")]
        private void ZoomIn()
        {
            KillTween();
            Debug.Log("Zooming In");
            _zoomSequence = DOTween.Sequence();
            //scale card
            _zoomSequence.Append(_cardVisualMainObject.DOScale(_zoomDoTweenSO.ZoomScale, _zoomDoTweenSO.ZoomDuration).SetEase(_zoomDoTweenSO.CardCurveZoomIn));

            //set glow to 0
            _zoomSequence.Join(_cardGlow.DOFade(0, _zoomDoTweenSO.GlowFadeDuration));

            //when completed move card parts
            _zoomSequence.Join(_bottomPart.DOLocalMoveY(_endPos, _zoomDoTweenSO.BottomPartMoveDuration).SetEase(_zoomDoTweenSO.BottomPartMoveCurve));

            //in parallel set alpha of bottompartBGscreen to 1
            _zoomSequence.Join(_bottomPartBlackScreen.DOFade(1, _zoomDoTweenSO.BlackScreenDuration));

            //in Parallel Set Name Scale to smaller
            _zoomSequence.Join(_cardName.DOScale(_zoomDoTweenSO.NameTextScaleZoomIn, _zoomDoTweenSO.NameScaleDurationZoomIn).SetEase(_zoomDoTweenSO.NameScaleCurveZoomIn));

            //in parallel set text opacity to visable
            _zoomSequence.Join(_description.DOFade(1, _zoomDoTweenSO.TextAlphaDuration));
            if(OnZoomInCompleted != null)
                _zoomSequence.OnComplete(OnZoomInCompleted.Invoke);
        }
  //      [Button("HandZoom Out")]
        private void ZoomOut()
        {
            KillTween();
            Debug.Log("Zooming Out");
            _zoomSequence = DOTween.Sequence();
            //in parallel set text opacity to 0
            _zoomSequence.Append(_description.DOFade(0, _zoomDoTweenSO.TextAlphaDuration));
            //in Parallel Set Name Scale to regular size
            _zoomSequence.Join(_cardName.DOScale(Vector3.one, _zoomDoTweenSO.NameScaleDurationZoomOut).SetEase(_zoomDoTweenSO.NameScaleCurveZoomOut));
            //when completed move card parts
            _zoomSequence.Join(_bottomPart.DOLocalMoveY(_startPos, _zoomDoTweenSO.BottomPartMoveDuration).SetEase(Ease.OutSine));
            //in parallel set alpha of bottompartBGscreen to 0
            _zoomSequence.Join(_bottomPartBlackScreen.DOFade(0, _zoomDoTweenSO.BottomPartMoveDuration));
            //set glow to 1
            _zoomSequence.Join(_cardGlow.DOFade(1, _zoomDoTweenSO.GlowFadeDuration));
            //scale card
            _zoomSequence.Join(_cardVisualMainObject.DOScale(Vector3.one, _zoomDoTweenSO.ZoomDuration));

            if (OnZoomOutCompleted != null)
                _zoomSequence.OnComplete(OnZoomOutCompleted.Invoke);
        }
    }
}
