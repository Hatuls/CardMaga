using CardMaga.UI.Visuals;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI
{
    [System.Serializable]
    public class CardGlowHandler
    {
        [SerializeField] GlowCardSO _glowCardSO;
        [SerializeField] Image _glowImage;
        public void ChangeGlowState(bool toActivate)
        {
            _glowImage.gameObject.SetActive(toActivate);
        }

        public void DiscardGlowAlpha()
        {
            _glowImage.DOFade(_glowCardSO.DiscardAplha, _glowCardSO.DiscardAplhaDuration);
        }

        public void ResetGlowAlpha()
        {
            _glowImage.color = _glowImage.color.SetColorAlpha(_glowCardSO.DefaultAplha);
        }
    }
}
