using UnityEngine;
using TMPro;

namespace UI.Meta.Laboratory
{
    public class LaboratoryScreenUI: TabAbst
    {
        #region Fields
        DeckCollectionScreenUI _deckCollectionScreenUI;
        FuseScreenUI _fuseScreenUI;
        //UpgradeScreenUI _upgradeScreenUI;
        TextMeshProUGUI _dismantleText;
        [SerializeField]
        MaxScrollLength _labTabs;
        public override void Close()
        {
            gameObject.SetActive(false);
        }
        public override void Open()
        {
            gameObject.SetActive(true);
        }
        #endregion
    }
}