using CardMaga.UI;
using CardMaga.UI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TutorialDirector
{
    public class DragCardTutorial1 : BaseTutorialDirector
{
        protected override void UnsubscribeEvent()
        {
            HandUI.OnCardExecute -= CardExecute;
        }
        protected override void SubscribeEvent()
        {
            HandUI.OnCardExecute += CardExecute;
        }

        private void CardExecute(BattleCardUI battleCardUI)
        {
            StopDirector();
        }
    }

}
