using UnityEngine;
using System;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Buff Type Visual SO", menuName = "ScriptableObjects/UI/Visuals/Buff Type Visual SO")]

    public class BuffTypeVisualSO : BaseVisualSO
    {
        [Tooltip("None = 0,Large Holder = 1,Small Holder = 2")]
        public Sprite[] Holders;
        [Tooltip("None = 0,Buff = 1,Debuff = 2")]
        public Sprite[] LargeBG;
        [Tooltip("None = 0,Buff = 1,Debuff = 2")]
        public Sprite[] SmallBG;
        public override void CheckValidation()
        {
            if (Holders == null)
                throw new Exception("BuffTypeVisualSO has no holders");
            if (LargeBG == null)
                throw new Exception("BuffTypeVisualSO has no large BG");
            if (SmallBG == null)
                throw new Exception("BuffTypeVisualSO has no Small BG");
        }
    }
}