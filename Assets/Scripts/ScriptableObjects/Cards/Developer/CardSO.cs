using UnityEngine;
using Keywords;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
namespace Cards
{
    [CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/Cards")]
    public class CardSO : ScriptableObject
    {
      [OdinSerialize]
        [ShowInInspector]
 public int ID { get; set; }

        #region Fields
        [TitleGroup("CardData", BoldTitle =true, Alignment = TitleAlignments.Centered)]  
     
        [HorizontalGroup("CardData/Info/Display")]


        [Tooltip("The Image of the card:")]
        [VerticalGroup("CardData/Info/Display/Coulmn 1")]
        [PreviewField(100, Alignment = ObjectFieldAlignment.Right )]
        [SerializeField] Sprite CardSprite;
     
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
        public CardType CardType { get; set; }
        public RarityEnum CardsRarityLevel => CardType._rarityLevel;
        public BodyPartEnum BodyPartEnum => CardType._bodyPart;
        public CardTypeEnum CardTypeEnum => CardType._cardType;




        [TabGroup("CardData/Info", "Data")]

        [OdinSerialize]
        [ShowInInspector]
        public Battles.Deck.DeckEnum GoToDeckAfterCrafting { get; set; }

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

        public ref Sprite GetCardImage => ref CardSprite;
        
 
         public int CardsMaxLevel => PerLevelUpgrade == null ? 1 : PerLevelUpgrade.Length+1;
      
        #endregion




       

    }

    public enum LevelUpgradeEnum
    {
        None=0,
        Stamina=1,
        BodyPart=2,
        KeywordAddition=3,
        ConditionReduction=4,
    }

    public enum RarityEnum
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        LegendREI
    };
}