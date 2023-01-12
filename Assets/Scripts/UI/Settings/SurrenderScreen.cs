using CardMaga.Battle.UI;
using System;

namespace CardMaga.UI.Settings
{
    public class SurrenderScreen : BaseUIElement
    {
        public static event Action OnSurrenderPressed;
        [UnityEngine.SerializeField]
        private CanvasLayerChanger _canvasLayerChanger;
        public int Priority => 0;
        public void OpenSurrenderScreen()
        {

            UIHistoryManager.Show(this, true);
        }
        public void ReturnBack()
        {
            UIHistoryManager.ReturnBack();
        }
        public void Surrender()
        {
            UIHistoryManager.CloseAll();
            BattleUiManager.Instance.BattleDataManager.EndBattleHandler.ForceEndBattle(false);
            _canvasLayerChanger.Reset();
            OnSurrenderPressed?.Invoke();
            UIHistoryManager.CloseAll();
        }
    }
}

