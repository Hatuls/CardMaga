
using UnityEngine;
namespace Keywords
{
    [CreateAssetMenu( fileName = "KeywordSO", menuName ="ScriptableObjects/Keywords" )]
    public class KeywordSO : ScriptableObject
    {
        #region Fields
        [Header("Keyword Information:")]
        private int _iD;

        [Tooltip("what duration is it ?")]
        [SerializeField] DurationEnum _durationEnum;

        [Tooltip("If this keyword exist should it be added to the previous one?")]
        [SerializeField] bool _isStackable;

        [Tooltip("If this is true then the amount to apply will be used as a precentage")]
        [SerializeField] bool _isPrecentage;

        [Tooltip("What Effect does it do")]
        [SerializeField] KeywordTypeEnum _keyword;

        [SerializeField] int _infoAmount;
        #endregion

        #region Properties
        public int ID => _iD;
        public int InfoAmount => _infoAmount;
        public bool GetIsStackable => _isStackable;
        public bool GetIsPrecentage => _isPrecentage;
        public DurationEnum GetDurationEnum => _durationEnum;
        public KeywordTypeEnum GetKeywordType => _keyword;
        #endregion

        public bool Init(string[] Data)
        {

            const int IDIndex = 0;
            const int DurationIndex = 1;
            const int StackableIndex = 2;
            const int PrecentageIndex = 3;
            const int InfoAmountIndex =4;

            if (int.TryParse(Data[IDIndex], out int keywordID))
            {
                _iD = keywordID;
                _keyword = (KeywordTypeEnum)_iD;
            }
            else
                return false;

            if (int.TryParse(Data[DurationIndex], out int duration))
                _durationEnum = (DurationEnum)duration;
            else
                throw new System.Exception($"ID:{_iD}, Keyword: {_keyword}\nDuration is not a valid number!");

            if (int.TryParse(Data[StackableIndex], out int stackable))
                  _isStackable = stackable == 1;
           else
                throw new System.Exception($"ID:{_iD}, Keyword: {_keyword}\nIs Stackable is not a valid number!");

            if (int.TryParse(Data[PrecentageIndex], out int pecentage))
                _isPrecentage = pecentage == 1;
            else
            throw new System.Exception($"ID:{_iD}, Keyword: {_keyword}\nIs Precentage is not a valid number!");

            if (int.TryParse(Data[InfoAmountIndex], out int amount))
                _infoAmount = amount;
            else
                throw new System.Exception($"ID:{_iD}, Keyword: {_keyword}\nInfo Amount is not a valid Number!");

            return true;
        }
    }


    public enum TargetEnum {
        None=0,
        MySelf=1,
        Opponent=2 ,
        All=3, 
    };
    public enum DurationEnum {
        Permanent,
        Instant,
        OverTurns,
        OverBattles
    };

    
}
