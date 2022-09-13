using Battle.Deck;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    public class ComboTypeVisualSO: BaseVisualSO
    {
        [Tooltip("0 = Hand, 1 = Deck, 2 = Disacrd, 3 = Burst")]
        public string[] TypeNames;

        [Tooltip("0 = Hand, 1 = Deck, 2 = Disacrd, 3 = Burst")]
        public string[] TypeDescription;

        [Tooltip("0 = Hand, 1 = Deck, 2 = Disacrd, 3 = Burst")]
        public Sprite[] TypeSprite;

        public override void CheckValidation()
        {
            if (TypeNames == null)
                throw new System.Exception("ComboTypeVisualAssigner Does not have Type Names");

            if (TypeDescription == null)
                throw new System.Exception("ComboTypeVisualAssigner Does not have Type Description");

            if (TypeSprite == null)
                throw new System.Exception("ComboTypeVisualAssigner Does not have Type Image");
        }

        public string GetTypeName(DeckEnum comboType)
        {
            switch (comboType)
            {
                case DeckEnum.Hand:
                    return TypeNames[0];
                case DeckEnum.PlayerDeck:
                    return TypeNames[1];
                case DeckEnum.Discard:
                    return TypeNames[2];
                case DeckEnum.AutoActivate:
                    return TypeNames[3];
                default:
                    throw new System.Exception($"ComboTypeVisualSO GetTypeName Received {comboType.ToString()} Deck Enum");
            }
        }
        public string GetTypeDescription(DeckEnum comboType)
        {
            switch (comboType)
            {
                case DeckEnum.Hand:
                    return TypeDescription[0];
                case DeckEnum.PlayerDeck:
                    return TypeDescription[1];
                case DeckEnum.Discard:
                    return TypeDescription[2];
                case DeckEnum.AutoActivate:
                    return TypeDescription[3];
                default:
                    throw new System.Exception($"ComboTypeVisualSO GetTypeDescription Received {comboType.ToString()} Deck Enum");
            }
        }
        public Sprite GetTypeSprite(DeckEnum comboType)
        {
            switch (comboType)
            {
                case DeckEnum.Hand:
                    return TypeSprite[0];
                case DeckEnum.PlayerDeck:
                    return TypeSprite[1];
                case DeckEnum.Discard:
                    return TypeSprite[2];
                case DeckEnum.AutoActivate:
                    return TypeSprite[3];
                default:
                    throw new System.Exception($"ComboTypeVisualSO GetTypeSprite Received {comboType.ToString()} Deck Enum");
            }
        }
    }
}