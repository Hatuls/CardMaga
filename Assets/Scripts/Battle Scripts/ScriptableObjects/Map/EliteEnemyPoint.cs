
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Elite Enemy", menuName = "ScriptableObjects/Map/Points/Elite Enemy")]
    public class EliteEnemyPoint : NodePointAbstSO
    {
        public override NodeType PointType =>  NodeType.Elite_Enemy;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
        }
    }
}