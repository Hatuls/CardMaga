using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class CardImageVisualAssigner : BaseVisualAssigner
    {
        [SerializeField] Image _cardSplash;
        public override void Init()
        {
            if (_cardSplash == null)
                throw new System.Exception("CardImageVisualAssigner Card Spalsh object is null");
        }
        public void SetSplashImage(Sprite splashSprite)
        {
            AssignSprite(_cardSplash, splashSprite);
        }
    }
}
