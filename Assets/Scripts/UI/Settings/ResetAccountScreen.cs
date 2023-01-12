using Account;
using CardMaga.Core;
using ReiTools.TokenMachine;

namespace CardMaga.UI.Settings
{
    public class ResetAccountScreen : BaseUIElement
    {
        [UnityEngine.SerializeField]
        private SceneLoader _sceneLoader;
        [UnityEngine.SerializeField]
        private CanvasLayerChanger _canvasLayerChanger;
        public int Priority => 0;
        public void OpenAccountScreen()
        {
            Show();
        }
        public void ReturnBack()
        {
            Hide();
        }
        public void ResetAccount()
        {
            TokenMachine _tokenMachine = new TokenMachine(FinishReset);
            AccountManager.Instance.ResetAccount(_tokenMachine);
        }
        private void FinishReset()
        {
            UIHistoryManager.CloseAll();
            _canvasLayerChanger.Reset();
            _sceneLoader.LoadScene();
        }
    }
}

