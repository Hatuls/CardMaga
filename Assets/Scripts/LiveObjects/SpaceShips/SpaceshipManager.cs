using CardMaga.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.LiveObjects.Spaceships
{
    public class SpaceshipManager : MonoBehaviour, ICheckValidation
    {
        [SerializeField] int _shipsToCreate = 5;
        [SerializeField] Spaceship _prefab;
        [SerializeField] GameObject _spaceshipsContainer;
        [Sirenix.OdinInspector.ReadOnly] [SerializeField] List<Spaceship> _activeSpaceships;
        [Sirenix.OdinInspector.ReadOnly] [SerializeField] List<Spaceship> _availableSpaceships;
        [SerializeField] List<SpaceshipHandler> _spaceshipHandlers;
        [SerializeField] SpaceshipManagerSO _spaceshipManagerSO;

        //when amount of spaceships are less than minimum send a spaceship regardless of the timer
        //have a short cooldown when having to send the spaceships
        //after sending the spaceship start cooldown
        //if cooldown is finished check if amount of spaceships are more than max
        //if true wait until spaceship amount is less than max
        //if false send spaceship
        
        IEnumerator StartSpaceshipCooldown(float cooldownTime)
        {
            yield return new WaitForSeconds(cooldownTime);

            SendSpaceship();
            //send spaceship
        }
        private void Start()
        {
            CheckValidation();
            for (int i = 0; i < _shipsToCreate; i++)
            {
                CreateSpaceship();
            }

            StartCoroutine(StartSpaceshipCooldown(_spaceshipManagerSO.GetCooldown()));
        }
        public void CheckValidation()
        {
            if (_prefab == null)
                throw new System.Exception("SpaceshipManager has no spaceship Prefab");
            if(_spaceshipsContainer==null)
                throw new System.Exception("SpaceshipManager has no spaceship Container");
            if(_spaceshipHandlers==null)
                throw new System.Exception("SpaceshipManager has no spaceship Handlers");
            foreach (var handler in _spaceshipHandlers)
            {
                handler.CheckValidation();
            }
        }
        public void ReciveSpaceship(Spaceship spaceship)
        {
            _availableSpaceships.Add(spaceship);
            _activeSpaceships.Remove(spaceship);

            CheckForNextSpaceship();
        }

        void CheckForNextSpaceship()
        {
            if (_activeSpaceships.Count <= _spaceshipManagerSO.MinShips)
            {
                SendSpaceship();
            }
        }
        void SendSpaceship()
        {
            if(_availableSpaceships.Count>0)
            {
                var randomHandler = Random.Range(0, _spaceshipHandlers.Count);
                _spaceshipHandlers[randomHandler].SendSpaceship(_availableSpaceships[0]);
                _activeSpaceships.Add(_availableSpaceships[0]);
                _availableSpaceships.RemoveAt(0);
            }
            else
            {
                CreateSpaceship();
                SendSpaceship();
            }

            //StartCoroutine(StartSpaceshipCooldown(_spaceshipManagerSO.GetCooldown()));
            //check if avilable
            //if true send spaceship to a random handler
            //if false create a spaceship and then send it
        }
        public virtual void CreateSpaceship()
        {
            //initialize a new spaceship in unity if all of the spaceships are active
            var spaceship = Instantiate(_prefab, _spaceshipsContainer.transform);
            spaceship.OnReachedDestination += ReciveSpaceship;
            _availableSpaceships.Add(spaceship);
        }
        private void OnDestroy()
        {
            foreach (var ship in _activeSpaceships)
            {
                ship.OnReachedDestination -= ReciveSpaceship;
            }
            foreach (var ship in _availableSpaceships)
            {
                ship.OnReachedDestination -= ReciveSpaceship;
            }
        }
    }
}