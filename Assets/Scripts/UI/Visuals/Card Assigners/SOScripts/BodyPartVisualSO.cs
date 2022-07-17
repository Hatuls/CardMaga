using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Visuals
{
    public class BodyPartVisualSO : ScriptableObject
    {
        [Tooltip("Base SO that holds body parts and colors")]
        public BodyPartBaseVisualSO BaseSO;

        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        public Sprite[] BodyPartsBG;

        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        public Sprite[] BodyPartsInnerBG;
    }
}
