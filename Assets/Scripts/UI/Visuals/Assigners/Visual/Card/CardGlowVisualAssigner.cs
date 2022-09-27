using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using CardMaga.Card;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class CardGlowVisualAssigner : BaseVisualAssigner<CardData>
    {
        [SerializeField] GlowCardSO _glowCardSO;
        [SerializeField] Image _glowImage;

        public override void CheckValidation()
        {
            if (_glowImage == null)
                throw new System.Exception("CardGlowVisualAssigner has no cardGlow");
        }

        public override void Init(CardData data)
        {
            _glowImage.sprite = _glowCardSO.GlowSprite;
            _glowImage.color = _glowCardSO.GlowColor;
        }

        public override void Dispose()
        {
        }
    }
}
