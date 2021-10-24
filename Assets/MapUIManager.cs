using Map;
using Meta.Map;
using UnityEngine;
namespace UI.Map
{

    public class MapUIManager : MonoBehaviour
    {
        private static MapUIManager _instance;
        public static MapUIManager Instance => _instance;
        [SerializeField]
        EventPointCollectionSO _eventPointCollection;
        [SerializeField] MapEventPoint[] _points;
        [SerializeField] byte _chestFloor;
        [SerializeField] MapEventPoint _currentPoint;

        private void Awake()
        {
            ResetPoints();
            _instance = this;

        }

        public void ResetPoints()
        {
            _currentPoint = null;
            for (int i = 0; i < _points.Length; i++)
            {
                _points[i].Init(_eventPointCollection.EventPoints[Random.Range(0, _eventPointCollection.EventPoints.Length)]);
            }
        }

      public void MapPointSelected(MapEventPoint selectedPoint)
        {
            _currentPoint = selectedPoint;
         
        }

        public void CompleteBattle()
        {
            for (int i = 0; i < _points.Length; i++)
                _points[i].PointLockState(false);
           
            var points = _currentPoint.ConnectTo;
            var length = points.Length;
            _currentPoint.PointLockState(false);
            for (int i = 0; i < length; i++)
                points[i].PointLockState(true);
            
        }
    }


    public static class MapManager
    {

        static MapEventPoint[] _points;
    }
}