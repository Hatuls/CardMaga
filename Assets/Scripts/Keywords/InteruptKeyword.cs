namespace Keywords
{
    public class InteruptKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Interupt;
        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);

                int length = data.GetAmountToApply;
                var target = data.GetTarget;
                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                {
                    var craftingslots = Battle.Deck.DeckManager.GetCraftingSlots(currentPlayer);
                    for (int i = 0; i < length; i++)
                        craftingslots.PushSlots();
                }

                if (target == TargetEnum.Opponent || target == TargetEnum.All)
                {
                    var craftingslot = Battle.Deck.DeckManager.GetCraftingSlots(!currentPlayer);
                    for (int i = 0; i < length; i++)
                        craftingslot.PushSlots();
                }
                data.KeywordSO.SoundEventSO.PlaySound();
            }
            else
                throw new System.Exception("Error Keyword Data is Null!! at "+ Keyword.ToString());
        }
    }
}