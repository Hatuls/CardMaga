using Map;
using Meta.Map;
using System.Collections.Generic;
using UnityEngine;
namespace UI.Map
{

    public class MapUIManager : MonoBehaviour
    {
        private static MapUIManager _instance;
        public static MapUIManager Instance => _instance;
        [SerializeField]  EventPointCollectionSO _eventPointCollection;
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


    public class MapManager
    {
        private static MapManager _instance;
        public static MapManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MapManager();
                return _instance;
            }
        }
        public MapManager()
        {
            List<EventPoint> eventList = new List<EventPoint>();
            const byte EighthFloor = 8;
            const byte SeventhFloor = 7;
            const byte SixthFloor = 6;
            const byte FifthFloor = 5;
            const byte FourthFloor = 4;
            const byte ThirdFloor = 3;
            const byte SecondFloor = 2;
            const byte FirstFloor = 1;
            const byte StartFloor = 0;

            //start floor 
   


            //TopPoint

            var bossPoint = new EventPoint(EighthFloor, EventPointType.Boss_Enemy);

            //RestPoint before Boss
            var restArea1 = new EventPoint(SeventhFloor, EventPointType.Rest_Area);


        }
         MapEventPoint[] _points;
      static   MapEventPoint _currentPoint= null;

        public static void ResetMap()
        {
            _currentPoint.reset
        }


        public static 
    }
}