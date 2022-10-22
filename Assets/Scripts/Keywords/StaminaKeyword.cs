using Battle;

namespace Keywords
{
    public class StaminaKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Stamina;


        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
       
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);

 
                var target = data.GetTarget;
                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStamina(data.GetAmountToApply);
   
                if (target == TargetEnum.Opponent || target == TargetEnum.All)
                playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStamina(data.GetAmountToApply);

            data.KeywordSO.SoundEventSO.PlaySound();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {

            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStamina(-data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStamina(-data.GetAmountToApply);
        }
    }

    public class ShuffleKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Shuffle;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {

             //   UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);
                data.KeywordSO.SoundEventSO.PlaySound();
            
                var target = data.GetTarget;
                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                    playersManager.GetCharacter(currentPlayer).DeckHandler.ResetDeck( Battle.Deck.DeckEnum.Discard);

                if (target == TargetEnum.Opponent || target == TargetEnum.All)
                    playersManager.GetCharacter(!currentPlayer).DeckHandler.ResetDeck(Battle.Deck.DeckEnum.Discard);

        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
           // implement logic
        }
    }
}