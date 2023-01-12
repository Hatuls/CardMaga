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
            Show();
        }
        public virtual void ExitSettings()
        {
            Hide();
            _canvasLayerChanger.Reset();
        }
    }



}