using UnityEngine;
using Keywords;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
namespace Cards
{
    [CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/Cards")]
    public class CardSO : SerializedScriptableObject
    {


        #region Fields
        [TitleGroup("CardData", BoldTitle =true, Alignment = TitleAlignments.Centered)]  
     
        [HorizontalGroup("CardData/Info/Display")]

        [OdinSerialize]
        [ShowInInspector]
        [VerticalGroup("CardData/Info/Display/Coulmn 1")]
        [PreviewField(100, Alignment = ObjectFieldAlignment.Right )]
        public Sprite CardSprite { get; set; }

        [VerticalGroup("CardData/Info/Display/Coulmn 2")]
        [LabelWidth(90f)]
        [OdinSerialize]
        [ShowInInspector]
        public string CardName { get; set; }

        [TabGroup("CardData/Info", "Animation")]
        [OdinSerialize]
        [ShowInInspector]
        public AnimationBundle AnimationBundle { get; set; }


        [TabGroup("CardData/Info", "Data")]
        [OdinSerialize]
        [ShowInInspector]
        public RarityEnum Rarity { get; set; }

        [TabGroup("CardData/Info", "Data")]
        [OdinSerialize]
        [ShowInInspector]
        public CardTypeData CardType { get; set; }
 
        public BodyPartEnum BodyPartEnum => CardType.BodyPart;
        public CardTypeEnum CardTypeEnum => CardType.CardType;





        [VerticalGroup("CardData/Info/Display/Coulmn 2")]
        [OdinSerialize]
        [ShowInInspector] 
        [LabelWidth(80f)]
        public string CardDescription { get; set; }
   


        [TabGroup("CardData/Info", "Data")]
        [OdinSerialize]
        [ShowInInspector]
        public int StaminaCost { get; set; }

        [TabGroup("CardData/Info", "Data")]
        [OdinSerialize]
        [ShowInInspector]
        public int PurchaseCost { get; set; }

   

        [TabGroup("CardData/Info", "Data")]
        [OdinSerialize]
        [ShowInInspector]
        public bool ToExhaust { get; set; }

        [TabGroup("CardData/Info", "Data")]
        [Tooltip("How much coins the card cost")]
        [SerializeField]
        int _salvageCost = 1;

        [TabGroup("CardData/Info", "Data")]
        [OdinSerialize]
        [ShowInInspector]
        public int ID { get; set; }

        [TabGroup("CardData/Info", "Keywords")]
        [OdinSerialize]
        [ShowInInspector]
        public KeywordData[] CardSOKeywords { get; set; }

        [TabGroup("CardData/Info", "Levels")]
        [OdinSerialize]
        [ShowInInspector]
        public PerLevelUpgrade[] PerLevelUpgrade { get; set; }

        [TabGroup("CardData/Info", "Crafting")]
        [OdinSerialize]
        [ShowInInspector] 
         public int[] CardsFusesFrom { get; set; }

        #endregion

        #region Properties




        [ShowInInspector]
        public int CardsMaxLevel => PerLevelUpgrade == null ? 0 : PerLevelUpgrade.Length;
      
        #endregion

    }

    public enum LevelUpgradeEnum
    {
        None=0,
        Stamina=1,
        KeywordAddition=2,
        ConditionReduction=3,
        ToRemoveExhaust = 4,
        BodyPart = 5,
    }

    public enum RarityEnum
    {
        None = 0,
        Common = 1,
        Uncommon = 2,
        Rare= 3,
        Epic=4,
        LegendREI=5
    };
}