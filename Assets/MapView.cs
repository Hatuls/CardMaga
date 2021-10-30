using Map;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Map
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





        public Color32 lockedColor;
        public Color32 visitedColor;

        [SerializeField] float _xOffset;
        [SerializeField] float _yOffset;




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
            CreateNodes(map.nodes);

            //DrawLines();

            //SetAttainableNodes();

            //SetLineColors();

            //CreateMapBackground(map);
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
            var mapNodeObject = Instantiate(_nodePrefab,this.transform);
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





    public static class MapGenerator
    {
        private static MapConfig _config;

        private static readonly List<NodeType> RandomNodes = new List<NodeType>
        {NodeType.Basic_Enemy, NodeType.Boss_Enemy, NodeType.Chest, NodeType.Dojo, NodeType.Elite_Enemy, NodeType.QuestionMark, NodeType.Rest_Area};

        private static List<float> _layerDistances;
        private static List<List<Point>> _paths;


        // ALL nodes by layer:
        private static readonly List<List<Node>> _nodes = new List<List<Node>>();

        public static Map GetMap(MapConfig conf)
        {
            if (conf == null)
            {
                Debug.LogWarning("Config was null in MapGenerator.Generate()");
                return null;
            }

            _config = conf;
            _nodes.Clear();

            GenerateLayerDistances();

            for (var i = 0; i < conf._nodeLayers.Length; i++)
             _nodes.Add(GenerateLayerNodes(i, _config));

            GeneratePaths();

            RandomizeNodePositions();

      //      RemoveCrossConnections();

            // select all the nodes with connections:
            var nodesList = _nodes.SelectMany(n => n).Where(n => n.ConnectTo.Count > 0 || n.ConnectFrom.Count > 0).ToList();

            // pick a random name of the boss level for this map:
          //  var bossNodeName = config.nodeBlueprints.Where(b => b.nodeType == NodeType.Boss).ToList().Random().name;
            return new Map(conf.name, nodesList, new List<Point>());
        }

      

        private static void RandomizeNodePositions()
        {
            for (var index = 0; index < _nodes.Count; index++)
            {
                var list = _nodes[index];
                var layer = _config._nodeLayers[index];
                var distToNextLayer = index + 1 >= _layerDistances.Count
                    ? 0f
                    : _layerDistances[index + 1];
                var distToPreviousLayer = _layerDistances[index];

                foreach (var node in list)
                {
                    var xRnd = Random.Range(-1f, 1f);
                    var yRnd = Random.Range(-1f, 1f);

                    var x = xRnd * layer.NodesApartDistance / 2f;
                    var y = yRnd < 0 ? distToPreviousLayer * yRnd / 2f : distToNextLayer * yRnd / 2f;

                    node.position += new Vector2(x, y) * layer.RandomizePosition;
                }
            }
        }

        private static List<Node> GenerateLayerNodes(int floorLevel, MapConfig mapConfig)
        {
            List<Node> currentLayerList = new List<Node>();
            var currentLayer = mapConfig._nodeLayers[floorLevel];
            int amountOfPoints = currentLayer.RandomAmountOfPoints;


            for (int j = 0; j < amountOfPoints; j++)
            {
                Vector3 pos = CalculateNodePosition(floorLevel, currentLayer, amountOfPoints, j);

         //       var node = Instantiate(_nodePrefab, pos, Quaternion.identity, this.transform);
        //        var NodeGFX = node.GetComponent<NodeMap>();

                bool getMainNode = Random.Range(0f, 1f) >= currentLayer.RandomizeNode;
                NodeType nodeType = getMainNode ? currentLayer.MainlyNode : (NodeType)Random.Range(1, (System.Enum.GetNames(typeof(NodeType)).Length - 1));

                Node n = new Node(nodeType, new Point(floorLevel, j))
                {
                    position = pos
                };
              //  NodeGFX.SetUp(n, mapConfig.GetNodePoint(nodeType));

                currentLayerList.Add(n);
                //_nodeMaps.Add(NodeGFX);
            }
            return currentLayerList;
        }
        private static Vector3 CalculateNodePosition(int floorLevel, NodeLayer currentLayer, int amountOfPoints, int currentNodeIndex)
        {
            //float yNoise = floorLevel == 0 ? ScreenSettings._yOffset : Random.Range((float)currentLayer.MinDistanceFromPreviousLayer, (float)currentLayer.MaxDistanceFromPreviousLayer);
            //float yPos = (ScreenSettings._screenMinY + ScreenSettings._yOffset) + yNoise * floorLevel;

            float nodesApartDistance = currentLayer.NodesApartDistance;

            var offset = nodesApartDistance * ScreenSettings._xOffset / 2;

            //float xNoiseOffset = ScreenSettings._xOffset + currentLayer.NodesApartDistance;
            float startPosition =-offset * currentNodeIndex * nodesApartDistance;//* //xNoiseOffset;

            float maxDistance = offset*amountOfPoints * nodesApartDistance;
            float halfOfTheDistance = maxDistance / 2;

            float xPos = startPosition+ halfOfTheDistance - nodesApartDistance / 4; //- halfOfTheDistance/2;
            Vector3 pos = new Vector3(xPos, GetDistanceToLayer(floorLevel));
            return pos;
        }

        private static float GetDistanceToLayer(int layerIndex)
        {
            if (layerIndex < 0 || layerIndex > _layerDistances.Count) return 0f;

            return _layerDistances.Take(layerIndex + 1).Sum();
        }

        private static  void GeneratePaths()
        {
            List<Node> currentFloorNode;
            List<Node> floorAbove;


            for (int i = 0; i < _nodes.Count - 1; i++)
            {
                currentFloorNode = _nodes[i];
                floorAbove = _nodes[i + 1];

                for (int j = 0; j < currentFloorNode.Count; j++)
                {
                    Node currentNode = currentFloorNode[j];

                    int goToIndex = j;
                    bool legalIndex;
                    do
                    {
                        goToIndex = RandomIndexToGoTo() + j;

                        legalIndex = IsIndexLegal(goToIndex, floorAbove);

                        if (legalIndex == false && currentFloorNode.Count > floorAbove.Count)
                        {
                            goToIndex = floorAbove.Count - 1;
                            legalIndex = true;
                        }
                    } while (!legalIndex); //|| currentNode.ConnectTo.Contains(floorAbove[goToIndex]));


                    Debug.Log("@ " + goToIndex);
                    if (currentNode.ConnectTo.Contains(floorAbove[goToIndex]) == false)
                        currentNode.ConnectTo.Add(floorAbove[goToIndex]);

                    if (floorAbove[goToIndex].ConnectFrom.Contains(currentNode) == false)
                        floorAbove[goToIndex].ConnectFrom.Add(currentNode);


                    if (i > 0 && currentNode.ConnectFrom.Count == 0)
                    {
                        List<Node> _layerNodeUnderMe = _nodes[i - 1];
                        do
                        {
                            goToIndex = RandomIndexToGoTo() + j;
                            legalIndex = IsIndexLegal(goToIndex, _layerNodeUnderMe);


                            if (legalIndex == false && currentFloorNode.Count > _layerNodeUnderMe.Count)
                            {
                                goToIndex = _layerNodeUnderMe.Count - 1;
                                legalIndex = IsIndexLegal(goToIndex, _layerNodeUnderMe);
                            }

                            Debug.Log("! " + goToIndex);

                        } while (!legalIndex);// currentNode.ConnectTo.Contains(_layerNodeUnderMe[goToIndex]));

                        Debug.Log("! " + goToIndex);
                        if (currentNode.ConnectFrom.Contains(_layerNodeUnderMe[goToIndex]) == false)
                            currentNode.ConnectFrom.Add(_layerNodeUnderMe[goToIndex]);

                        if (_layerNodeUnderMe[goToIndex].ConnectTo.Contains(currentNode) == false)
                            _layerNodeUnderMe[goToIndex].ConnectTo.Add(currentNode);
                    }

                }
            }

            bool IsIndexLegal(int index, List<Node> a) => index >= 0 && index < a.Count;

            int RandomIndexToGoTo() => Random.Range(-1, 2);
        }
        private static void GenerateLayerDistances()
        {
            int layers = _config._nodeLayers.Length;
            _layerDistances = new List<float>(layers);

            for (int i = 0; i < layers; i++)
                _layerDistances.Add(_config._nodeLayers[i].RandomizeDistanceFromPreviousLayer);
        }
    }
}