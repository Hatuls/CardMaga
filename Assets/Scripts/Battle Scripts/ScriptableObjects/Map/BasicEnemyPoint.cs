
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Basic Enemy", menuName = "ScriptableObjects/Map/Points/Basic Enemy")]
    public class BasicEnemyPoint : NodePointAbstSO
    {
        public override NodeType PointType =>  NodeType.Basic_Enemy;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}