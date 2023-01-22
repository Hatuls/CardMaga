using Account.GeneralData;
using CardMaga.Card;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Body Parts BattleCard SO", menuName = "ScriptableObjects/UI/Visuals/Body Part BattleCard Visual SO")]
    public class BodyPartCardVisualSO : BodyPartVisualSO
    {
        //any addition specific for cards will be implemented here

        public Sprite[] GoldBodyPartImages;

        public Sprite GetBodyPart(CardCore core)
        {
            var cardType = core.CardSO.CardTypeEnum;
            if (core.CardSO.IsCombo)
            {
                
                if (GoldBodyPartImages.Length <= (int)cardType)
                {
                    return GoldBodyPartImages[0];
                }
                else
                {
                    switch (cardType)
                    {
                        case CardTypeEnum.Utility:
                            return GoldBodyPartImages[3];
                        case CardTypeEnum.Defend:
                            return GoldBodyPartImages[2];
                        case CardTypeEnum.Attack:
                            return GoldBodyPartImages[1];
                        case CardTypeEnum.None:
                            return GoldBodyPartImages[0];
                        default:
                            throw new System.Exception($"BodyPartVisualSO Have recived Unknown BattleCard Type {cardType}");
                    }
                }
            }
            else
                return base.GetBodyPartBG(cardType);
        }    
    }
}
