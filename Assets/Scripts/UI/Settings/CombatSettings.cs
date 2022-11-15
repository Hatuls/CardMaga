using Battle.Data;
using FMODUnity;
using UnityEngine;

namespace CardMaga.UI.Settings
{

    public class CombatSettings : BaseUIElement
    {
        [SerializeField]
        private CanvasLayerChanger _canvasLayerChanger;
        [SerializeField]
        private SurrenderScreen _surrenderScreen;
        [SerializeField]
        StudioGlobalParameterTrigger _sfkParameter;
        [SerializeField]
        StudioGlobalParameterTrigger _musicParameter;

        [SerializeField]
        private GameObject[] _surrenderGO;
        private void Awake()
        {
            if (BattleData.Instance.BattleConfigSO.IsTutorial)
                System.Array.ForEach(_surrenderGO, x => x.SetActive(false));
        }

        private bool _isSFKOn = true;
        private bool _isMusicOn = true;
        public void ToggleSFXSettings()
        {
            _isSFKOn = !_isSFKOn;
            _sfkParameter.value = (_isSFKOn) ? 0f : 1f;
            _sfkParameter.TriggerParameters();
        }
        public void ToggleMusicSettings()
        {
            _isMusicOn = !_isMusicOn;
            _musicParameter.value = (_isMusicOn) ? 0f : 1f;
            _musicParameter.TriggerParameters();
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