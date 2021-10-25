
using System.Collections;
using TMPro;
using UnityEngine;
namespace Battles.UI
{
    public enum TextType { NormalDMG, CritDMG, Healing, Money, Shield }
    public class TextPopUp : MonoBehaviour
    {
        [SerializeField] RectTransform _rectTransform;
        private float disappearTimer;
        [SerializeField] TextMeshProUGUI textMesh;

        private Color color;
        float moveYSpeed = 2f;
        float disappearSpeed = 3f;


        public void Create(ref Color color, Vector3 Position, string Amount)
        {
            _rectTransform.anchoredPosition = Position;
            Setup(ref color, Amount);
        }
   

        public void ResetTextsPopUp()
        {
            gameObject.SetActive(false);
           StopAllCoroutines();
        }
        private void Setup(ref Color clr, string txt)
        {
            color = clr;
            textMesh.SetText(txt);
            moveYSpeed= TextPopUpHandler.GetInstance.GetTextSettings.GetTextMoveY;
            disappearSpeed = TextPopUpHandler.GetInstance.GetTextSettings.GetTextDisappearSpeed;
            disappearTimer = TextPopUpHandler.GetInstance.GetTextSettings.GetTextDisappearTime;
            StartCoroutine(TextAnimations());
        }
        private IEnumerator TextAnimations()
        {
            textMesh.color = color;
            while (disappearTimer > 0)
            {
                yield return null;
                _rectTransform.position += Vector3.up  * moveYSpeed;
                color.a -= disappearSpeed*Time.deltaTime;
                textMesh.color = color;

                if(color.a <= 0)
                {
                    gameObject.SetActive(false);
                    yield break;
                }

            }
        }
    }

}