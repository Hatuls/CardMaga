using UnityEngine;


namespace Battle
{
    [CreateAssetMenu(fileName = "Battle SO" , menuName = "ScriptableObjects/Battle Config/Battle Data SO")]
    public class BattleSO : ScriptableObject
    {
        [SerializeField]
        int _staminaShardsToProduceStamina = 3;
        public int StaminaShardsToProduceStamina => _staminaShardsToProduceStamina;
    }
}