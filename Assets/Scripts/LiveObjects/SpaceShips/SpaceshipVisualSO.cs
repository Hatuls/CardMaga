using CardMaga.UI;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.LiveObjects.Spaceships
{
    [CreateAssetMenu(fileName = "SpaceShip Visual SO", menuName = "ScriptableObjects / LiveObjects / Spaceships / Spaceship Visual SO")]
    public class SpaceshipVisualSO: ScriptableObject, ICheckValidation
    {
       [SerializeField] List<Sprite> _spaceshipSprites;
       [SerializeField] List<Sprite> _jetSprites;

        public void CheckValidation()
        {
            if (_spaceshipSprites == null)
                throw new System.Exception("SpaceshipVisualSO Has No Spaceship Sprites");
            if (_jetSprites == null)
                throw new System.Exception("SpaceshipVisualSO Has No Jet Sprites");
        }
        public Sprite GetSpaceShipSprite()
        {
            var randomSpaceshipIndex = Random.Range(0, _spaceshipSprites.Count);
            return _spaceshipSprites[randomSpaceshipIndex];
        }
        public Sprite GetJetSprite()
        {
            var randomJetSprite = Random.Range(0, _jetSprites.Count);
            return _jetSprites[randomJetSprite];
        }
    }
}