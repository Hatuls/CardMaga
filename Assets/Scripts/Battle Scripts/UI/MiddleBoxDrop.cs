
using UnityEngine;

using UnityEngine.EventSystems;
namespace Battle.UI
{
    [RequireComponent(typeof(EventTrigger))]
    public class MiddleBoxDrop : MonoBehaviour { 
        [SerializeField] BoxCollider2D boxCollider2D;
        [SerializeField] Unity.Events.StringEvent _playSound;

  //      public void OnPointerExit() => CardUIManager.Instance.IsTryingToPlace = false;
        public void OnDrop()
        {

        }


    }

}

