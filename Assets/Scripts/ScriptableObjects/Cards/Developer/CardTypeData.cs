using Sirenix.OdinInspector;
namespace Cards
{
    [System.Serializable]
    public class CardTypeData
    {
        [ShowInInspector]
        public CardTypeEnum CardType { get; set; }

        [ShowInInspector]
        public BodyPartEnum BodyPart { get; set; }


    }
}