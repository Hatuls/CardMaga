using Map;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace UI.Map
{
    public class MapView : MonoBehaviour
    {
      
        static MapView _instance;
        public static MapView Instance => _instance;

        [SerializeField] GameObject _nodePrefab;
        public Color32 lockedColor;
        public Color32 visitedColor;

        [SerializeField] float _xOffset;
        [SerializeField] float _yOffset;



      
        float _screenMaxX;
        float _screenMinX;
        float _screenMaxY;
        float _screenMinY;

        List<List<Node>> _layersNodeList = new List<List<Node>>();
        List<NodeMap> _nodeMaps = new List<NodeMap>();

        private void Awake()
        {
            _instance = this;
            AssignScreenCoordinates();
        }


        public void GenerateMap(MapConfig mapConfig)
        {
            // run on each floor and deploy amount of points
            int layerAmount = mapConfig._nodeLayers.Length;

            CleanPrevious();
            GenerateNodes(mapConfig, layerAmount);
            GeneratePaths();
            UnlockFirstFloor();
        }

        private void UnlockFirstFloor()
        {
            var firstFloor = _layersNodeList.First(innerList => innerList.Any(node => node.FloorLevel == 0));
            foreach (var node in firstFloor)
            {
              node.SetState(NodeStates.Attainable);
            }
        }

        private void GeneratePaths()
        {
            List<Node> currentFloorNode;
            List<Node> floorAbove;


            for (int i = 0; i < _layersNodeList.Count-1; i++)
            {
                currentFloorNode = _layersNodeList[i];
                floorAbove = _layersNodeList[i + 1];

                for (int j = 0; j < currentFloorNode.Count; j++)
                {
                    Node currentNode = currentFloorNode[j];

                    int goToIndex = j;
                    bool legalIndex;
                    do
                    {
                        goToIndex = RandomIndexToGoTo() + j;
         
                        legalIndex = IsIndexLegal(goToIndex, floorAbove);

                        if (legalIndex == false&& currentFloorNode.Count > floorAbove.Count)
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
                        List<Node> _layerNodeUnderMe = _layersNodeList[i - 1];
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


            //currentFloorNode = _layersNodeList[_layersNodeList.Count - 2];
            //floorAbove = _layersNodeList[_layersNodeList.Count - 1];
            //var bossNode = floorAbove[0];
            //for (int i = 0; i < currentFloorNode.Count; i++)
            //{
            //    currentFloorNode[i].ConnectTo.Add(floorAbove[0]);
            //    bossNode.ConnectFrom.Add(currentFloorNode[i]);
            //}


            bool IsIndexLegal(int index, List<Node> a) => index >= 0 && index < a.Count;

            int RandomIndexToGoTo() => Random.Range(-1, 2);
        }

        private void GenerateNodes(MapConfig mapConfig, int layerAmount)
        {

            for (int floorLevel = 0; floorLevel < layerAmount; floorLevel++)
                _layersNodeList.Add(GenerateLayerNodes(floorLevel, mapConfig));
        }
        private List<Node> GenerateLayerNodes(int floorLevel,MapConfig mapConfig)
        {     
           List<Node> currentLayerList = new List<Node>();
            var currentLayer = mapConfig._nodeLayers[floorLevel];
            int amountOfPoints = currentLayer.RandomAmountOfPoints;
       

            for (int j = 0; j < amountOfPoints; j++)
            {
                Vector3 pos = CalculateNodePosition(floorLevel, currentLayer, amountOfPoints, j);

                var node = Instantiate(_nodePrefab, pos, Quaternion.identity, this.transform);
                var NodeGFX = node.GetComponent<NodeMap>();

                bool getMainNode = Random.Range(0f, 1f) >= currentLayer.RandomizeNode;
                NodeType nodeType = getMainNode ? currentLayer.MainlyNode : (NodeType)Random.Range(1, (System.Enum.GetNames(typeof(NodeType)).Length - 1));

                Node n = new Node((byte)floorLevel, nodeType);

                NodeGFX.SetUp(n, mapConfig.GetNodePoint(nodeType));

                currentLayerList.Add(n);
                _nodeMaps.Add(NodeGFX);
            }
            return currentLayerList;
        }
        private Vector3 CalculateNodePosition(int floorLevel, NodeLayer currentLayer, int amountOfPoints, int currentNodeIndex)
        {
            float yNoise = floorLevel == 0 ? _yOffset : Random.Range((float)currentLayer.MinDistanceFromPreviousLayer, (float)currentLayer.MaxDistanceFromPreviousLayer);
            float yPos = (_screenMinY+_yOffset) + yNoise * floorLevel;
            

            float xNoiseOffset = _xOffset + currentLayer.RandomizeXPosition;
            float startPosition = currentNodeIndex * xNoiseOffset;


            float halfOfTheDistance = (amountOfPoints  == 1) ? 0 : ((amountOfPoints * _xOffset) / 2) - (_xOffset / 2);

            float xPos = startPosition - halfOfTheDistance;
            Vector3 pos = new Vector3(xPos, yPos);
            return pos;
        }
        private void CleanPrevious()
        {
            for (int i = 0; i < _nodeMaps.Count; i++)
            {
                Destroy(_nodeMaps[i].gameObject);
            }
            _nodeMaps.Clear();

            for (int i = 0; i < _layersNodeList.Count; i++)
                _layersNodeList[i].Clear();
            
            _layersNodeList.Clear();
        }

        private void AssignScreenCoordinates()
        {
            var bottomLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
            _screenMinY = bottomLeft.y;
            _screenMinX = bottomLeft.x;

           var topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            _screenMaxX = topRight.x;
            _screenMaxY = topRight.y;

          
        }


    }

}