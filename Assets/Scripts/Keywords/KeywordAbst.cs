using Battle;
using Conditions;

namespace Keywords
{
    [System.Serializable]
    public abstract class KeywordAbst : IKeyword
    {

        public abstract KeywordTypeEnum Keyword { get; }

        public abstract void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager);

        public static bool CheckCondition(Condition con)
        {

            bool isValidCondition = true; ;
            if (con.Equals(typeof(ComboCondition)))
            {
                for (int i = 0; i < con.GetComboCondition.Length; i++)
                {
                    isValidCondition &= con.GetComboCondition[i].IsConditionMet();


                    if (isValidCondition == false)
                        break;
                }
            }
            else if (con.Equals(typeof(ParamCondition)))
            {
                for (int i = 0; i < con.GetParamaterCondition.Length; i++)
                {
                    isValidCondition &= con.GetParamaterCondition[i].IsConditionMet();


                    if (isValidCondition == false)
                        break;
                }
            }

            return isValidCondition;
        }
    }
    public interface IKeyword
    {
        void ProcessOnTarget(bool isFromPlayer, KeywordData keywordData, IPlayersManager playersManager);
        KeywordTypeEnum Keyword { get; }
    }
}