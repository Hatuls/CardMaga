using UnityEngine;
using Sirenix.OdinInspector;

namespace Relics
{
    public enum RelicNameEnum
    {
        NutShell = 0,
        KnockOut = 1,
        PowerUp = 2,
        Breath =3
    };

    [CreateAssetMenu (fileName = "Relic", menuName = "ScriptableObjects/Relic")]
    public class RelicSO : ScriptableObject
    {

        #region Fields
  
        [TitleGroup("Recipe","", TitleAlignments.Centered,boldTitle: true)]
        [TabGroup("Recipe/General Info","Picture")]

        [PreviewField(100,ObjectFieldAlignment.Center)]
        [HideLabel]
        [Tooltip("Recipe's Icon")]
        [SerializeField] Sprite _icon;


        [TabGroup("Recipe/General Info","Data")]
        [LabelWidth(130)]
        [Tooltip("Relic Name")]
        [SerializeField] RelicNameEnum _relicName;


        [TabGroup("Recipe/General Info", "Data")]
        [Tooltip("Relic Rarity")]
        [LabelWidth(130)]
        [SerializeField] Cards.RarityEnum _rarityLevel;


        [TabGroup("Recipe/General Info", "Data")]
        [LabelWidth(100)]
        [SerializeField] int _cost;


        [TabGroup("Recipe/General Info", "Data")]
        [Tooltip("Combo Description")]
        [TextArea]
        [SerializeField] string _description;


        [TabGroup("Recipe/General Info", "Combo")]
        [Header("Combo:")]
        [Tooltip("The Order will define the accomplishment of the combo")]
        [LabelWidth(20)]
        [SerializeField] Cards.BodyPartEnum[] _bodyPart;

      
        [Space(100)]
        [BoxGroup]
        [InlineEditor]
        [SerializeField] Cards.CardSO _craftedCard;

        #endregion

        #region Properties
        public Cards.BodyPartEnum[] GetCombo => _bodyPart;
        public Cards.RarityEnum GetRarityEnum => _rarityLevel;
        public Sprite GetIcon => _icon;
        public int Cost => _cost;
        public RelicNameEnum GetRelicName => _relicName;
        public string GetDescription => _description;
        public Cards.CardSO GetCraftedCard => _craftedCard;

        #endregion

    }
}
