using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public class BodyPartVisualSO : BaseVisualSO
    {
        [Tooltip("BasePoolObject SO that holds body parts and colors")]
        public BodyPartBaseVisualSO BaseSO;

        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        public Sprite[] BodyPartsBG;

        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        public Sprite[] BodyPartsInnerBG;

        public override void CheckValidation()
        {
            BaseSO.CheckValidation();

            if(BodyPartsBG == null)
                throw new System.Exception("BodyPartVisualSO has no BodyPartsBG");
            if (BodyPartsInnerBG == null)
                throw new System.Exception("BodyPartVisualSO has no BodyOartsInnerBG");
        }
    }
}
