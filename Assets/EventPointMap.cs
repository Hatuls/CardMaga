using Map;
using UnityEngine;
namespace UI.Map
{
    [CreateAssetMenu(fileName = "Map Pre Set", menuName = "ScriptableObjects/Map/LoadOuts")]
    public class EventPointMap : ScriptableObject
    {
        [SerializeField]
        NodePointAbstSO[] _startFloor;
        [SerializeField]
        NodePointAbstSO[] _firstFloor;
        [SerializeField]
        NodePointAbstSO[] _secondsFloor;
        [SerializeField]
        NodePointAbstSO[] _thirdFloor;
        [SerializeField]
        NodePointAbstSO[] _forthFloor;
        [SerializeField]
        NodePointAbstSO[] _fifthFloor;
        [SerializeField]
        NodePointAbstSO[] _sixthFloor;
        [SerializeField]
        NodePointAbstSO[] _seventhFloor;
        [SerializeField]
        NodePointAbstSO[] _eighthFloor;
        public NodePointAbstSO[] GetFloorEventPointSO(byte floor)
        {
            switch (floor)
            {
                case 0:
                    return _startFloor;
                case 1:
                    return _firstFloor;
                case 2:
                    return _secondsFloor;
                case 3:
                    return _thirdFloor;
                case 4:
                    return _fifthFloor;
                case 5:
                    return _sixthFloor;
                case 6:
                    return _seventhFloor;
                case 7:
                    return _eighthFloor;

                default:
                    throw new System.Exception($"EventPointsSet: {floor} was out of range!");
            }
        }
    }
}