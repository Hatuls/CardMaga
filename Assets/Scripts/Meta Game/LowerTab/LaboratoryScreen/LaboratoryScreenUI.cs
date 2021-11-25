using UnityEngine;
using TMPro;
using System;

namespace UI.Meta.Laboratory
{
    public enum LabPanelsEnum
    {
        Deck = 0,
        Upgrade = 1,
        Fuse = 2,
     
    }
    public class LaboratoryScreenUI: TabAbst
    {
        #region Singleton
        private static LaboratoryScreenUI _instance;
        public static LaboratoryScreenUI Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("LaboratoryScreenUI is null!");

                return _instance;
            }
        }

        public LabPanelsEnum LabPanelsEnum { get => _labPanelsEnum; set => _labPanelsEnum = value; }

        private void Awake()
        {
            _instance = this;
        }
        #endregion

        #region Fields
        [SerializeField]
        GameObject _deckPanel;
        [SerializeField]
        GameObject _deckPanelTitle;
        [SerializeField]
        GameObject _upgradePanel;
        [SerializeField]
        GameObject _upgradePanelTitle;
        [SerializeField]
        GameObject _fusePanel;
        [SerializeField]
        GameObject _fusePanelTitle;
        TextMeshProUGUI _dismantleText;

        [SerializeField]
        LabPanelsEnum _labPanelsEnum;
        #endregion
        public override void Close()
        {
            gameObject.SetActive(false);
        }
        public override void Open()
        {
            OpenPanel(LabPanelsEnum.Upgrade);
            gameObject.SetActive(true);
        }


        #region ButtonSwitch
        public void OpenPanel(LabPanelsEnum screen)
        {
            switch (screen)
            {
                case LabPanelsEnum.Deck:
                    OpenDeckPanel();
                    break;
                case LabPanelsEnum.Upgrade:
                    OpenUpgradePanel();
                    break;
                case LabPanelsEnum.Fuse:
                    OpenFusePanel();
                    break;
                default:
                    throw new Exception("LabratoryScreenUI Unknown Enum");
            }
        }
        public void OpenPanel(int panelIndex)
        => OpenPanel((LabPanelsEnum)panelIndex);

        private void OpenDeckPanel()
        {
            LabPanelsEnum = LabPanelsEnum.Deck;
            CloseUpgradePanel();
            CloseFusePanel();
            _deckPanelTitle.gameObject.SetActive(true);
            _deckPanel.gameObject.SetActive(true);
        }
        private void OpenUpgradePanel()
        {
            LabPanelsEnum = LabPanelsEnum.Upgrade;
            CloseDeckPanel();
            CloseFusePanel();
            _upgradePanelTitle.gameObject.SetActive(true);
            _upgradePanel.gameObject.SetActive(true);

        }
        private void OpenFusePanel()
        {
            LabPanelsEnum = LabPanelsEnum.Fuse;
            CloseDeckPanel();
            CloseUpgradePanel();
            _fusePanelTitle.gameObject.SetActive(true);
            _fusePanel.gameObject.SetActive(true);
        }
        private void CloseDeckPanel()
        {
            _deckPanelTitle.gameObject.SetActive(false);
            _deckPanel.gameObject.SetActive(false);
        }
        private void CloseUpgradePanel()
        {
            _upgradePanelTitle.gameObject.SetActive(false);
            _upgradePanel.gameObject.SetActive(false);
        }
        private void CloseFusePanel()
        {
            _fusePanelTitle.gameObject.SetActive(false);
            _fusePanel.gameObject.SetActive(false);
        }
        #endregion
    }
}