using Characters.Stats;

namespace Keywords
{
    public class AttackKeyword : KeywordAbst
    {
        public override KeywordTypeEnum GetKeyword => KeywordTypeEnum.Attack;

        public override void ProcessOnTarget(bool isFromPlayer, bool isToPlayer, ref KeywordData keywordData)
        {
            if (keywordData != null)
            {
                //  CameraController.ShakeCamera();

                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + keywordData.GetTarget.ToString() + " recieved " + keywordData.KeywordSO.GetKeywordType.ToString() + " with Amount " + keywordData.GetAmountToApply);
                StatsHandler.GetInstance.RecieveDamage(
                    isToPlayer,
                    StatsHandler.GetInstance.GetCharacterStats(isFromPlayer).Strength + keywordData.GetAmountToApply
                    );
            }
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            switch (data.GetTarget)
            {
                case TargetEnum.MySelf:
                    StatsHandler.GetInstance.RecieveDamage(
                           currentPlayer,
                           StatsHandler.GetInstance.GetCharacterStats(!currentPlayer).Strength + data.GetAmountToApply
                           );

                    break;

                case TargetEnum.All:
                    StatsHandler.GetInstance.RecieveDamage(
                    currentPlayer,
                    StatsHandler.GetInstance.GetCharacterStats(!currentPlayer).Strength + data.GetAmountToApply
                    );

                    StatsHandler.GetInstance.RecieveDamage(
                    !currentPlayer,
                    StatsHandler.GetInstance.GetCharacterStats(currentPlayer).Strength + data.GetAmountToApply
                    );

                    break;

                case TargetEnum.Opponent:

                    StatsHandler.GetInstance.RecieveDamage(
                   !currentPlayer,
                    StatsHandler.GetInstance.GetCharacterStats(currentPlayer).Strength + data.GetAmountToApply
                    );
                    break;
                case TargetEnum.None:
                default:
                    break;
            }

        }
    }
}