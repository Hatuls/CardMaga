using CardMaga.UI;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.LiveObjects.Spaceships
{
    [System.Serializable]
    public class SpaceshipHandler : ICheckValidation
    {

        [SerializeField] SpaceshipVisualSO _spaceshipVisualSO;
        [SerializeField] SpaceshipStatsSO _spaceshipStatsSO;
        [SerializeField] List<Path> _avilablePaths;
        [SerializeField] JetDirectionSO _jetDirection;
        [Sirenix.OdinInspector.ReadOnly] [SerializeField] List<Path> _activePaths;
        public void CheckValidation()
        {
            _spaceshipVisualSO.CheckValidation();
            _spaceshipStatsSO.CheckValidation();

            if (_avilablePaths == null)
                throw new System.Exception("SpaceshipHandler has no Paths");
            if (_jetDirection == null)
                throw new System.Exception("SpaceshipHandler has no JetDirection");
        }
        public void SendSpaceship(Spaceship spaceship)
        {
            if (TryGetRandomPath(out Path path))
            {
                InitSpaceship(spaceship,path);
                path.IsOccupied = true;
                _activePaths.Add(path);
                _avilablePaths.Remove(path);
                spaceship.OnReachedDestination += RecivePath;
            }
            else
            {
                Debug.LogWarning("SpaceshipHandler does not have enough paths, spaceship will not be sent");
            }
        }
        public void RecivePath(Spaceship spaceship)
        {
            spaceship.Path.IsOccupied = false;
            spaceship.OnReachedDestination -= RecivePath;
            _avilablePaths.Add(spaceship.Path);
            _activePaths.Remove(spaceship.Path);
        }
        void InitSpaceship(Spaceship spaceship, Path path)
        {
            spaceship.Init(path, _spaceshipVisualSO.GetSpaceShipSprite(), _spaceshipVisualSO.GetJetSprite(),_jetDirection,
                        _spaceshipStatsSO.GetSpeed(), _spaceshipStatsSO.GetAcceleration());
        }
        bool TryGetRandomPath(out Path path)
        {
            if (_avilablePaths.Count <=0)
            {
                path = null;
                return false;
            }
            else
            {
                var randomPathIndex = Random.Range(0, _avilablePaths.Count);
                path = _avilablePaths[randomPathIndex];
                return true;
            }
        }
    }
}