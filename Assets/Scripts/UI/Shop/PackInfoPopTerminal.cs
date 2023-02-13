using CardMaga.Battle.Players;
using CardMaga.MetaUI.Shop;
using CardMaga.Rewards;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    public class PackInfoPopTerminal : BasePopUpTerminal
    {
        [SerializeField]
        private LocationTagSO _startingLocation;
        protected override Vector2 GetStartLocation()
      => PopUpManager.Instance.GetPosition(_startingLocation);

        protected override void Start()
        {
            base.Start();
            PackDealUI.OnPackInfoPopupRequired += ShowPopUp;
            PackDealUI.OnPackInfoPopupFinished += HidePopUp;
        }
        protected override void OnDestroy()
        {
            PackDealUI.OnPackInfoPopupFinished -= HidePopUp;
            PackDealUI.OnPackInfoPopupRequired -= ShowPopUp;
            base.OnDestroy();
        }
        protected void ShowPopUp(PackDealUI bundle)
        {
            HidePopUp();
            base.ShowPopUp();
            _currentActivePopUp.GetComponent<PackInfoPopup>().InitText(bundle.RarirtyContainer);
        }
    }

}