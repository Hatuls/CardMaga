using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using CardMaga.Card;

namespace Battle.Combo
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
        private int _id;
        public int ID { get => _id; set=> _id = value; }

        [TabGroup("Recipe/General Info", "Data")]
        [LabelWidth(130)]
        [SerializeField]
        private string _comboName;
        public string ComboName { get=> _comboName; set=> _comboName =value; }


        [TabGroup("Recipe/General Info", "Data")]
        [ShowInInspector]
        [LabelWidth(130)]
        public RarityEnum GetRarityEnum => CraftedCard== null ? RarityEnum.None : CraftedCard.Rarity;


        [TabGroup("Recipe/General Info", "Data")]
        [LabelWidth(100)]
        [SerializeField]
        private int _cost;
        public int Cost { get => _cost; set=> _cost = value; }


        [TabGroup("Recipe/General Info", "Data")]
        [SerializeField]
        private Battle.Deck.DeckEnum _goToDeckAfterCrafting;
        public Battle.Deck.DeckEnum GoToDeckAfterCrafting { get => _goToDeckAfterCrafting; set=> _goToDeckAfterCrafting = value; }


        [TabGroup("Recipe/General Info", "Combo")]
        [SerializeField]
        [LabelWidth(20)]
        private CardTypeData[] _comboSequence;
        public CardTypeData[] ComboSequence { get => _comboSequence; set=> _comboSequence =value; }



        [TabGroup("Recipe/General Info", "Data")]
        [SerializeField]
        private CardSO _craftedCard;
        public CardSO CraftedCard { get => _craftedCard;
#if UNITY_EDITOR
            set => _craftedCard = value;
#endif
        }



#endregion

        #region Properties


        public List<string[]> GetDescription => CraftedCard.CardDescription(0);
        


        #endregion

    }
}
