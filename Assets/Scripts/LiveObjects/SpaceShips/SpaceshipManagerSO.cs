using CardMaga.UI;
using UnityEngine;

namespace CardMaga.LiveObjects.Spaceships
{
    [CreateAssetMenu(fileName = "SpaceShip Manager SO", menuName = "ScriptableObjects / LiveObjects / Spaceships / Spaceship Manager SO")]
    public class SpaceshipManagerSO : ScriptableObject, ICheckValidation
    {
        public int MaxShips;
        public int MinShips;
        public float MaxCooldown;
        public float MinCooldown;
        public void CheckValidation()
        {
            if (MaxShips <= 0)
                throw new System.Exception("SpaceshipManagerSO MaxShips can not be 0 or lower than that");
            if (MinShips > MaxShips)
                throw new System.Exception("SpaceshipManagerSO MinShips can not be Higher than MaxShips");
            if(MaxCooldown<0)
                throw new System.Exception("SpaceshipManagerSO MaxCooldown can not be lower than 0");
            if (MinCooldown>MaxCooldown)
                throw new System.Exception("SpaceshipManagerSO MinCooldown can not be Higher than Max Cooldown");
        }
        public float GetCooldown()
        {
            var randomCooldown = Random.Range(MinCooldown, MaxCooldown);
            return randomCooldown;
        }
    }
}