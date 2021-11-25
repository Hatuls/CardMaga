using UnityEngine;

namespace UI.Meta.Laboratory
{
    public class FuseLabOpen : MetaCardUIOpenerAbst
    {
        public override void OpenScreen(MetaCardUIHandler metaCardUIHandler)
        {
            if (metaCardUIHandler.InfoButton.activeSelf == false)
                metaCardUIHandler.InfoButton.SetActive(true);

            if (metaCardUIHandler.DismantleButton.activeSelf)
                metaCardUIHandler.DismantleButton.SetActive(false);

            if (!metaCardUIHandler.UseButton.activeSelf)
                metaCardUIHandler.UseButton.SetActive(true);

                  if (metaCardUIHandler.RemoveButton.activeSelf)
                metaCardUIHandler.RemoveButton.SetActive(false);
        }
    }
}
