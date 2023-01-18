using CardMaga.ObjectPool;
using UnityEngine;
namespace CardMaga.UI.PopUp
{
    [CreateAssetMenu(fileName = "New PopUpSO", menuName = "ScriptableObjects/PopUp/New PopUp SO")]
    public class PopUpSO : BasePoolSO<PopUp>
    {
        [SerializeField,Tooltip("Can this pop up be shown multiple time\nSetting it false means there can be only one popup of this type at a time")]
        private bool _isStackable;

        public bool IsStackable => _isStackable;
    }
}
