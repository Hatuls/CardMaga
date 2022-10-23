using Battle;

namespace Keywords
{
    public class StunShardKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.StunShard;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {

            data.KeywordSO.SoundEventSO.PlaySound();
            var target = data.GetTarget;

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);
            

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Reduce(data.GetAmountToApply);


            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword).Reduce(data.GetAmountToApply);
        }
    }
        public class RageShardKeyword : BaseKeywordLogic
        {
            public override KeywordTypeEnum Keyword => KeywordTypeEnum.RageShard;

            public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
            {

                data.KeywordSO.SoundEventSO.PlaySound();
                var target = data.GetTarget;

                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                    playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);
                

                if (target == TargetEnum.All || target == TargetEnum.Opponent)
                    playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);
            }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Reduce(data.GetAmountToApply);


            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword).Reduce(data.GetAmountToApply);
        }
    }
        public class ProtectionShardKeyword : BaseKeywordLogic
        {
            public override KeywordTypeEnum Keyword => KeywordTypeEnum.ProtectionShard;

            public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
            {

                data.KeywordSO.SoundEventSO.PlaySound();
                var target = data.GetTarget;
                if (target == TargetEnum.All || target == TargetEnum.MySelf)

                    playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);

                if (target == TargetEnum.All || target == TargetEnum.Opponent)
                    playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);
            }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)

                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Reduce(-data.GetAmountToApply);

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword).Reduce(data.GetAmountToApply);
        }
    }
    }
