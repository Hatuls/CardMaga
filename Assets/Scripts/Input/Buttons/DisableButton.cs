using UnityEngine;
using UnityEngine.UI;
namespace CardMaga.Input
{


    public class DisableButton : Button
    {
 
        [SerializeField] private Sprite _onDisable;

        public override void PointDown()
        {
            if (IsLock)
                return;
            base.PointDown();

        }
        public override void PointUp()
        {
            if (IsLock)
                return;
            base.PointUp();
        }

        [ContextMenu("Disable")]
        public void Disable()
        {
            Lock();
            _renderer.sprite = _onDisable;
        }
        [ContextMenu("Enable")]
        public void Enable()
        {
            UnLock();
            _renderer.sprite = _onIdle;
        }
    }

}