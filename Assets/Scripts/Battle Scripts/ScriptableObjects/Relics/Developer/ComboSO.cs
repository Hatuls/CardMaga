using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Combo
{


    [CreateAssetMenu (fileName = "Combo", menuName = "ScriptableObjects/Combo")]
    public class ComboSO : ScriptableObject
    {

        #region Fields

        [TitleGroup("Recipe", "", TitleAlignments.Centered, boldTitle: true)]
        [TabGroup("Recipe/General Info", "Picture")]

        [PreviewField(100, ObjectFieldAlignment.Center)]
        [HideLabel]
        [SerializeField]
        private Sprite _img;
        public Sprite Image { get=> _img; set=> _img=value; }


        [TabGroup("Recipe/General Info", "Data")]
        [LabelWidth(130)]
        [SerializeField]
        private ushort _id;
        public ushort ID { get => _id; set=> _id = value; }

        [TabGroup("Recipe/General Info", "Data")]
        [LabelWidth(130)]
        [SerializeField]
        private string _comboName;
        public string ComboName { get=> _comboName; set=> _comboName =value; }


        [TabGroup("Recipe/General Info", "Data")]
        [ShowInInspector]
        [LabelWidth(130)]
        public Cards.RarityEnum GetRarityEnum => CraftedCard== null ? Cards.RarityEnum.None : CraftedCard.Rarity;


        [TabGroup("Recipe/General Info", "Data")]
        [LabelWidth(100)]
        [SerializeField]
        private int _cost;
        public int Cost { get => _cost; set=> _cost = value; }


        [TabGroup("Recipe/General Info", "Data")]
        [SerializeField]
        private Battles.Deck.DeckEnum _goToDeckAfterCrafting;
        public Battles.Deck.DeckEnum GoToDeckAfterCrafting { get => _goToDeckAfterCrafting; set=> _goToDeckAfterCrafting = value; }


        [TabGroup("Recipe/General Info", "Combo")]
        [SerializeField]
        [LabelWidth(20)]
        private Cards.CardTypeData[] _comboSequance;
        public Cards.CardTypeData[] ComboSequance { get => _comboSequance; set=> _comboSequance =value; }



        [TabGroup("Recipe/General Info", "Data")]
        [SerializeField]
        private Cards.CardSO _craftedCard;
        public Cards.CardSO CraftedCard { get => _craftedCard; set=> _craftedCard =value; }



        #endregion

        #region Properties


        public string GetDescription => CraftedCard.CardDescription(0);


        #endregion

    }
}
