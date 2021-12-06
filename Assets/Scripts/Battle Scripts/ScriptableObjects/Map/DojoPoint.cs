
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Dojo Point", menuName = "ScriptableObjects/Map/Points/Dojo")]
    public class DojoPoint : NodePointAbstSO
    {
        public override NodeType PointType => NodeType.Dojo;

        public override void ActivatePoint()
        {
            throw new System.NotImplementedException();
            OnEnterNode?.PlaySound();
        }
    }
}