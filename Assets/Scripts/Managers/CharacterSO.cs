using UnityEngine;
namespace Battles
{
    [CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character")]
    public class CharacterSO : CharacterAbstSO
    {
        [SerializeField] int _maxMana;
        public int GetMaxStamina { get => _maxMana; }
    }
}
