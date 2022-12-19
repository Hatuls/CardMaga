
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.Input
{
    public class Button : TouchableItem
    {
        [SerializeField] protected Image _renderer;
        [Header("Sprites")]
        [SerializeField] protected Sprite _onPress;
        [SerializeField] protected Sprite _onIdle;


        protected override void PointDown()
        {
            _renderer.sprite = _onPress;
            base.PointDown();
        }
        protected override void PointUp()
        {
            _renderer.sprite = _onIdle;
            base.PointUp();
        }
    }    
}

