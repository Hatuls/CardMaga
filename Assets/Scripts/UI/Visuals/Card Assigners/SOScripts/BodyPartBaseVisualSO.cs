using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Body Parts Base SO", menuName = "ScriptableObjects/UI/Visuals/Body Part base Visual SO")]
    public class BodyPartBaseVisualSO : ScriptableObject
    {
        [Tooltip("Empty = 0, Head = 1, Elbow = 2, Hand = 3, Knee = 4, Leg = 5, Joker = 6")]
        public Sprite[] BodyParts;
        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        public Color[] MainColor;
        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        public Color[] InnerBGColor;
    }
    //None = 0, Empty = 1, Head = 2, Elbow = 3, Hand = 4, Knee = 5, Leg = 6, Joker = 7 
}
