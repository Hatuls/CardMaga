
using UnityEngine;


namespace Map
{
    [CreateAssetMenu (fileName = "Chest", menuName = "ScriptableObjects/Map/Points/Chest")]
    public class ChestPoint : EventPointAbstSO
    {
        public override EventPointType PointType =>  EventPointType.Chest;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}