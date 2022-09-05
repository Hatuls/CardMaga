using Battle;

namespace Keywords
{
    public class StunShardKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.StunShard;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {

            var target = data.GetTarget;


            data.KeywordSO.SoundEventSO.PlaySound();
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
        }
    }
        public class RageShardKeyword : KeywordAbst
        {
            public override KeywordTypeEnum Keyword => KeywordTypeEnum.RageShard;

            public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
            {

                var target = data.GetTarget;

                data.KeywordSO.SoundEventSO.PlaySound();
                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                {
                    playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
                }

                if (target == TargetEnum.All || target == TargetEnum.Opponent)
                    playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
            }
        }
        public class ProtectionShardKeyword : KeywordAbst
        {
            public override KeywordTypeEnum Keyword => KeywordTypeEnum.ProtectionShard;

            public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
            {

                var target = data.GetTarget;


                data.KeywordSO.SoundEventSO.PlaySound();
                if (target == TargetEnum.All || target == TargetEnum.MySelf)

                    playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);

                if (target == TargetEnum.All || target == TargetEnum.Opponent)
                    playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
            }

        }
    }
