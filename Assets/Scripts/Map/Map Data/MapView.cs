
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardMaga.Map
{



    public class MapView : MonoBehaviour
    {

        static MapView _instance;
        public static MapView Instance => _instance;

        [SerializeField] GameObject _nodePrefab;
        [SerializeField] GameObject _linePrefab;

        [SerializeField]
        MapManager _mapManager;


        public Color32 lockedColor;
        public Color32 visitedColor;

        [SerializeField] float _xOffset;
        [SerializeField] float _yOffset;

        [SerializeField]
        private GameObject _mapContainer;

        // List<LineConnectionPrefabs> _lineConnections = new List<LineConnectionPrefabs>();
        List<NodeMap> _nodeMaps = new List<NodeMap>();
        List<MapLine> _mapLines = new List<MapLine>();
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


            CreateNodes(map.nodes);

            SetAttainableNodes();

            DrawLines();

            //SetLineColors();

            //CreateMapBackground(map);
        }
        private void DrawLines()
        {
            int nodeMapLength = _nodeMaps.Count;
            Map map = MapManager.Instance.CurrentMap;
            Point p = null;
            for (int i = 0; i < nodeMapLength; i++)
            {
                var nodeData = _nodeMaps[i].NodeData;
                var connectedAmount = nodeData.incoming;
                int pointConnectedAmount = connectedAmount.Count;

                for (int j = 0; j < pointConnectedAmount; j++)
                {
                    p = connectedAmount[j];
                    MapLine mapLine = Instantiate(_linePrefab, _mapContainer.transform)
                             .GetComponent<MapLine>();
                    var Node = map.GetNode(p);

                    mapLine.SetColor(Node.NodeState);

                    mapLine.ConnectLines(nodeData.Position, Node.Position);
                    _mapLines.Add(mapLine);
                }
            }
        }


        public void SetAttainableNodes()
        {
            if (_mapManager.CurrentMap == null)
                return;
            // first set all the nodes as unattainable/locked:
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


        private void CreateNodes(List<Node> nodes)
        {
            var factoryInstance = Factory.GameFactory.Instance.EventPointFactoryHandler;

            for (int i = 0; i < nodes.Count; i++)
            {
                _nodeMaps.Add(CreateMapNode(nodes[i], factoryInstance));
            }
        }

        private NodeMap CreateMapNode(Node node, Factory.GameFactory.EventPointFactory eventFactoryRef)
        {
            var mapNodeObject = Instantiate(_nodePrefab, _mapContainer.transform);
            var mapNode = mapNodeObject.GetComponent<NodeMap>();
            var blueprint = eventFactoryRef.GetEventPoint(node.NodeTypeEnum);
            mapNode.SetUp(node, blueprint);
            //      mapNode.transform.localPosition = node.position;
            mapNode.transform.position = node.Position;
            return mapNode;
        }

        private void ClearMap()
        {
            for (int i = 0; i < _nodeMaps.Count; i++)
            {
                Destroy(_nodeMaps[i].gameObject);
            }
            for (int i = 0; i < _mapLines.Count; i++)
            {
                Destroy(_mapLines[i].gameObject);
            }

            _nodeMaps.Clear();
            _mapLines.Clear();
        }

        private void AssignScreenCoordinates()
        {
            var bottomLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
            ScreenCoordinates._screenMinY = bottomLeft.y;
            ScreenCoordinates._screenMinX = bottomLeft.x;

            var topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            ScreenCoordinates._screenMaxX = topRight.x;
            ScreenCoordinates._screenMaxY = topRight.y;

            ScreenCoordinates._xOffset = _xOffset;
            ScreenCoordinates._yOffset = _yOffset;
        }


    }
    public static class ScreenCoordinates
    {
        public static float _screenMaxX;
        public static float _screenMinX;
        public static float _screenMaxY;
        public static float _screenMinY;
        public static float _xOffset;
        public static float _yOffset;
    }
}