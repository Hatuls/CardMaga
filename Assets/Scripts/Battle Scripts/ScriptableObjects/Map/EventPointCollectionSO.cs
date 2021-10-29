
using Map;
using UnityEngine;


namespace Collections
{
    [CreateAssetMenu (fileName ="EventPointCollection", menuName ="ScriptableObjects/Collections/Map Event Points Collection")]
    public class EventPointCollectionSO : ScriptableObject
    {
        [SerializeField] NodePointAbstSO[] _points;
        public NodePointAbstSO[] EventPoints => _points;

        public NodePointAbstSO GetEventPoint(NodeType type)
        {
            for (int i = 0; i < _points.Length; i++)
            {
                if (type == _points[i].PointType)
                    return _points[i];
            }
            throw new System.Exception($"Event Point Collection: Type: {type} was not found or the collection was empty!");
        }
    }
}