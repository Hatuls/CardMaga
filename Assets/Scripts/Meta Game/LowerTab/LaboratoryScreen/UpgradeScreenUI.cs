using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Meta.Laboratory
{
    public class UpgradeScreenUI : TabAbst
    {
        public override void Open()
        {
            gameObject.SetActive(true);
        }
        public override void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
