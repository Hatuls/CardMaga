using UnityEngine;
namespace CardMaga.UI.Settings
{
    [RequireComponent(typeof(DefaultSettings))]
    public class DefaultSettings : BaseUIElement
    {
        [SerializeField]
        protected CanvasLayerChanger _canvasLayerChanger;
      

        public void ShowSettings()
        {
            _canvasLayerChanger.PrioritizeLayer(true);
            UIHistoryManager.Show(this, true);
        }
        public void ExitSettings()
        {
            UIHistoryManager.ReturnBack();
            _canvasLayerChanger.Reset();
        }
    }



}