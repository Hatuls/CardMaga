using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Stamin Card SO", menuName = "ScriptableObjects/UI/Visuals/Stamina Card Visual SO")]
    public class StaminaCardSO : ScriptableObject
    {
        public Sprite[] StaminaBG;
        public Sprite[] StaminaInnerCircle;
        public Sprite[] StaminaFront;
    }
}
