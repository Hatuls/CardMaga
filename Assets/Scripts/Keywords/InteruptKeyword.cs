using Battle;

namespace Keywords
{

    public class InteruptKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Interupt;
        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            if (data != null)
            {
                UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);

                int length = data.GetAmountToApply;
                var target = data.GetTarget;
        
                PlayerCraftingSlots playerCraftingSlots;
                if (target == TargetEnum.All || target == TargetEnum.MySelf)
                {
                    playerCraftingSlots = (playersManager.GetCharacter(currentPlayer).DeckHandler[Battle.Deck.DeckEnum.CraftingSlots] as PlayerCraftingSlots);
                    for (int i = 0; i < length; i++)
                        playerCraftingSlots.PushSlots();
                }

                if (target == TargetEnum.Opponent || target == TargetEnum.All)
                {
                    playerCraftingSlots = (playersManager.GetCharacter(!currentPlayer).DeckHandler[Battle.Deck.DeckEnum.CraftingSlots] as PlayerCraftingSlots);
                    for (int i = 0; i < length; i++)
                        playerCraftingSlots.PushSlots();
                }
                data.KeywordSO.SoundEventSO.PlaySound();
            }
            else
                throw new System.Exception("Error Keyword Data is Null!! at "+ Keyword.ToString());
        }
    }
}