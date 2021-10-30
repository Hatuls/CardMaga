using Map;
using Meta.Map;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace UI.Map
{

    public class MapManager : MonoBehaviour
    {
        private static MapManager _instance;
        public static MapManager Instance => _instance;
        //   [SerializeField]  EventPointCollectionSO _eventPointCollection;
        [SerializeField] NodeMap[] _points;
        [SerializeField] NodeMap _currentPoint;
        [SerializeField]
        MapView _mapView;

        [SerializeField]
        MapConfig _mapCFG;
        [Sirenix.OdinInspector.Button()]
        public void GenerateMap() => _mapView.GenerateMap(_mapCFG);


        public void MapPointSelected(NodeMap selectedPoint, Node node)
        {
            _currentPoint = selectedPoint;
            var eventPoint = Factory.GameFactory.Instance.EventPointFactoryHandler.GetEventPoint(node.NodeTypeEnum);
            eventPoint.ActivatePoint();

        }

        public void CompleteBattle()
        {
            for (int i = 0; i < _points.Length; i++)
                _points[i].PointLockState(false);
        }
    }



    public class Map
    {
 

    }
}