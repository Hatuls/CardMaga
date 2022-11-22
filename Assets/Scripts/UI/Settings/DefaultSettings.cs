using UnityEngine;
namespace CardMaga.UI.Settings
{
    [RequireComponent(typeof(DefaultSettings))]
    public class DefaultSettings : BaseUIElement
    {
        [SerializeField]
        protected CanvasLayerChanger _canvasLayerChanger;
      

        public virtual void ShowSettings()
        {
            _canvasLayerChanger.PrioritizeLayer(true);
            UIHistoryManager.Show(this, true);
        }
        public virtual void ExitSettings()
        {
            UIHistoryManager.ReturnBack();
            _canvasLayerChanger.Reset();
        }
    }



}