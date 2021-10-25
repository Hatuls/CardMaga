
using UnityEngine;


namespace Map
{
    [CreateAssetMenu (fileName = "Rest Area Point", menuName = "ScriptableObjects/Map/Points/Rest Area")]
    public class RestAreaPoint : EventPointAbstSO
    {
        public override EventPointType PointType => EventPointType.Rest_Area;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}