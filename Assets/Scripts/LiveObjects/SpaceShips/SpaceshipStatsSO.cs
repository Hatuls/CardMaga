using CardMaga.UI;
using UnityEngine;

namespace CardMaga.LiveObjects.Spaceships
{
    [CreateAssetMenu(fileName = "SpaceShip Stats SO", menuName = "ScriptableObjects / LiveObjects / Spaceships / Spaceship Stats SO")]
    public class SpaceshipStatsSO: ScriptableObject, ICheckValidation
    {
        [SerializeField] float _maxSpeed;
        [SerializeField] float _minSpeed;
        [Sirenix.OdinInspector.InfoBox("Acceleration Affect Only Last Part of Path")]
        [SerializeField] float _maxAcceleration;
        [SerializeField] float _minAcceleration;

        public float GetSpeed()
        {
            var randomSpeed = Random.Range(_minSpeed, _maxSpeed);
            return randomSpeed;
        }
        public float GetAcceleration()
        {
            var randomAcceleration = Random.Range(_minAcceleration, _maxAcceleration);
            return randomAcceleration;
        }
        public void CheckValidation()
        {
            if (_maxSpeed <= 0)
                throw new System.Exception("SpaceshipStatsSO Max Speed has to be bigger than 0");
            if (_maxSpeed <= 0)
                throw new System.Exception("SpaceshipStatsSO Min Speed has to be bigger than 0");
        }
    }
}