using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Meta.Settings
{
    public class SettingsScreenUI : TabAbst
    {
        public override void Close()
        {
            gameObject.SetActive(false);
        }

        public override void Open()
        {
            gameObject.SetActive(true);
        }
    }
}
