using UnityEngine;

namespace UI
{
    public class TabUI : MonoBehaviour
    {
        [SerializeField]
        TabHandler _tabHandler;
        [SerializeField]
        TabAbst _tabToOpen;
        public void Open()
        {
            _tabHandler.CloseAllOtherTabsThan(this);
            _tabToOpen.Open();
        }
        public void Close()
        {
            _tabToOpen.Close();
        }
    }
}
