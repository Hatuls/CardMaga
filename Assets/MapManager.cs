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

        Map _currentMap;


        [SerializeField]
        MapView _mapView;

        [SerializeField]
        MapConfig _mapCFG;

        [Sirenix.OdinInspector.Button()]
        public void GenerateMap() => GenerateMap(_mapCFG);
        public void GenerateMap(MapConfig mapConfig)
        {
            _currentMap = MapGenerator.GetMap(mapConfig);
            _mapView.ShowMap(_currentMap);

        }

        public void SaveMap()
        {
            if (_currentMap == null) return;

            var json = JsonUtilityHandler.ConvertObjectToJson(_currentMap);
            PlayerPrefs.SetString("Map", json);
            PlayerPrefs.Save();
        }
    }



    public class Map
    {
        public List<Node> nodes; // template of the map (the whole map nodes)

        public List<Point> path; // players path

        public string configName;
        public Map(string configName,List<Node> nodes, List<Point> path)
        {
            this.configName = configName;
            this.nodes = nodes;
            this.path = path;
        }

        public string ToJson()
        =>  Rei.Utilities.JsonUtilityConverter.ConvertToJson(this);
    }
}