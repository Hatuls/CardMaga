using System;
using UnityEngine;

namespace UI.Meta.Laboratory
{
    public class CollectionLabOpen : MetaCardUIOpenerAbst
    {
        [SerializeField]
        MetaCardUIOpenerAbst _upgradeOpener;

        [SerializeField]
        MetaCardUIOpenerAbst _fuseOpener;
        [SerializeField]
        MetaCardUIOpenerAbst _deckOpener;
       
        public override  void OpenScreen(MetaCardUIHandler metaCardUIHandler)
        {
            switch (LaboratoryScreenUI.Instance.LabPanelsEnum)
            {
                case LabPanelsEnum.Deck:
                    _deckOpener.OpenScreen(metaCardUIHandler);
                    break;
                case LabPanelsEnum.Upgrade:
                    _upgradeOpener.OpenScreen(metaCardUIHandler);
                    break;
                case LabPanelsEnum.Fuse:
                    _fuseOpener.OpenScreen(metaCardUIHandler);
                    break;
               
                default:
                    throw new NotImplementedException();
            }
        }


    }
}
