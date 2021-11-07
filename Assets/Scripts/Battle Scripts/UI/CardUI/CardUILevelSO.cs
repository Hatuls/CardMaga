using UnityEngine;
namespace Battles.UI.CardUIAttributes
{
    [CreateAssetMenu(fileName = "Card UI Level Art",menuName = "ScriptableObjects/Art/CardUI/Levels")]
    public class CardUILevelSO : ScriptableObject
    {
        public Sprite OffImage;
        public Sprite OnImage;
        public Sprite MissingImage;
    }
}