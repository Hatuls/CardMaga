using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Stamin BattleCard SO", menuName = "ScriptableObjects/UI/Visuals/Stamina BattleCard Visual SO")]
    public class StaminaCardSO : ScriptableObject
    {
        public Sprite[] StaminaBG;
        public Sprite[] StaminaInnerCircle;
        public Sprite[] StaminaFront;
    }
}
