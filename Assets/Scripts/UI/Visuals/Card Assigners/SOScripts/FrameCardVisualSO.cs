using UnityEngine;

namespace UI.Visuals
{
    [CreateAssetMenu(fileName = "Frame Card SO", menuName = "ScriptableObjects/UI/Visuals/Frame Card Visual SO")]
    public class FrameCardVisualSO : ScriptableObject
    {
        public Sprite[] Frames;
    }
}
