using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "HandZoom Positions SO", menuName = "ScriptableObjects/UI/Visuals/ZoomPositionsSO")]
    public class ZoomPositionsSO : ScriptableObject
    {
        [Tooltip("Attack = 0, Defense = 1, Utility = 2")]
        public float[] YStartPosition;
        [Tooltip("Attack = 0, Defense = 1, Utility = 2")]
        public float[] YEndPosition;
    }
}
