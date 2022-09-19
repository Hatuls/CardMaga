using CardMaga.UI;
using UnityEngine;

namespace CardMaga.LiveObjects.Spaceships
{
    public class Jet : MonoBehaviour, ICheckValidation
    {
        [SerializeField] SpriteRenderer _jetSprite;
        private void Start()
        {
            CheckValidation();
            ActivateJet(false);
        }
        public void CheckValidation()
        {
            if (_jetSprite == null)
                throw new System.Exception("Jet has no jetSprite");
        }
        public void Init(Sprite sprite)
        {
            _jetSprite.AssignSprite(sprite);
        }
        public void ActivateJet(bool toActivate)
        {
            _jetSprite.transform.gameObject.SetActive(toActivate);
        }
        private void OnDisable()
        {
            ActivateJet(false);
        }
    }
}
namespace CardMaga.LiveObjects
{
    public static class SpriteRendererHelper
    {
        public static  SpriteRenderer AssignSprite(this SpriteRenderer spriteRenderer,Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            return spriteRenderer;
        }
    }
}