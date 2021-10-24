using Map;
using UnityEngine;
namespace UI.Map
{
    [CreateAssetMenu(fileName = "Map Pre Set", menuName = "ScriptableObjects/Map/LoadOuts")]
    public class EventPointMap : ScriptableObject
    {
        [SerializeField]
        EventPointAbstSO[] _startFloor;
        [SerializeField]
        EventPointAbstSO[] _firstFloor;
        [SerializeField]
        EventPointAbstSO[] _secondsFloor;
        [SerializeField]
        EventPointAbstSO[] _thirdFloor;
        [SerializeField]
        EventPointAbstSO[] _forthFloor;
        [SerializeField]
        EventPointAbstSO[] _fifthFloor;
        [SerializeField]
        EventPointAbstSO[] _sixthFloor;
        [SerializeField]
        EventPointAbstSO[] _seventhFloor;
        [SerializeField]
        EventPointAbstSO[] _eighthFloor;
        public EventPointAbstSO[] GetFloorEventPointSO(byte floor)
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