using CardMaga.VFX;
using UnityEngine;

namespace CardMaga.Keywords
{
    [CreateAssetMenu(fileName = "KeywordSO", menuName = "ScriptableObjects/Keywords")]
    public class KeywordSO : ScriptableObject
    {
        #region Fields
        [Header("Keyword Information:")]
        [SerializeField]
        private int _iD;
        [SerializeField]
        bool _ignoreInfoAmount;

        [Tooltip("what duration is it ?")]
        [SerializeField] EffectType _durationEnum;

        [Tooltip("If this keyword exist should it be added to the previous one?")]
        [SerializeField] bool _isStackable;

        [Tooltip("If this is true then the amount to apply will be used as a precentage")]
        [SerializeField] bool _isPrecentage;

        [Tooltip("What Effect does it do")]
        [SerializeField] KeywordType _keyword;

        [SerializeField] byte _infoAmount;

        [SerializeField] string[] _descriptions;

        [SerializeField] SoundEventSO _soundEvent;

        [SerializeField] private VisualEffectSO _vfx;
        #endregion

        #region Properties
        public SoundEventSO SoundEventSO { get => _soundEvent; set => _soundEvent = value; }
        public int ID => _iD;
        public byte InfoAmount => _infoAmount;
        public bool GetIsStackable => _isStackable;
        public bool GetIsPrecentage => _isPrecentage;
        public EffectType GetDurationEnum => _durationEnum;
        public KeywordType GetKeywordType => _keyword;
        public string KeywordName => _keyword.ToString();

        public bool IgnoreInfoAmount => _ignoreInfoAmount;
        public VisualEffectSO GetVFX() => _vfx;
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
                _keyword = (KeywordType)keywordID;
            }
            else
                return false;

            if (int.TryParse(Data[DurationIndex], out int duration))
                _durationEnum = (EffectType)duration;
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
                _ignoreInfoAmount = System.Convert.ToBoolean(toIgnore);
            else
                throw new System.Exception($"KeywordsSO:\nID: {_iD}\n Ignore info amount on keyword is not a valid number!");

            _descriptions = Data[DescriptionIndex].Replace('^', ',').Split('#');
            if (Data[VFXIndex].Length > 1)
            {
                _vfx = Resources.Load<VisualEffectSO>("VFX/Keywords VFX/" + Data[VFXIndex]);
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
    public enum EffectType
    {
        OnActivate = 0,
        OnMyStartTurn,
        OnMyEndTurn,
        OnOpponentStartTurn,
        OnOpponentEndTurn,
    };


}
