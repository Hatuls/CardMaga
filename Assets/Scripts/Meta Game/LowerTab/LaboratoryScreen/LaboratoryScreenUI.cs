using UnityEngine;
using TMPro;

namespace UI.Meta.Laboratory
{
    public class LaboratoryScreenUI: TabAbst
    {
        #region Fields
        [SerializeField]
        DeckCollectionScreenUI _deckCollectionScreenUI;
        [SerializeField]
        FuseScreenUI _fuseScreenUI;
        [SerializeField]
        UpgradeScreenUI _upgradeScreenUI;
        [SerializeField]
        MaxScrollLength _labTabs;
        TextMeshProUGUI _dismantleText;
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