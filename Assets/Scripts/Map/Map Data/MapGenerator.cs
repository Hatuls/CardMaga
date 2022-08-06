
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CardMaga.Map
{
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
             _nodes.Add(GenerateLayerNodes(i));

            GeneratePaths();

            RandomizeNodePositions();

            SetUpConnections();


              RemoveCrossConnections();

            // select all the nodes with connections:
            var nodesList = _nodes.SelectMany(n => n).Where(n => n.incoming.Count > 0 || n.outgoing.Count > 0).ToList();

            return new Map(conf.name, nodesList, new List<Point>());
        }

        private static void RemoveCrossConnections()
        {
            for (var i = 0; i < _config.GridWidth - 1; i++)
                for (var j = 0; j < _config._nodeLayers.Length - 1; j++)
                {
                    var node = GetNode(new Point(i, j));
                    if (node == null || node.HasNoConnections()) continue;
                    var right = GetNode(new Point(i + 1, j));
                    if (right == null || right.HasNoConnections()) continue;
                    var top = GetNode(new Point(i, j + 1));
                    if (top == null || top.HasNoConnections()) continue;
                    var topRight = GetNode(new Point(i + 1, j + 1));
                    if (topRight == null || topRight.HasNoConnections()) continue;

                    // Debug.Log("Inspecting node for connections: " + node.point);
                    if (!node.outgoing.Any(element => element.Equals(topRight.point))) continue;
                    if (!right.outgoing.Any(element => element.Equals(top.point))) continue;

                    // Debug.Log("Found a cross node: " + node.point);

                    // we managed to find a cross node:
                    // 1) add direct connections:
                    node.AddOutgoing(top.point);
                    top.AddIncoming(node.point);

                    right.AddOutgoing(topRight.point);
                    topRight.AddIncoming(right.point);

                    var rnd = Random.Range(0f, 1f);
                    if (rnd < 0.2f)
                    {
                        // remove both cross connections:
                        // a) 
                        node.RemoveOutgoing(topRight.point);
                        topRight.RemoveIncoming(node.point);
                        // b) 
                        right.RemoveOutgoing(top.point);
                        top.RemoveIncoming(right.point);
                    }
                    else if (rnd < 0.6f)
                    {
                        // a) 
                        node.RemoveOutgoing(topRight.point);
                        topRight.RemoveIncoming(node.point);
                    }
                    else
                    {
                        // b) 
                        right.RemoveOutgoing(top.point);
                        top.RemoveIncoming(right.point);
                    }
                }
        }
        private static void SetUpConnections()
        {
            foreach (var path in _paths)
            {
                for (var i = 0; i < path.Count; i++)
                {
                    var node = GetNode(path[i]);

                    if (i > 0)
                    {
                        // previous because the path is flipped
                        var nextNode = GetNode(path[i - 1]);
                        nextNode.AddIncoming(node.point);
                        node.AddOutgoing(nextNode.point);
                    }

                    if (i < path.Count - 1)
                    {
                        var previousNode = GetNode(path[i + 1]);
                        previousNode.AddOutgoing(node.point);
                        node.AddIncoming(previousNode.point);
                    }
                }
            }
        }
        private static List<Node> GenerateLayerNodes(int floorLevel)
        {
            List<Node> currentLayerList = new List<Node>();
            var currentLayer = _config._nodeLayers[floorLevel];
            int amountOfPoints = _config.GridWidth;


            for (int j = 0; j < amountOfPoints; j++)    
            {
                Vector3 pos = CalculateNodePosition(floorLevel, currentLayer, amountOfPoints, j);

                bool getMainNode = Random.Range(0f, 1f) >= currentLayer.RandomizeNode;
                NodeType nodeType = getMainNode ? currentLayer.MainlyNode : (NodeType)Random.Range(1, (System.Enum.GetNames(typeof(NodeType)).Length - 1));

                Node n = new Node(nodeType, new Point(j, floorLevel), pos);
   

                currentLayerList.Add(n);
       
            }
            return currentLayerList;
        }
        private static Vector3 CalculateNodePosition(int floorLevel, NodeLayer currentLayer, int amountOfPoints, int currentNodeIndex)
        {

            float nodesApartDistance = currentLayer.NodesApartDistance;

        //    var offset = nodesApartDistance * ScreenSettings._xOffset / 2;
            var offset = currentLayer.NodesApartDistance * _config.GridWidth / 2f;

            float startPosition =-offset * currentNodeIndex * nodesApartDistance;

            float maxDistance = offset*amountOfPoints * nodesApartDistance;
            float halfOfTheDistance = maxDistance / 2;

            float smallOffsetToCenter = nodesApartDistance / 4;
//
      //      float xPos = startPosition+ halfOfTheDistance - smallOffsetToCenter;
            Vector3 pos = new Vector3(-offset + currentNodeIndex * currentLayer.NodesApartDistance  + smallOffsetToCenter, GetDistanceToLayer(floorLevel));
            return pos;
        }
        private static float GetDistanceToLayer(int layerIndex)
        {
            float startPos = ScreenCoordinates._screenMinY + ScreenCoordinates._yOffset;
            if (layerIndex ==0)
                return startPos;

           return  _layerDistances.Take(layerIndex + 1).Sum();
        }
        private static Node GetNode(Point p)
        {
            if (p.y >= _nodes.Count) return null;
            if (p.x >= _nodes[p.y].Count) return null;

            return _nodes[p.y][p.x];
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

                    node.RandomizePosition( new Vector2(x, y) * layer.RandomizePosition);
                }
            }
        }
        private static void Shuffle<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        private static void GeneratePaths()
        {
            var finalNode = GetFinalNode();
            _paths = new List<List<Point>>();
            var numOfStartingNodes = _config.NumOfStartingNodes;
            var numOfPreBossNodes = _config.NumOfPreBossNodes;

            var candidateXs = new List<int>();
            for (var i = 0; i < _config.GridWidth; i++)
                candidateXs.Add(i);

            candidateXs.Shuffle();
            var preBossXs = candidateXs.Take(numOfPreBossNodes);
            var preBossPoints = (from x in preBossXs select new Point(x, finalNode.y - 1)).ToList();
            var attempts = 0;

            // start by generating paths from each of the preBossPoints to the 1st layer:
            foreach (var point in preBossPoints)
            {
                var path = Path(point, 0, _config.GridWidth);
                path.Insert(0, finalNode);
                _paths.Add(path);
                attempts++;
            }

            while (!PathsLeadToAtLeastNDifferentPoints(_paths, numOfStartingNodes) && attempts < 100)
            {
                var randomPreBossPoint = preBossPoints[UnityEngine.Random.Range(0, preBossPoints.Count)];
                var path = Path(randomPreBossPoint, 0, _config.GridWidth);
                path.Insert(0, finalNode);
                _paths.Add(path);
                attempts++;
            }

            Debug.Log("Attempts to generate paths: " + attempts);
        }

        private static bool PathsLeadToAtLeastNDifferentPoints(IEnumerable<List<Point>> paths, int n)
        {
            return (from path in paths select path[path.Count - 1].x).Distinct().Count() >= n;
        }

        private static List<Point> Path(Point from, int toY, int width, bool firstStepUnconstrained = false)
        {
            if (from.y == toY)
            {
                Debug.LogError("Points are on same layers, return");
                return null;
            }

            // making one y step in this direction with each move
            var direction = from.y > toY ? -1 : 1;

            var path = new List<Point> { from };
            while (path[path.Count - 1].y != toY)
            {
                var lastPoint = path[path.Count - 1];
                var candidateXs = new List<int>();
                if (firstStepUnconstrained && lastPoint.Equals(from))
                {
                    for (var i = 0; i < width; i++)
                        candidateXs.Add(i);
                }
                else
                {
                    // forward
                    candidateXs.Add(lastPoint.x);
                    // left
                    if (lastPoint.x - 1 >= 0) candidateXs.Add(lastPoint.x - 1);
                    // right
                    if (lastPoint.x + 1 < width) candidateXs.Add(lastPoint.x + 1);
                }

                var nextPoint = new Point(candidateXs[Random.Range(0, candidateXs.Count)], lastPoint.y + direction);
                path.Add(nextPoint);
            }

            return path;
        }

        private static void GenerateLayerDistances()
        {
            int layers = _config._nodeLayers.Length;
            _layerDistances = new List<float>(layers);
            float startPos = ScreenCoordinates._screenMinY + ScreenCoordinates._yOffset;

            for (int i = 0; i < layers; i++)
                _layerDistances.Add((i == 0) ? startPos : _config._nodeLayers[i].RandomizeDistanceFromPreviousLayer);
        }

        private static Point GetFinalNode()
        {
            var y = _config._nodeLayers.Length - 1;
            if (_config.GridWidth % 2 == 1)
                return new Point(_config.GridWidth / 2, y);

            return Random.Range(0, 2) == 0
                ? new Point(_config.GridWidth / 2, y)
                : new Point(_config.GridWidth / 2 - 1, y);
        }
    }
}