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


                var target = data.GetTarget;
                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                {
                    for (int i = 0; i < data.GetAmountToApply; i++)
                        Battles.Deck.DeckManager.GetCraftingSlots(currentPlayer).PushSlots();
                }

                if (target == TargetEnum.Opponent || target == TargetEnum.All)
                {
                    for (int i = 0; i < data.GetAmountToApply; i++)
                             Battles.Deck.DeckManager.GetCraftingSlots(!currentPlayer).PushSlots();
                }
            }
            else
                throw new System.Exception("Error Keyword Data is Null!! at "+ Keyword.ToString());
        }
    }
}