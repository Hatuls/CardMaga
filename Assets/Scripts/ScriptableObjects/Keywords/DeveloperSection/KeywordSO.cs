
using UnityEngine;
namespace Keywords
{
    [CreateAssetMenu( fileName = "KeywordSO", menuName ="ScriptableObjects/Keywords" )]
    public class KeywordSO : ScriptableObject
    {
        #region Fields
        [Header("Keyword Information:")]

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
        public DurationEnum GetDurationEnum => _durationEnum;
        public KeywordTypeEnum GetKeywordType => _keyword;
        #endregion

    }


    public enum TargetEnum {
        MySelf=1,
        All=3, 
        None=0,
        Opponent=2 
    };
    public enum DurationEnum {
        Permanent,
        Instant,
        OverTurns,
        OverBattles
    };


}
