using UnityEngine;


namespace Battles
{
    [CreateAssetMenu(fileName = "Battle SO" , menuName = "ScriptableObjects/Battles/Battle Data SO")]
    public class BattleSO : ScriptableObject
    {
        [SerializeField]
        int _staminaShardsToProduceStamina = 3;
        public int StaminaShardsToProduceStamina => _staminaShardsToProduceStamina;
    }
}