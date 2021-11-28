using System;
using UnityEngine;

namespace UI
{
    public class TabHandler : MonoBehaviour
    {
        [SerializeField]
        TabUI[] _tabs;
        [SerializeField]
        Sprite _defaultBackground;
        [SerializeField]
        Sprite _highlightBackground;

       
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
                    _tabs[i].BackGroundImage.sprite = _defaultBackground;
                }
                else
                {

                    _tabs[i].BackGroundImage.sprite = _highlightBackground;
                }
            }
        }
    }
}
