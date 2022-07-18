using UnityEngine.UI;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class CardGlowVisualAssigner :BaseVisualAssigner
    {
        [SerializeField] GlowCardSO _glowCardSO;
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
            _glowImage.sprite = _glowCardSO.GlowSprite;
            _glowImage.color = _glowCardSO.GlowColor;
        }
        public void SetGlow(bool toActivate)
        {
            _glowImage.gameObject.SetActive(toActivate);
        }
    }
}
