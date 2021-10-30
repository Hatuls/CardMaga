
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Map
{


    public static class ScreenSettings
    {
        public static float _screenMaxX;
        public static float _screenMinX;
        public static float _screenMaxY;
        public static float _screenMinY;
        public static float _xOffset;
        public static float _yOffset;
    }
    public class MapView : MonoBehaviour
    {
      
        static MapView _instance;
        public static MapView Instance => _instance;

        [SerializeField] GameObject _nodePrefab;


        [SerializeField]
        MapManager _mapManager;


        public Color32 lockedColor;
        public Color32 visitedColor;

        [SerializeField] float _xOffset;
        [SerializeField] float _yOffset;

        private GameObject firstParent;
        private GameObject mapParent;


        // List<LineConnectionPrefabs> _lineConnections = new List<LineConnectionPrefabs>();
        List<NodeMap> _nodeMaps = new List<NodeMap>();

        private void Awake()
        {
            _instance = this;
            AssignScreenCoordinates();
        }


    

        public void ShowMap(Map map)
        {
            if (map == null)
            {
                Debug.LogWarning("Map was null in MapView.ShowMap()");
                return;
            }

            ClearMap();

            CreateMapParent();


            CreateNodes(map.nodes);

            //DrawLines();

            SetAttainableNodes();

            //SetLineColors();

            //CreateMapBackground(map);
        }

        private void SetAttainableNodes()
        {
            foreach (var node in _nodeMaps)
                node.SetState(NodeStates.Locked);

            if (_mapManager.CurrentMap.path.Count == 0)
            {
                // we have not started traveling on this map yet, set entire first layer as attainable:
                foreach (var node in _nodeMaps.Where(n => n.NodeData.point.y == 0))
                    node.SetState(NodeStates.Attainable);
            }
            else
            {
                // we have already started moving on this map, first highlight the path as visited:
                foreach (var point in _mapManager.CurrentMap.path)
                {
                    var mapNode = GetNode(point);
                    if (mapNode != null)
                        mapNode.SetState(NodeStates.Visited);
                }

                var currentPoint = _mapManager.CurrentMap.path[_mapManager.CurrentMap.path.Count - 1];
                var currentNode = _mapManager.CurrentMap.GetNode(currentPoint);

                // set all the nodes that we can travel to as attainable:
                foreach (var point in currentNode.outgoing)
                {
                    var mapNode = GetNode(point);
                    if (mapNode != null)
                        mapNode.SetState(NodeStates.Attainable);
                }
            }

            NodeMap GetNode(Point p) => _nodeMaps.FirstOrDefault(n => n.NodeData.point.Equals(p));
        }
        private void CreateMapParent()
        {
            if (firstParent == null)
            {

            firstParent = new GameObject("OuterMapParent");
            mapParent = new GameObject("MapParentWithAScroll");
            mapParent.transform.SetParent(firstParent.transform);
            }

        }

        private void CreateNodes(List<Node> nodes)
        {
            var factoryInstance = Factory.GameFactory.Instance.EventPointFactoryHandler;

            for (int i = 0; i < nodes.Count; i++)
            {
                _nodeMaps.Add(CreateMapNode(nodes[i], factoryInstance));
            }
        }

        private NodeMap CreateMapNode(Node node,Factory.GameFactory.EventPointFactory eventFactoryRef)
        {
            var mapNodeObject = Instantiate(_nodePrefab, mapParent.transform);
            var mapNode = mapNodeObject.GetComponent<NodeMap>();
            var blueprint =eventFactoryRef.GetEventPoint(node.NodeTypeEnum);
            mapNode.SetUp(node, blueprint);
            //      mapNode.transform.localPosition = node.position;
            mapNode.transform.position = node.position;
            return mapNode;
        }

        private  void ClearMap()
        {
            for (int i = 0; i < _nodeMaps.Count; i++)
            {
                Destroy(_nodeMaps[i].gameObject);
            }


            //_lineConnections.Clear();
            _nodeMaps.Clear();

        }
 
        private void AssignScreenCoordinates()
        {
            var bottomLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
            ScreenSettings. _screenMinY = bottomLeft.y;
            ScreenSettings. _screenMinX = bottomLeft.x;

           var topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            ScreenSettings._screenMaxX = topRight.x;
            ScreenSettings._screenMaxY = topRight.y;

            ScreenSettings._xOffset = _xOffset;
            ScreenSettings._yOffset = _yOffset;
        }


    }
}