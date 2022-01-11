using UnityEngine;
namespace Keywords
{
    [CreateAssetMenu(fileName = "KeywordSO", menuName = "ScriptableObjects/Keywords")]
    public class KeywordSO : ScriptableObject
    {
        #region Fields
        [Header("Keyword Information:")]
        [SerializeField]
        private int _iD;
        [SerializeField]
        bool _ignoreInfoAmmount;

        [Tooltip("what duration is it ?")]
        [SerializeField] DurationEnum _durationEnum;

        [Tooltip("If this keyword exist should it be added to the previous one?")]
        [SerializeField] bool _isStackable;

        [Tooltip("If this is true then the amount to apply will be used as a precentage")]
        [SerializeField] bool _isPrecentage;

        [Tooltip("What Effect does it do")]
        [SerializeField] KeywordTypeEnum _keyword;

        [SerializeField] byte _infoAmount;

        [SerializeField] string[] _descriptions;

        [SerializeField] SoundEventSO _soundEvent;

        [SerializeField]
        private VFXSO _vfx;
        #endregion

        #region Properties
        public SoundEventSO SoundEventSO { get => _soundEvent; set => _soundEvent = value; }
        public int ID => _iD;
        public byte InfoAmount => _infoAmount;
        public bool GetIsStackable => _isStackable;
        public bool GetIsPrecentage => _isPrecentage;
        public DurationEnum GetDurationEnum => _durationEnum;
        public KeywordTypeEnum GetKeywordType => _keyword;
        public string KeywordName => _keyword.ToString();

        public bool IgnoreInfoAmount => _ignoreInfoAmmount;
        public VFXSO GetVFX() => _vfx;
        public string GetDescription(params int[] amount)
        {
            if (IgnoreInfoAmount)
                return _descriptions[0];

            string info = string.Empty;
            for (int i = 0; i < _descriptions.Length; i++)
            {
                string amountData = string.Empty;

                if (i < amount.Length)
                    amountData = amount[i].ToString();

                info += string.Concat(_descriptions[i], amountData);
            }

            return info;
        }
        #endregion


#if UNITY_EDITOR

        public bool Init(string[] Data)
        {

            const int IDIndex = 0;
            const int DurationIndex = 1;
            const int StackableIndex = 2;
            const int PrecentageIndex = 3;
            const int InfoAmountIndex = 4;
            const int IgnoreInfoAmountIndex = 5;
            const int DescriptionIndex = 6;
            const int VFXIndex = 7;


            if (int.TryParse(Data[IDIndex], out int keywordID))
            {
                _iD = keywordID;
                _keyword = (KeywordTypeEnum)keywordID;
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

            if (byte.TryParse(Data[InfoAmountIndex], out byte amount))
                _infoAmount = amount;

            if (byte.TryParse(Data[IgnoreInfoAmountIndex], out byte toIgnore))
                _ignoreInfoAmmount = System.Convert.ToBoolean(toIgnore);
            else
                throw new System.Exception($"KeywordsSO:\nID: {_iD}\n Ignore info amount on keyword is not a valid number!");

            _descriptions = Data[DescriptionIndex].Replace('^', ',').Split('#');
            if (Data[VFXIndex].Length > 1)
            {
                _vfx = Resources.Load<VFXSO>("VFX/Keywords VFX/" + Data[VFXIndex]);
            }
            return true;
        }

#endif
    }


    public enum TargetEnum
    {
        None = 0,
        MySelf = 1,
        Opponent = 2,
        All = 3,
    };
    public enum DurationEnum
    {
        Permanent,
        Instant,
        OverTurns,
        OverBattles
    };


}
