using Battle.Deck;
using CardMaga.Battle;

namespace CardMaga.Keywords
{
    public class StaminaKeyword : BaseKeywordLogic
    {
        public StaminaKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }


        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + target.ToString() + " recieved " + KeywordSO.GetKeywordType.ToString() + " with Amount of " + amount);


            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStamina(amount);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStamina(amount);

            KeywordSO.SoundEventSO.PlaySound();
        }

        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {

            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStamina(-amount);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStamina(-amount);
        }
    }

    public class ShuffleKeyword : BaseKeywordLogic
    {
        public ShuffleKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

  

        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            //   UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);
           KeywordSO.SoundEventSO.PlaySound();

      
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).DeckHandler.ResetDeck(DeckEnum.Discard);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).DeckHandler.ResetDeck(DeckEnum.Discard);
        }


        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}