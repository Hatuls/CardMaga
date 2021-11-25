namespace UI.Meta.Laboratory
{
    public  class RemoveMetaCardUIOpener : MetaCardUIOpenerAbst
    {
        public override void OpenScreen(MetaCardUIHandler metaCardUIHandler)
        {

                metaCardUIHandler.InfoButton.SetActive(true);

                metaCardUIHandler.DismantleButton.SetActive(false);

                metaCardUIHandler.UseButton.SetActive(false);

        
                metaCardUIHandler.RemoveButton.SetActive(true);
        }
    }
}
