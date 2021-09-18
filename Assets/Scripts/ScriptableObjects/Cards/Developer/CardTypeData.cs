using Sirenix.OdinInspector;
using Sirenix.Serialization;
namespace Cards
{
    [System.Serializable]
    public class CardTypeData

    {
        [OdinSerialize]
        [ShowInInspector]
        public BodyPartEnum BodyPart { get; set; }
        [OdinSerialize]
        [ShowInInspector]
        public CardTypeEnum CardType { get; set; }



    }
}