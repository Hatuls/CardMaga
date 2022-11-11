using CardMaga.Battle;
using CardMaga.Battle.UI;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using System;

namespace CardMaga.UI.Settings
{
    public class SurrenderScreen : BaseUIElement
    {
        [UnityEngine.SerializeField]
        private CanvasLayerChanger _canvasLayerChanger;
        public int Priority => 0;
        public void OpenSurrenderScreen()
        {

            UIHistoryManager.Show(this, true);
            //    ClickHelper.Instance.LoadObject(false, true, ReturnBack, this.transform as RectTransform);
        }
        public void ReturnBack()
        {
            UIHistoryManager.ShowLast();
 
        }
        public void Surrender()
        {
            BattleUiManager.Instance.BattleDataManager.EndBattleHandler.ForceEndBattle(true);
            UIHistoryManager.CloseAll();
            _canvasLayerChanger.Reset();
        }
    }
}

