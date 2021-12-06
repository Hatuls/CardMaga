
using Map.UI;
using UnityEngine;


namespace Map
{
    [CreateAssetMenu (fileName = "Rest Area Point", menuName = "ScriptableObjects/Map/Points/Rest Area")]
    public class RestAreaPoint : NodePointAbstSO
    {
        public override NodeType PointType => NodeType.Rest_Area;

        [Sirenix.OdinInspector.Button]
        public override void ActivatePoint()
        {
                        OnEnterNode.PlaySound();
            RestAreaUI.Instance.EnterRestArea();
        }
    }
}