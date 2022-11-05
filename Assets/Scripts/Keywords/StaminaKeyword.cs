using Battle.Deck;
using CardMaga.Battle;

namespace CardMaga.Keywords
{
    public class StaminaKeyword : BaseKeywordLogic
    {
        public StaminaKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);


            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStamina(data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStamina(data.GetAmountToApply);

            data.KeywordSO.SoundEventSO.PlaySound();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStamina(-data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStamina(-data.GetAmountToApply);
        }
    }

    public class ShuffleKeyword : BaseKeywordLogic
    {
        public ShuffleKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            //   UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);
            data.KeywordSO.SoundEventSO.PlaySound();

            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).DeckHandler.ResetDeck(DeckEnum.Discard);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).DeckHandler.ResetDeck(DeckEnum.Discard);

        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            // implement logic
        }
    }
}