using UnityEngine;
namespace Battles.UI
{
    [CreateAssetMenu(fileName = "TextPopSettings", menuName = "ScriptableObjects/Settings")]
    public class TextPopUpSettingsSO : ScriptableObject
    {
        [SerializeField] float _disappearSpeed;
        [SerializeField] float _textMoveY;
        [SerializeField] float _textDisapearTimer;
        [SerializeField] Color _normalDamage;
        [SerializeField] Color _shield;
        [SerializeField] Color _money;
        [SerializeField] Color _criticalDamage;
        [SerializeField] Color _healing;



        public ref float GetTextDisappearTime =>ref _textDisapearTimer;
        public ref float GetTextMoveY =>ref _textMoveY;
        public ref float GetTextDisappearSpeed =>ref _disappearSpeed;
        public ref Color NormalDamage { get =>ref _normalDamage; }
        public ref Color Shield { get =>ref _shield; }
        public ref Color Money { get =>ref _money; }
        public ref Color CriticalDamage { get =>ref _criticalDamage; }
        public ref Color Healing { get =>ref _healing; }
    }

}