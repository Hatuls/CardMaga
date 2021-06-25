
using UnityEngine;
namespace Keywords
{
    [CreateAssetMenu( fileName = "KeywordSO", menuName ="ScriptableObjects/Keywords" )]
    public  class KeywordSO : ScriptableObject
    {
        #region Fields
        [Header("Keyward Information:")]
        [Tooltip("When is it activated?")]
        [SerializeField] WhenActivatedEnum _whenToActivate;

        [Tooltip("what duration is it ?")]
        [SerializeField] DurationEnum _durationEnum;

        [Tooltip("If this keyword exist should it be added to the previous one?")]
        [SerializeField] bool _isStackable;

        [Tooltip("If this is true then the amount to apply will be used as a precentage")]
        [SerializeField] bool _isPrecentage;

        [Tooltip("What Effect does it do")]
        [SerializeField] KeywordTypeEnum _keyword;
        #endregion

        #region Properties
        public bool GetIsStackable => _isStackable;
        public bool GetIsPrecentage => _isPrecentage;
        public WhenActivatedEnum GetWhenToActivate => _whenToActivate;
        public DurationEnum GetDurationEnum => _durationEnum;
        public KeywordTypeEnum GetKeywordType => _keyword;
        #endregion

    }
    public enum TargetEnum {
        Player,
        All, 
        None,
        Enemy 
    };
    public enum DurationEnum {
        Permanent,
        Instant,
        OverTurns,
        OverBattles
    };
    public enum WhenActivatedEnum { 
        StartOfTurn,
        EndOfTurn,
        AtCard,
        EndOfCombat,
        StartOfCombat
    };

}
