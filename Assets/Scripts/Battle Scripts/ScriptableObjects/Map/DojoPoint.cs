
using UnityEngine;


namespace Map
{
    [CreateAssetMenu(fileName = "Dojo Point", menuName = "ScriptableObjects/Map/Points/Dojo")]
    public class DojoPoint : NodePointAbstSO
    {
        public override NodeType PointType => NodeType.Dojo;

        [Sirenix.OdinInspector.Button("Open Dojo")]

        public override void ActivatePoint()
        {
            OnEnterNode?.PlaySound();
            DojoManager.Instance.InitDojo();
        }
    }
}