
using UnityEngine;


namespace Map
{
    [CreateAssetMenu (fileName = "Chest", menuName = "ScriptableObjects/Map/Points/Chest")]
    public class ChestPoint : NodePointAbstSO
    {
        public override NodeType PointType =>  NodeType.Chest;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}