
using UnityEngine;


namespace Map
{
    [CreateAssetMenu (fileName ="EventPointCollection", menuName ="ScriptableObjects/Collections/Map Event Points Collection")]
    public class EventPointCollectionSO : ScriptableObject
    {
        [SerializeField] EventPointAbstSO[] _points;
        public EventPointAbstSO[] EventPoints => _points;

        public EventPointAbstSO GetEventPoint(EventPointType type)
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