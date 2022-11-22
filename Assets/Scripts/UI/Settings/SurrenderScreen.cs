using CardMaga.Battle.UI;

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
        }
    }
}

