using System;
using UnityEngine;

namespace UI
{
    public class TabHandler : MonoBehaviour
    {
        [SerializeField]
        TabUI[] _tabs;

        public void CloseAllOtherTabsThan(TabUI tab)
        {
            if(tab == null)
            {
                throw new Exception("TabHandler tab entered is null");
            }
            for (int i = 0; i < _tabs.Length; i++)
            {
                if(_tabs[i] != tab)
                {
                    _tabs[i].Close();
                }
            }
        }
    }
}
