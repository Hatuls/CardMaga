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

        [OdinSerialize]
        [ShowInInspector]
        public Sprite Image { get; set; }


        [TabGroup("Recipe/General Info", "Data")]
        [LabelWidth(130)]
        [OdinSerialize]
        [ShowInInspector]
        public int ID { get; set; }

        [TabGroup("Recipe/General Info", "Data")]
        [LabelWidth(130)]
        [OdinSerialize]
        [ShowInInspector]
       public string ComboName { get; set; }


        [TabGroup("Recipe/General Info", "Data")]
        [ShowInInspector]
        [LabelWidth(130)]
        public Cards.RarityEnum GetRarityEnum => CraftedCard== null ? Cards.RarityEnum.None : CraftedCard.Rarity;


        [TabGroup("Recipe/General Info", "Data")]
        [LabelWidth(100)]
        [ShowInInspector]
        public int Cost { get; set; }


        [TabGroup("Recipe/General Info", "Data")]

        [OdinSerialize]
        [ShowInInspector]
        public Battles.Deck.DeckEnum GoToDeckAfterCrafting { get; set; }


        [TabGroup("Recipe/General Info", "Combo")]
        [OdinSerialize]
        [ShowInInspector]
        [LabelWidth(20)]
        public Cards.CardTypeData[] ComboSequance { get; set; }



        [TabGroup("Recipe/General Info", "Data")]

        [OdinSerialize]
        [ShowInInspector]
        public Cards.CardSO CraftedCard { get; set; }

        #endregion

        #region Properties


        public string GetDescription => CraftedCard.CardDescription;


        #endregion

    }
}
