
using System.Collections.Generic;
using UnityEngine;
namespace Map
{
    [CreateAssetMenu(fileName = "Map Config", menuName = "ScriptableObjects/Map/Map Config")]
    public class MapConfig : ScriptableObject
    {
        [SerializeField]
        NodePointAbstSO[] _nodesSO;

        public int GridWidth => Mathf.Max(_maxNumOfPreBossNodes, _maxNumOfStartingNodes);

        [SerializeField] int _minNumOfPreBossNodes;
        [SerializeField] int _maxNumOfPreBossNodes;
        public int NumOfPreBossNodes => Random.Range(_minNumOfPreBossNodes, _maxNumOfPreBossNodes + 1);
        [Space]
        [SerializeField] int _minNumOfStartingNodes;
        [SerializeField] int _maxNumOfStartingNodes;
        public int NumOfStartingNodes => Random.Range(_minNumOfStartingNodes, _maxNumOfStartingNodes + 1);
        [Space]
       
        public NodeLayer[] _nodeLayers;

        public NodePointAbstSO GetNodePoint(NodeType node)
        {
            for (int i = 0; i < _nodesSO.Length; i++)
            {
                if (node == _nodesSO[i].PointType)
                    return _nodesSO[i];
            }

            throw new System.Exception($"MapConfig: Node Was not found {node}");
        }
    }


    [System.Serializable]
    public class NodeLayer
    {
        [Header("Node:")]
        [SerializeField] NodeType _mainlyNode;
        [Space]

        [Header("Positioning:")]
        [Range(.5f, 1.5f)]
        [SerializeField] float _minDistanceFromPreviousLayer;
        [Range(.5f,1.5f)]
        [SerializeField] float _maxDistanceFromPreviousLayer;
        [Space]

        [Tooltip("Distance between the nodes on this layer")]
        [SerializeField]
        private float _nodesApartDistance;

        [Tooltip("If this is set to 0, nodes on this layer will appear in a straight line. Closer to 1f = more position randomization")]
        [Range(0f, 1f)] 
        [SerializeField]
        private float _randomizePosition;


        [Space]
        [Header("Node Randomization:")]
        [Tooltip("0 - mean it always be the mainlyNode")]
        [Range(0, 1)]
        [SerializeField] float _randomizeNode;

        public NodeType MainlyNode => _mainlyNode; 
        public float MinDistanceFromPreviousLayer  => _minDistanceFromPreviousLayer; 
        public float MaxDistanceFromPreviousLayer  => _maxDistanceFromPreviousLayer; 

        public float RandomizeDistanceFromPreviousLayer => Random.Range(_minDistanceFromPreviousLayer, _maxDistanceFromPreviousLayer);
        public float RandomizeNode  => _randomizeNode; 
        public float NodesApartDistance  => _nodesApartDistance; 
        public float RandomizePosition => _randomizePosition;

    }
}