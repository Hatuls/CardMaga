using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Meta.Workshop
{
    public class WorkshopScreenUI : TabAbst
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
