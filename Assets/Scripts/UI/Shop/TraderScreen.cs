using CardMaga.UI;
using CardMaga.UI.PopUp;
using System;
using UnityEngine;
namespace CardMaga.MetaUI.Shop
{

    public class TraderScreen : BaseUIScreen
    {
        [SerializeField]
        private ConfirmPurchasePopUp _confirmPurchasePopUp;
        [SerializeField]
        private GameObject _blackPanel;

        private void Awake()
        {
            _confirmPurchasePopUp.OnHide += CloseBlackPanel;
            _confirmPurchasePopUp.OnShow += ShowBlackPanel;
            PackDealUI.OnTryBuy += OpenPurchasePopUpScreen;
        }

        private void OnDestroy()
        {
            PackDealUI.OnTryBuy -= OpenPurchasePopUpScreen;
            _confirmPurchasePopUp.OnHide -= CloseBlackPanel;
            _confirmPurchasePopUp.OnShow -= ShowBlackPanel;
        }


        private void ShowBlackPanel()
        {
            _blackPanel.SetActive(true);
        }

        private void CloseBlackPanel()
        {
            _blackPanel.SetActive(false);
        }

        public override void CloseScreen()
        { 
            _confirmPurchasePopUp.Hide();
            base.CloseScreen();
            PopUpManager.Instance.CloseAllPopups();
        }
  

        private void OpenPurchasePopUpScreen(IDealable dealable)
        {
            _confirmPurchasePopUp.Show();
            _confirmPurchasePopUp.Init(dealable);
        }

    }
}