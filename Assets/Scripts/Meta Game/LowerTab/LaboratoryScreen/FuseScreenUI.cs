﻿using UnityEngine;

namespace UI.Meta.Laboratory
{
    public class FuseScreenUI : TabAbst
    {
        #region Fields
        GameObject _upgradeScreenPanel;
        #endregion
        #region Interface
        public override void Open()
        {
            gameObject.SetActive(true);
        }
        public override void Close()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}