using UnityEngine;


namespace Battles.UI.CardUIAttributes
{
    public class CardTranslations
    {
       // public Vector2 cardReturnPosition;
        RectTransform _rectTransform;
        public CardTranslations( RectTransform rectTransform)
        {
            _rectTransform = rectTransform;
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
                {
                    MoveCardX(moveTo.x, seconds);
                    MoveCardY(moveTo.y, seconds);
                    MoveCardZ(moveTo.z, seconds);
                }
                else
                {
                    MoveCardX(moveTo.x, seconds);
                    MoveCardY(moveTo.y, seconds,true);
                    MoveCardZ(moveTo.z, seconds);
                }       
               
            }
            else
                _rectTransform.position = Vector2.Lerp(_rectTransform.position, moveTo, seconds);
        }
        public void CancelAllTweens()
        {
            LeanTween.cancelAll(true);
        }
        public void MoveCardZ(float destination, float time)
        {
            LeanTween.moveZ(_rectTransform, destination, time);
        }
        public void MoveCardX(float destination, float time, bool? SetActiveLater= null ,LeanTweenType type = LeanTweenType.notUsed)
        {
            if (SetActiveLater == null)
                LeanTween.moveX(_rectTransform, destination, time).setEase(type);
            else
                LeanTween.moveX(_rectTransform, destination, time).setEase(type).setOnComplete(() => _rectTransform.gameObject.SetActive(SetActiveLater.GetValueOrDefault()));
        }
        public void MoveCardY(float destination, float time, bool? SetActiveLater = null, LeanTweenType type = LeanTweenType.notUsed)
        {
            if (SetActiveLater == null)
                LeanTween.moveY(_rectTransform, destination, time).setEase(type);
            else
                LeanTween.moveY(_rectTransform, destination, time).setEase(type).setOnComplete(() => _rectTransform.gameObject.SetActive(SetActiveLater.GetValueOrDefault()));
        }
        public void SetScale(Vector3 toScale, float delay, LeanTweenType type = LeanTweenType.notUsed)
        {
            if (_rectTransform == null)
            {
                //       Debug.LogError("RectTranscorm is Null");
                return;
            }
            LeanTween.scale(_rectTransform, toScale, delay).setEase(type);
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


       public void SetRotation(float zRotation, float time , LeanTweenType type = LeanTweenType.notUsed, System.Action action = null)
        {
            LeanTween.rotate(_rectTransform.gameObject,Vector3.forward* zRotation, time).setEase(type).setOnComplete(action);
        }
    }
}