using CardMaga.Card;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class CardImageVisualAssigner : BaseVisualAssigner<BattleCardData>
    {
        [SerializeField] Image _cardSplash;

        public override void CheckValidation()
        {
            if (_cardSplash == null)
                throw new System.Exception("CardImageVisualAssigner BattleCard Spalsh object is null");
        }

        public override void Dispose()
        {
        }

        public override void Init(BattleCardData battleCardData)
        {
            _cardSplash.AssignSprite(battleCardData.CardSO.CardSprite);
        }
    }
}
