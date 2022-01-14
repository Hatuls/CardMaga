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

        [SerializeField]
        DeckCollectionScreenUI _deckCollectionScreenUI;

        [SerializeField]
        GameObject[] _gameObjectsToActivateOnLabratortyScreen;

        [SerializeField]
        UpgradeCardScreenUI _upgradeScreenUI;

        [SerializeField]
        CardUIInteractionHandle _cardUIInteractionHandler;

        [SerializeField]
        LabPanelsEnum _labPanelsEnum;
        [SerializeField] bool isOpeningLastPanel = false;
         TutorialChecker isFirstTimeOpeningLab;
        [SerializeField] GameObject _tutorialAnimation ;
#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button()]
        private void ResetTutorialFirstTime()
        {
            if (SaveManager.CheckFileExists("LabFirstTime", SaveManager.FileStreamType.FileStream))
            {
                SaveManager.DeleteFile("LabFirstTime", ".txt", SaveManager.FileStreamType.FileStream,"",true);
            }
        }
#endif
#endregion
        private void Start()
        {
            isFirstTimeOpeningLab = SaveManager.Load<TutorialChecker>("LabFirstTime", SaveManager.FileStreamType.FileStream);
            if (isFirstTimeOpeningLab == null)
            {
                isFirstTimeOpeningLab = new TutorialChecker();
                isFirstTimeOpeningLab.IsFirstTime = true;
            }
        }
        public override void Close()
        {
            for (int i = 0; i < _gameObjectsToActivateOnLabratortyScreen.Length; i++)
                _gameObjectsToActivateOnLabratortyScreen[i].SetActive(false);
            
            _cardUIInteractionHandler.UnSubscribe();
        }
        public override void Open()
        {
            _cardUIInteractionHandler.Subscribe();
            OpenPanel(_labPanelsEnum);
            for (int i = 0; i < _gameObjectsToActivateOnLabratortyScreen.Length; i++)
                _gameObjectsToActivateOnLabratortyScreen[i].SetActive(true);

            if (isFirstTimeOpeningLab.IsFirstTime)
            {
                isFirstTimeOpeningLab.IsFirstTime = false;
                SaveManager.SaveFile(isFirstTimeOpeningLab, "LabFirstTime", SaveManager.FileStreamType.FileStream);
                _tutorialAnimation.SetActive(true);
            }
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
            if(isOpeningLastPanel)
            LabPanelsEnum = LabPanelsEnum.Deck;
            CloseUpgradePanel();
            CloseFusePanel();
            _deckCollectionScreenUI.Open();
            _deckPanelTitle.gameObject.SetActive(true);
            _deckPanel.gameObject.SetActive(true);
    
        }
        private void OpenUpgradePanel()
        {
            if (isOpeningLastPanel)
                LabPanelsEnum = LabPanelsEnum.Upgrade;
            CloseDeckPanel();
            CloseFusePanel();
            _upgradePanelTitle.gameObject.SetActive(true);
            _upgradePanel.gameObject.SetActive(true);
            _upgradeScreenUI.Open();
        }
        private void OpenFusePanel()
        {
            if (isOpeningLastPanel)
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
            _deckCollectionScreenUI.Close();
        }
        private void CloseUpgradePanel()
        {
            _upgradeScreenUI.Close();
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
    [Serializable]
    public class TutorialChecker
    {
        public bool IsFirstTime;
    }
}