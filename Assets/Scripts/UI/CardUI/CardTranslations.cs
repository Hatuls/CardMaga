using UnityEngine;


namespace Battles.UI.CardUIAttributes
{
    public class CardTranslations
    {
        RectTransform _rectTransform;
        public CardTranslations(ref RectTransform rectTransform)
        {
            _rectTransform = rectTransform;
        }

        public void MoveCard(Vector3 moveTo)
        {
            if (_rectTransform == null)
            {
                Debug.LogError("RectTranscorm is Null");
                return;
            }
            LeanTween.move(_rectTransform, moveTo, Time.deltaTime);
        }
        public void MoveCard(bool withTween, Vector3 moveTo, float seconds, bool? setActiveLater = null)
        {
            if (_rectTransform == null)
            {
                Debug.LogError("RectTranscorm is Null");
                return;
            }

            if (withTween)
            {
                if (setActiveLater == null)
                    LeanTween.move(_rectTransform, moveTo, seconds);
                else               
                    LeanTween.move(_rectTransform, moveTo, seconds).setOnComplete(() => _rectTransform.gameObject.SetActive(setActiveLater.GetValueOrDefault()));

            }
            else
                _rectTransform.position = Vector2.Lerp(_rectTransform.position, moveTo, seconds);
        }


        public void SetScale(Vector3 toScale, float delay)
        {
            if (_rectTransform == null)
            {
                //       Debug.LogError("RectTranscorm is Null");
                return;
            }
            LeanTween.scale(_rectTransform, toScale, delay);
        }
        public void SetPosition(in Vector3 setTo)
        {
            if (_rectTransform == null)
            {
                //Debug.LogError("RectTranscorm is Null");
                return;
            }
            _rectTransform.anchoredPosition3D = setTo;
        }
        public void SetRotation(Vector3 rotateTo)
        {
            if (_rectTransform == null)
            {
                //   Debug.LogError("RectTranscorm is Null");
                return;
            }
            _rectTransform.localRotation = Quaternion.Euler(rotateTo);
        }


       
    }
}