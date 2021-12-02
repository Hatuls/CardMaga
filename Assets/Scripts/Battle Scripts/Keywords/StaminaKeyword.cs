using Characters.Stats;

namespace Keywords
{
    public class StaminaKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Stamina;


        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
       
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);

 
                var target = data.GetTarget;
                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                    StaminaHandler.Instance.AddStamina(currentPlayer, data.GetAmountToApply);
   
                if (target == TargetEnum.Opponent || target == TargetEnum.All)
                   StaminaHandler.Instance.AddStamina(!currentPlayer, data.GetAmountToApply);

            data.KeywordSO.PlaySound();
        }
    }

    public class ShuffleKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Shuffle;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            if (data != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);
                data.KeywordSO.PlaySound();

                var target = data.GetTarget;
                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                    Battles.Deck.DeckManager.Instance.ResetDeck(currentPlayer, Battles.Deck.DeckEnum.Disposal);

                if (target == TargetEnum.Opponent || target == TargetEnum.All)
                    Battles.Deck.DeckManager.Instance.ResetDeck(!currentPlayer, Battles.Deck.DeckEnum.Disposal);
            }
            else
            {
                throw new System.Exception($"ShuffleKeyword Data Is null!\nplayer: {currentPlayer}");
            }


        }
    }
}