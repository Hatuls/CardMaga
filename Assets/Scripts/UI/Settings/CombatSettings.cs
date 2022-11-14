using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.UI.Settings
{

    public class CombatSettings : BaseUIElement
    {
      [SerializeField]
      private CanvasLayerChanger _canvasLayerChanger;
        [SerializeField]
        private SurrenderScreen _surrenderScreen;
        [SerializeField, EventsGroup]
        private UnityEvent OnToggleMusic;
        [SerializeField, EventsGroup]
        private UnityEvent OnToggleSFX;
        public void ToggleSFXSettings()
        {
            OnToggleSFX?.Invoke();
        }
        public void ToggleMusicSettings()
        {
            OnToggleMusic?.Invoke();
        }
        public void ShowSettings()
        {
            _canvasLayerChanger.PrioritizeLayer(true);
            UIHistoryManager.Show(this, true);
        //    ClickHelper.Instance.LoadObject(false, true, ExitSettings, this.transform as RectTransform);
        }
        public void ExitSettings()
        {
            UIHistoryManager.ReturnBack();
            _canvasLayerChanger.Reset(); 
        }
    }




}