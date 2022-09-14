using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public class BodyPartVisualSO : BaseVisualSO
    {
        [Tooltip("Base SO that holds body parts and colors")]
        public BodyPartBaseVisualSO BaseSO;

        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        public Sprite[] BodyPartsBG;

        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        public Sprite[] BodyPartsInnerBG;
    }
}
