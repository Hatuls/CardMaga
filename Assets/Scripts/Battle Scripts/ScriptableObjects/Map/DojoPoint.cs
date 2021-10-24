
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Dojo Point", menuName = "ScriptableObjects/Map/Points/Dojo")]
    public class DojoPoint : EventPointAbstSO
    {
        public override EventPointType PointType => EventPointType.Dojo;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}