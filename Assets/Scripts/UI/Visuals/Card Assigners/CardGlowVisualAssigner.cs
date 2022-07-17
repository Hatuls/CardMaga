using UnityEngine.UI;
using UnityEngine;

namespace UI.Visuals
{
    [System.Serializable]
    public class CardGlowVisualAssigner :BaseVisualAssigner
    {
        [SerializeField] GlowCardSO GlowCardSO;
        [SerializeField] Image _glowImage;
        public override void Init()
        {
            if (_glowImage == null)
                throw new System.Exception("CardGlowVisualAssigner has no cardGlow");
            SetGlow();
        }
        private void SetGlow()
        {
            //assign sprite
            _glowImage.sprite = GlowCardSO._glowSprite;
            _glowImage.color = GlowCardSO._glowColor;
        }
        public void SetGlow(bool toActivate)
        {
            _glowImage.gameObject.SetActive(toActivate);
        }
    }
}
