using UnityEngine;
using UnityEngine.UI;
using Keywords;


namespace Relics
{
    public enum RelicNameEnum
    {
        NutShell = 0,
        KnockOut = 1,
        PowerUp = 2,
        Breath =3
    };
    public enum RelicType { Passive,Active };
    [CreateAssetMenu (fileName = "Relic", menuName = "ScriptableObjects/Relic")]
    public class RelicSO : ScriptableObject
    {
        #region Fields
        [Tooltip("Relic Name")]
        [SerializeField] RelicNameEnum _relicName;

        [Tooltip("Relic Type ")]
        [SerializeField] RelicType _relicType;

        [Tooltip("When is it activated?")]
        [SerializeField] WhenActivatedEnum _whenToActivate;

       // need to enter animation class;

        [Tooltip("Relic Rarity")]
        [SerializeField] Cards.RarityEnum _rarityLevel;

        [Tooltip("Relic's Icon")]
        [SerializeField] Image _icon;

        [Tooltip("Relic's Description")]
        [TextArea]
        [SerializeField] string _description;

        [Space]
        [Header("Combo:")]
        [Tooltip("The Order will define the accomplishment of the combo")]
        [SerializeField] Cards.BodyPartEnum[] _bodyPart;


        [Header("Keywords")]
        [Tooltip("Card's Keywords:")]
        [SerializeField] KeywordData[] _keywordEffect;

        #endregion
        #region Properties
        public RelicNameEnum GetRelicName => _relicName;
        public Cards.BodyPartEnum[] GetCombo => _bodyPart;
        public Cards.RarityEnum GetRarityEnum => _rarityLevel;
        public string GetDescription => _description;
        public Image GetIcon => _icon;
        public WhenActivatedEnum GetWhenToActivate => _whenToActivate;
        public KeywordData[] GetKeywordEffect => _keywordEffect;
        public RelicType GetRelicType => _relicType;
        #endregion

    }
}
